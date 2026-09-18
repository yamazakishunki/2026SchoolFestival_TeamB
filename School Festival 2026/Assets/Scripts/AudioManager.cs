using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }

    [Header("Music")]
    [SerializeField] private AudioSource musicSourceA;
    [SerializeField] private AudioSource musicSourceB;
    [SerializeField] private float crossfadeDuration = 1f;

    [Header("SFX")]
    [SerializeField] private AudioSource sfxSource;

    private AudioSource activeMusicSource;
    private AudioSource inactiveMusicSource;
    private Coroutine fadeRoutine;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);

        activeMusicSource = musicSourceA;
        inactiveMusicSource = musicSourceB;
    }

    // ---- Music ----

    public void PlayMusic(AudioClip clip, bool loop = true, float targetVolume = 1f)
    {
        if (activeMusicSource.clip == clip && activeMusicSource.isPlaying) return;

        if (fadeRoutine != null) StopCoroutine(fadeRoutine);
        fadeRoutine = StartCoroutine(CrossfadeTo(clip, loop, targetVolume)); 
    }

    private IEnumerator CrossfadeTo(AudioClip clip, bool loop, float targetVolume)
    {
        inactiveMusicSource.clip = clip;
        inactiveMusicSource.loop = loop;
        inactiveMusicSource.volume = 0f;
        inactiveMusicSource.Play();

        float elapsed = 0f;
        float startVolA = activeMusicSource.volume;

        while (elapsed < crossfadeDuration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / crossfadeDuration;
            activeMusicSource.volume = Mathf.Lerp(startVolA, 0f, t);
            inactiveMusicSource.volume = Mathf.Lerp(0f, targetVolume, t); 
            yield return null;
        }

        activeMusicSource.Stop();
        activeMusicSource.volume = 1f;

        (activeMusicSource, inactiveMusicSource) = (inactiveMusicSource, activeMusicSource);
    }

    // ---- SFX ----

    public void PlaySFX(AudioClip clip, float volume = 1f)
    {
        sfxSource.PlayOneShot(clip, volume);
    }
}