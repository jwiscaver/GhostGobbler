using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class AnimatedImage : MonoBehaviour
{
    [Tooltip("Sprite array for handling animations.")]
    [SerializeField] public Sprite[] sprites = new Sprite[0];

    [Tooltip("Time for a complete cycle of the animation")]
    [SerializeField] public float animationTime = 0.125f;

    [Tooltip("Does the animation loop?")]
    [SerializeField] public bool loop = true;

    private Image imageComponent;
    private int animationFrame;

    private void Awake()
    {
        imageComponent = GetComponent<Image>();
    }

    private void OnEnable()
    {
        imageComponent.enabled = true;
    }

    private void OnDisable()
    {
        imageComponent.enabled = false;
    }

    private void Start()
    {
        InvokeRepeating(nameof(Advance), animationTime, animationTime);
    }

    private void Advance()
    {
        if (!imageComponent.enabled)
        {
            return;
        }

        animationFrame++;

        if (animationFrame >= sprites.Length && loop)
        {
            animationFrame = 0;
        }

        if (animationFrame >= 0 && animationFrame < sprites.Length)
        {
            imageComponent.sprite = sprites[animationFrame];
        }
    }

    public void Restart()
    {
        animationFrame = -1;
        Advance();
    }
}
