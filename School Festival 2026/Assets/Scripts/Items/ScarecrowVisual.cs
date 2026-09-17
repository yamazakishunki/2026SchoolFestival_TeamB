using UnityEngine;

public class ScarecrowVisual : MonoBehaviour
{
    [SerializeField] private GameObject[] scarecrowSprites; // size 4, one per plot, matching AreaId order

    private void OnEnable()
    {
        ItemEffectManager.OnAreaBlocked += ShowAt;
        ItemEffectManager.OnAreaUnblocked += HideAll;
    }

    private void OnDisable()
    {
        ItemEffectManager.OnAreaBlocked -= ShowAt;
        ItemEffectManager.OnAreaUnblocked -= HideAll;
    }

    private void Awake()
    {
        HideAll();
    }

    private void ShowAt(int areaId)
    {
        HideAll(); // just in case, though only one is ever blocked at a time

        if (areaId >= 0 && areaId < scarecrowSprites.Length)
        {
            scarecrowSprites[areaId].SetActive(true);
        }
    }

    private void HideAll()
    {
        foreach (var sprite in scarecrowSprites)
        {
            sprite.SetActive(false);
        }
    }
}
