using UnityEngine;

/// <summary>
/// Locks the camera to a specific aspect ratio (default 16:9) by adding black bars.
/// </summary>
[RequireComponent(typeof(Camera))]
public class FixedAspectRatio : MonoBehaviour
{
    [SerializeField] private float targetAspectRatio = 16f / 9f;

    private void Start()
    {
        UpdateViewport();
    }

    private void UpdateViewport()
    {
        float screenAspect = (float)Screen.width / Screen.height;
        float scale = screenAspect / targetAspectRatio;

        Camera cam = GetComponent<Camera>();
        Rect rect = cam.rect;

        if (scale < 1.0f)
        {
            // Screen is taller → letterbox
            rect.width = 1.0f;
            rect.height = scale;
            rect.x = 0;
            rect.y = (1.0f - scale) / 2.0f;
        }
        else
        {
            // Screen is wider → pillarbox
            float invScale = 1.0f / scale;
            rect.width = invScale;
            rect.height = 1.0f;
            rect.x = (1.0f - invScale) / 2.0f;
            rect.y = 0;
        }

        cam.rect = rect;
    }

    // Re-apply if the window size changes (e.g., in windowed mode)
    private void OnPreCull()
    {
        GL.Clear(true, true, Color.black);
    }
}
