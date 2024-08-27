using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class GrayscaleController : MonoBehaviour
{
    public PostProcessVolume postProcessVolume;
    private ColorGrading colorGradingLayer;
    private bool isGrayscale = false;  // Tracks if grayscale is currently enabled

    private void Start()
    {
        // Get the ColorGrading layer from the PostProcessVolume
        postProcessVolume.profile.TryGetSettings(out colorGradingLayer);

        // Ensure grayscale is off by default (saturation at 0)
        if (colorGradingLayer != null)
        {
            colorGradingLayer.saturation.value = 0f;  // Full color by default
        }
    }

    public void ToggleGrayscale()
    {
        if (colorGradingLayer != null)
        {
            // Toggle between grayscale and normal color
            if (isGrayscale)
            {
                // Disable grayscale
                colorGradingLayer.saturation.value = 0f;  // Full color
                isGrayscale = false;
            }
            else
            {
                // Enable grayscale
                colorGradingLayer.saturation.value = -100f;  // Grayscale
                isGrayscale = true;
            }
        }
    }
}