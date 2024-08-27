using UnityEngine;
using System.Collections;

[RequireComponent(typeof(SpriteRenderer))]
public class FlashBehavior : MonoBehaviour
{
    public float flashDelay = 0.2f;
    private SpriteRenderer spriteRenderer;

    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void Flash()
    {
        StartCoroutine(FlashEffect());
    }

    private IEnumerator FlashEffect()
    {
        while (true)
        {
            spriteRenderer.enabled = !spriteRenderer.enabled;
            yield return new WaitForSeconds(flashDelay);
        }
    }
}