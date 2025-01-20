using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShapes : MonoBehaviour
{
    public GameObject[] shapes;
    private int currentShapeIndex = 0;
    private GameObject currentShape;
    private Coroutine spinCoroutine;

    private Vector3 originalPosition;
    private Vector3 originalScale;
    private bool canChangeShape = true;

    public bool isGameOver = false;

    [Header("Animation Settings")]
    [SerializeField] private float animationDuration = 0.5f;
    [SerializeField] private float spinAngle = 360f;
    [SerializeField] private float bounceHeight = 0.2f;
    [SerializeField] private float maxScaleMultiplier = 1.2f;

    [Header("Audio Settings")]
    public AudioSource audioSource;
    public AudioClip[] changeShapeSounds;

    void Start()
    {
        originalPosition = transform.position;
        originalScale = transform.localScale;
        ChangeShape(0);
    }

    void ChangeShape(int newShapeIndex)
    {
        if (!canChangeShape || isGameOver) return;

        if (spinCoroutine != null)
            StopCoroutine(spinCoroutine);

        if (currentShape != null)
            Destroy(currentShape);

        currentShapeIndex = newShapeIndex;
        currentShape = Instantiate(shapes[currentShapeIndex], transform.position, Quaternion.identity, transform);

        ShapeBehavior shapeBehavior = currentShape.GetComponent<ShapeBehavior>();
        if (shapeBehavior != null)
        {
            shapeBehavior.Initialize(true);
        }

        PlayRandomSound();
        spinCoroutine = StartCoroutine(SpinBounceAndScaleShape());
        StartCoroutine(ShapeChangeCooldown());
    }

    public string GetCurrentShapeName()
    {
        return shapes[currentShapeIndex].name;
    }

    private void OnMouseDown()
    {
        if (!isGameOver)
        {
            ChangeShape((currentShapeIndex + 1) % shapes.Length);
        }
    }

    private IEnumerator SpinBounceAndScaleShape()
    {
        float elapsedTime = 0f;

        while (elapsedTime < animationDuration)
        {
            float t = elapsedTime / animationDuration;

            float angle = Mathf.Lerp(0, spinAngle, t);
            transform.localRotation = Quaternion.Euler(0, angle, 0);

            float bounce = Mathf.Sin(t * Mathf.PI) * bounceHeight;
            transform.position = originalPosition + new Vector3(0, bounce, 0);

            float scaleMultiplier = Mathf.Lerp(1, maxScaleMultiplier, Mathf.Sin(t * Mathf.PI));
            transform.localScale = originalScale * scaleMultiplier;

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        transform.localRotation = Quaternion.identity;
        transform.position = originalPosition;
        transform.localScale = originalScale;
    }

    private IEnumerator ShapeChangeCooldown()
    {
        canChangeShape = false;
        yield return new WaitForSeconds(animationDuration);
        canChangeShape = true;
    }
    private void PlayRandomSound()
    {
        if (changeShapeSounds.Length > 0 && audioSource != null)
        {
            int randomIndex = Random.Range(0, changeShapeSounds.Length);
            audioSource.PlayOneShot(changeShapeSounds[randomIndex]);
        }
    }
}
