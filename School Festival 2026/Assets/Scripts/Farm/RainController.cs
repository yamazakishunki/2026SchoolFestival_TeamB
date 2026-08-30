using UnityEngine;

public class RainController : MonoBehaviour
{
    [SerializeField] private GameObject rainAnimationObject; // your pre-made rain droplet GameObject

    private void OnEnable()
    {
        GameStateManager.OnRainingStart += ShowRain;
    }

    private void OnDisable()
    {
        GameStateManager.OnRainingStart -= ShowRain;
    }

    private void Awake()
    {
        rainAnimationObject.SetActive(false); // hidden until Raining starts
    }

    private void ShowRain()
    {
        rainAnimationObject.SetActive(true);
    }
}