using UnityEngine;

public class MoveAndWrap : MonoBehaviour
{
    // Speed of the character movement
    public float speed = 5f;

    // Original position of the character (off-screen on the left)
    private Vector3 originalPosition;

    // Width of the screen in world units
    private float screenWidthInWorldUnits;

    // Width of the character sprite
    private float characterWidth;

    void Start()
    {
        // Calculate the screen width in world units
        screenWidthInWorldUnits = Camera.main.orthographicSize * 2f * Camera.main.aspect;

        // Get the width of the character sprite
        characterWidth = GetComponent<SpriteRenderer>().bounds.size.x;

        // Store the original position of the character (off-screen on the left)
        originalPosition = new Vector3(Camera.main.transform.position.x - screenWidthInWorldUnits / 2 - characterWidth, transform.position.y, transform.position.z);

        // Set the character's initial position to the original position
        transform.position = originalPosition;
    }

    void Update()
    {
        // Move the character to the right
        transform.Translate(Vector3.right * speed * Time.deltaTime);

        // Check if the character has moved off the right side of the screen
        if (transform.position.x > Camera.main.transform.position.x + screenWidthInWorldUnits / 2 + characterWidth / 2)
        {
            // Reset the character to its original position (off-screen on the left)
            transform.position = originalPosition;
        }
    }
}
