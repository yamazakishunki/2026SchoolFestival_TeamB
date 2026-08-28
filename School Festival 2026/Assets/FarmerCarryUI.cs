using UnityEngine;

public class FarmerCarryUI : MonoBehaviour
{
    [SerializeField] private FarmerInventory inventory;
    [SerializeField] private SpriteRenderer[] riceIcons; // size = maxCarry, ordered left to right
    [SerializeField] private Sprite filledIcon;
    [SerializeField] private Sprite emptyIcon; // optional: leave null to just hide instead

    private void Start()
    {
        inventory.OnCarriedChanged.AddListener(UpdateIcons);
        UpdateIcons(inventory.CarriedRice); // set correct state immediately on start
    }

    private void OnDestroy()
    {
        inventory.OnCarriedChanged.RemoveListener(UpdateIcons);
    }

    private void UpdateIcons(int carriedCount)
    {
        for (int i = 0; i < riceIcons.Length; i++)
        {
            bool isFilled = i < carriedCount;

            if (emptyIcon != null)
            {
                riceIcons[i].sprite = isFilled ? filledIcon : emptyIcon;
                riceIcons[i].enabled = true;
            }
            else
            {
                riceIcons[i].sprite = filledIcon;
                riceIcons[i].enabled = isFilled; // just hide unfilled slots
            }
        }
    }
}