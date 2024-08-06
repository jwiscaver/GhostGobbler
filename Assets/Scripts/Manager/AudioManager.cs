using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }

    [SerializeField] private AudioSource chompSource;
    [SerializeField] private AudioSource deathSource;
    [SerializeField] private AudioSource fruitCollectSource;
    [SerializeField] private AudioSource ghostKillSource;
    [SerializeField] private AudioSource introSource;
    [SerializeField] private AudioSource normalGhostSource;
    [SerializeField] private AudioSource ghostFrightenedSource;

    private void Awake()
    {
        if (Instance != null)
        {
            DestroyImmediate(gameObject);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    // waka 
    public void PlayChomp()
    {
        if (!IsChompPlaying())
        {
            PlayClip(chompSource, loop: false);
        }
    }

    public void StopChomp()
    {
        chompSource.Stop();
    }

    public bool IsChompPlaying()
    {
        return chompSource.isPlaying;
    }

    // siren
    public void PlayNormalGhostMusic()
    {
        PlayClip(normalGhostSource, true);
    }

    public void StopNormalGhostMusic()
    {
        normalGhostSource.Stop();
    }

    public void PlayDeath()
    {
        PlayClip(deathSource);
    }

    public void PlayFruitCollect()
    {
        PlayClip(fruitCollectSource);
    }

    public void PlayGhostKill()
    {
        PlayClip(ghostKillSource);
    }

    public void PlayIntro()
    {
        PlayClip(introSource);
    }

    public void PlayGhostFrightened()
    {
        PlayClip(ghostFrightenedSource);
    }

    public void StopGhostFrightened()
    {
        ghostFrightenedSource.Stop();
    }

    private void PlayClip(AudioSource source, bool loop = false)
    {
        /*if (source.isPlaying)
        {
            source.Stop();
        }*/
        source.loop = loop;
        source.Play();
    }
}
