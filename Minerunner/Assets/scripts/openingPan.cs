using UnityEngine;
using UnityEngine.UI;

public class CinematicFlyover : MonoBehaviour
{
    public Camera flyoverCamera;
    public Camera mainCamera;

    public Transform lookAtTarget; 

    public Vector3 startFocusPosition = new Vector3(11f, 0f, -3f);
    public Vector3 endZoomOutPosition = new Vector3(11f, 7.5f, -3f);
    public Vector3 finalPanPosition = new Vector3(-0.3f, 7.5f, -3.5f);
    public Vector3 finalPanRotationEuler = new Vector3(62.67f, 0.239f, -0.001f);

    public float zoomOutDuration = 2f;
    public float panDuration = 3f;
    public float curveAmount = 3f;

    public Image fadeImage;
    public float fadeDuration = 1f;

    private float timer = 0f;
    private bool isZooming = true;
    private bool isPanning = false;
    private bool isFadingOut = false;
    private bool hasFinished = false;
    private Quaternion finalPanRotation;
    private gameMaster gameMaster;

    void Start()
    {
        flyoverCamera.enabled = true;
        mainCamera.enabled = false;

        flyoverCamera.transform.position = startFocusPosition;
        flyoverCamera.transform.LookAt(lookAtTarget);

        finalPanRotation = Quaternion.Euler(finalPanRotationEuler);

        if (fadeImage != null)
        {
            var c = fadeImage.color;
            c.a = 1f;
            fadeImage.color = c;
        }

        gameMaster = FindObjectOfType<gameMaster>();
    }

    void Update()
    {
        gameMaster.isPaused = true;
        if (hasFinished)
        {
            gameMaster.isPaused = false;
            return;
        }

        if (isZooming)
        {
            timer += Time.deltaTime;
            float t = SmoothStep(Mathf.Clamp01(timer / zoomOutDuration));

            flyoverCamera.transform.position = Vector3.Lerp(startFocusPosition, endZoomOutPosition, t);
            flyoverCamera.transform.LookAt(lookAtTarget);

            if (fadeImage != null && timer < fadeDuration)
                FadeIn();

            if (t >= 1f)
            {
                isZooming = false;
                isPanning = true;
                timer = 0f;
            }
        }
        else if (isPanning)
        {
            timer += Time.deltaTime;
            float t = SmoothStep(Mathf.Clamp01(timer / panDuration));

            // Cinematic curve movement
            Vector3 midpoint = (endZoomOutPosition + finalPanPosition) * 0.5f + Vector3.up * curveAmount;
            Vector3 m1 = Vector3.Lerp(endZoomOutPosition, midpoint, t);
            Vector3 m2 = Vector3.Lerp(midpoint, finalPanPosition, t);
            flyoverCamera.transform.position = Vector3.Lerp(m1, m2, t);

            // 🔥 Slerp to final rotation smoothly
            flyoverCamera.transform.rotation = Quaternion.Slerp(flyoverCamera.transform.rotation, finalPanRotation, t);

            if (t >= 1f)
            {
                isPanning = false;
                isFadingOut = true;
                timer = 0f;
            }
        }
        else if (isFadingOut)
        {
            timer += Time.deltaTime;

            if (fadeImage != null)
                FadeOut();

            if (timer >= fadeDuration)
            {
                EndFlyover();
            }
        }
    }

    void FadeIn()
    {
        var c = fadeImage.color;
        c.a = Mathf.Lerp(1f, 0f, timer / fadeDuration);
        fadeImage.color = c;
    }

    void FadeOut()
    {
        var c = fadeImage.color;
        c.a = Mathf.Lerp(0f, 1f, timer / fadeDuration);
        fadeImage.color = c;
    }

    void EndFlyover()
    {
        flyoverCamera.enabled = false;
        mainCamera.enabled = true;

        if (fadeImage != null)
        {
            var c = fadeImage.color;
            c.a = 0f;
            fadeImage.color = c;
        }

        hasFinished = true;
    }

    float SmoothStep(float t)
    {
        return t * t * (3f - 2f * t);
    }
}
