using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using TMPro;

public class LetterActionHandler : MonoBehaviour
{

public GrayscaleController grayscaleController;

    private Dictionary<GameObject, float> originalFontSizes = new Dictionary<GameObject, float>();

    private void OnEnable()
    {
        // Subscribe to the event when the script is enabled
        LetterClickHandler.OnClickedOnLinkEvent += HandleLinkClick;
    }

    private void OnDisable()
    {
        // Unsubscribe from the event when the script is disabled
        LetterClickHandler.OnClickedOnLinkEvent -= HandleLinkClick;
    }

    private void HandleLinkClick(string clickedLink, GameObject clickedObject)
    {
        Debug.Log("Link clicked: " + clickedLink);

        switch (clickedLink)
        {
            case "G":
               
                Transform child = clickedObject.transform.GetChild(0);  // Assuming child is at index 0
                if (child != null)
                {
                    child.gameObject.SetActive(true);  // Activate the child
                }

                StartCoroutine(HideParentVisualsForTime(clickedObject, 5f));  // Hide visuals for 5 seconds
                break;

            case "G2":
               
                Transform secondChild = clickedObject.transform.GetChild(0);  // Assuming child is at index 0
                if (secondChild != null)
                {
                    secondChild.gameObject.SetActive(true);  // Activate the child
                }

                StartCoroutine(HideParentVisualsForTime(clickedObject, 5f));  // Hide visuals for 5 seconds
                break;

            case "H":
                Debug.Log("Logic for H triggered");
                // Add your action for "H" here
                break;

        case "O":
        case "O2":
            Debug.Log("Logic for O triggered. Toggling text size.");

            TMP_Text textComponent = clickedObject.GetComponent<TMP_Text>();
            if (textComponent != null)
            {
                // Check if the object is already enlarged
                if (!originalFontSizes.ContainsKey(clickedObject))
                {
                    // Store the original font size
                    originalFontSizes[clickedObject] = textComponent.fontSize;

                    // Double the font size
                    textComponent.fontSize *= 2;
                }
                else
                {
                    // Revert to the original font size
                    textComponent.fontSize = originalFontSizes[clickedObject];

                    // Remove the entry from the dictionary as it's back to normal size
                    originalFontSizes.Remove(clickedObject);
                }
            }
            break;

            case "S":
                Debug.Log("Logic for S triggered");
                // Add your action for "S" here
                break;

            case "T":
                StartCoroutine(FallAndReturn(clickedObject));
                break;

            case "B":
            case "B2":
                //Debug.Log("Logic for B triggered. Rotating Y axis by 180 degrees.");
                StartCoroutine(RotateAndReturn(clickedObject));
                break;

            case "L":
                grayscaleController.ToggleGrayscale();
                break;

            case "E":
                Debug.Log("Logic for E triggered");
                // Add your action for "E" here
                break;

            case "R":
                Debug.Log("Logic for R triggered");
                // Add your action for "R" here
                break;

            default:
                Debug.Log("Unknown letter clicked: " + clickedLink);
                break;
        }
    }

    private IEnumerator HideParentVisualsForTime(GameObject parent, float delay)
    {
        // Disable the TMP_Text component to hide the text without disabling the entire object
        TMP_Text textComponent = parent.GetComponent<TMP_Text>();
        if (textComponent != null)
        {
            textComponent.enabled = false;  // Hide the text
        }

        // Disable the Renderer if there is any visual (like a 3D object or Sprite)
        Renderer renderer = parent.GetComponent<Renderer>();
        if (renderer != null)
        {
            renderer.enabled = false;  // Hide the visual object
        }

        // Wait for the specified time (5 seconds)
        yield return new WaitForSeconds(delay);

        // Re-enable the components after the delay
        if (textComponent != null)
        {
            textComponent.enabled = true;  // Show the text again
        }

        if (renderer != null)
        {
            renderer.enabled = true;  // Show the visual object again
        }

        // Deactivate the child after making the parent visible again
        Transform child = parent.transform.GetChild(0);  // Assuming child is at index 0
        if (child != null)
        {
            child.gameObject.SetActive(false);  // Deactivate the child
        }
    }

    private IEnumerator FallAndReturn(GameObject letterObject)
{
    // Get the RectTransform of the letter
    RectTransform letterRect = letterObject.GetComponent<RectTransform>();

    // Store the original local position of the letter
    Vector3 originalPosition = letterRect.localPosition;

    // Find the Dasher image in the canvas
    GameObject dasherObject = GameObject.Find("DasherHitPoint");
    if (dasherObject == null)
    {
        Debug.LogError("Dasher image not found in the scene.");
        yield break;
    }

    RectTransform dasherRect = dasherObject.GetComponent<RectTransform>();

    // Convert Dasher's world position to the local position relative to the parent of the letter
    Vector3 dasherLocalPosition = letterRect.parent.InverseTransformPoint(dasherRect.position);

    // Falling speed and threshold for stopping
    float fallSpeed = 650f;  // Adjust speed as necessary
    float distanceThreshold = 10f;  // Distance to stop "collision"

    // Fall until the letter reaches the Dasher's position
    while (Vector3.Distance(letterRect.localPosition, dasherLocalPosition) > distanceThreshold)
    {
        // Move the letter downward towards the Dasher's position (in local space)
        letterRect.localPosition = Vector3.MoveTowards(letterRect.localPosition, dasherLocalPosition, fallSpeed * Time.deltaTime);

        // Wait for the next frame
        yield return null;
    }

    // Ensure the letter reaches exactly at Dasher's position
    letterRect.localPosition = dasherLocalPosition;

    // Pause briefly before going back up (optional)
    yield return new WaitForSeconds(0.5f);  // Optional delay before moving up

    // Move the letter back up to the original position at the same speed
    while (Vector3.Distance(letterRect.localPosition, originalPosition) > distanceThreshold)
    {
        // Move the letter upward towards the original position (in local space)
        letterRect.localPosition = Vector3.MoveTowards(letterRect.localPosition, originalPosition, fallSpeed * Time.deltaTime);

        // Wait for the next frame
        yield return null;
    }

    // Ensure the letter reaches the exact original position
    letterRect.localPosition = originalPosition;
}

private IEnumerator RotateAndReturn(GameObject letterObject)
{
    // Store the original rotation
    Quaternion originalRotation = letterObject.transform.rotation;

    // Rotate the object by 180 degrees on the Y axis
    letterObject.transform.rotation = Quaternion.Euler(0, 180, 0);

    // Wait for 5 seconds
    yield return new WaitForSeconds(5f);

    // Revert the rotation back to the original
    letterObject.transform.rotation = originalRotation;
}



}
