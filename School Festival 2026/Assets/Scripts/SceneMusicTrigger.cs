using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class SceneMusicTrigger : MonoBehaviour
{
    [SerializeField] private AudioClip sceneMusic;
    [Range(0.0f, 1.0f)][SerializeField] private float volume1;

    [SerializeField] private AudioClip naviFX;
    [Range(0.0f, 1.0f)][SerializeField] private float volume2;

    [SerializeField] private AudioClip selectFX;
    [Range(0.0f, 1.0f)][SerializeField] private float volume3;


    private GameObject lastSelected;

    private void Start()
    {
        AudioManager.Instance.PlayMusic(sceneMusic, true, volume1);
    }

    private void Update()
    {
        GameObject current = EventSystem.current.currentSelectedGameObject;
        if (current != lastSelected)
        {


            if (current != null)
            {
                AudioManager.Instance.PlaySFX(naviFX, volume2);
            }
            lastSelected = current;
        }
        if (Input.GetButtonDown("Submit")&&current != null)
        {
            AudioManager.Instance.PlaySFX(selectFX, volume3);
        }
    }


}