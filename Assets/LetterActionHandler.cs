using UnityEngine;

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

    // Method to handle the click event
    private void HandleLinkClick(string clickedLink)
    {
        Debug.Log("Link clicked: " + clickedLink);

        // Perform different actions based on which letter was clicked
        switch (clickedLink)
        {
            case "G":
                Debug.Log("Logic for G triggered");
                // Add your action for "G" here
                break;

            case "H":
                Debug.Log("Logic for H triggered");
                // Add your action for "H" here
                break;

            case "O":
            case "O2":
                Debug.Log("Logic for O triggered");
                // Add your action for "O" and "O2" here
                break;

            case "S":
                Debug.Log("Logic for S triggered");
                // Add your action for "S" here
                break;

            case "T":
                Debug.Log("Logic for T triggered");
                // Add your action for "T" here
                break;

            case "G2":
                Debug.Log("Logic for second G triggered");
                // Add your action for "G2" here
                break;

            case "B":
            case "B2":
                Debug.Log("Logic for B triggered");
                // Add your action for "B" and "B2" here
                break;

            case "L":
                Debug.Log("Logic for L triggered");
                // Add your action for "L" here
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
}
