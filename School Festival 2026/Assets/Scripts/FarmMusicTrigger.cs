using UnityEngine;

public class FarmMusicTrigger : MonoBehaviour
{
    [SerializeField] private AudioClip normalMusic;
    [Range(0.0f, 1.0f)][SerializeField] private float volume1;
    [SerializeField] private AudioClip feverMusic;
    [Range(0.0f, 1.0f)][SerializeField] private float volume2;
    [SerializeField] private AudioClip rainMusic;
    [Range(0.0f, 1.0f)][SerializeField] private float volume3;

    private void OnEnable()
    {
        GameStateManager.OnFeverStart += PlayFeverMusic;
        GameStateManager.OnRainingStart += PlayRainMusic;
    }

    private void OnDisable()
    {
        GameStateManager.OnFeverStart -= PlayFeverMusic;
        GameStateManager.OnRainingStart -= PlayRainMusic;
    }

    private void Start()
    {
        AudioManager.Instance.PlayMusic(normalMusic,true ,volume1);
    }

    private void PlayFeverMusic() => AudioManager.Instance.PlayMusic(feverMusic,true,volume2);
    private void PlayRainMusic() => AudioManager.Instance.PlayMusic(rainMusic,true,volume3);
}
