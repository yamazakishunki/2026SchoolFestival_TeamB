using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneMusicTrigger : MonoBehaviour
{
    [SerializeField] private AudioClip sceneMusic;
    [Range (0.0f,1.0f)][SerializeField] private float volume;

    private void Start()
    {
        AudioManager.Instance.PlayMusic(sceneMusic, true, volume);
    }
}