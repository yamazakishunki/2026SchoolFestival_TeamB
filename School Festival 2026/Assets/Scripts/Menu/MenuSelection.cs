using UnityEngine;
using UnityEngine.EventSystems;

public class MenuSelection : MonoBehaviour
{
    [SerializeField] private GameObject firstSelectedButton;

    private void Start()
    {
        EventSystem.current.SetSelectedGameObject(firstSelectedButton);
    }
}