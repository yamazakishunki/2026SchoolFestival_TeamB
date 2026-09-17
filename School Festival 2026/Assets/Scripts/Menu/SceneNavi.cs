using UnityEngine;
using UnityEngine.SceneManagement;

public class ExplanationMenu : MonoBehaviour
{
    [SerializeField] private string mainMenuSceneName = "MainMenu";

    private void Update() // NEW
    {
        if (Input.GetButtonDown("Submit") || Input.GetKeyDown(KeyCode.Space)) // covers Enter, Space (if bound), and controller A/Cross by default
        {
            OnBackButton();
        }
    }

    public void OnBackButton()
    {
        SceneManager.LoadScene(mainMenuSceneName);
    }

}