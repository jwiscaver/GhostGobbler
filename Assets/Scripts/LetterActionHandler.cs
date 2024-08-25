using UnityEngine;
using System.Collections;
using TMPro;

public class LetterActionHandler : MonoBehaviour
{
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
            Debug.Log("Logic for G triggered. Hiding the parent’s visual components and showing the child.");
            Transform child = clickedObject.transform.GetChild(0);  // Assuming child is at index 0
            if (child != null)
            {
                child.gameObject.SetActive(true);  // Activate the child
            }

            StartCoroutine(HideParentVisualsForTime(clickedObject, 5f));  // Hide visuals for 5 seconds
            break;

        case "G2":
            Debug.Log("Logic for second G triggered. Hiding the parent’s visual components and showing the child.");
            Transform secondChild = clickedObject.transform.GetChild(0);  // Assuming child is at index 0
            if (secondChild != null)
            {
                secondChild.gameObject.SetActive(true);  // Activate the child
            }

            StartCoroutine(HideParentVisualsForTime(clickedObject, 5f));  // Hide visuals for 5 seconds
            break;

        // Other cases...

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
}
