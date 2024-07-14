using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Ghost))]
public abstract class GhostBehavior : MonoBehaviour
{
    public Ghost ghost { get; private set; }
    public float duration;

    private Coroutine disableCoroutine;

    private void Awake()
    {
        ghost = GetComponent<Ghost>();
        this.enabled = false;
    }

    public void Enable()
    {
        Enable(duration);
    }

    public virtual void Enable(float duration)
    {
        enabled = true;

        if (disableCoroutine != null)
        {
            StopCoroutine(disableCoroutine);
        }
        disableCoroutine = StartCoroutine(DisableAfterDuration(duration));
    }

    private IEnumerator DisableAfterDuration(float duration)
    {
        yield return new WaitForSeconds(duration);
        Disable();
    }

    public virtual void Disable()
    {
        enabled = false;

        if (disableCoroutine != null)
        {
            StopCoroutine(disableCoroutine);
            disableCoroutine = null;
        }
    }
}