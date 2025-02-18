using UnityEngine;
using TMPro;

public class InventoryUI : MonoBehaviour
{
    private TextMeshProUGUI treasureText;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        treasureText = GetComponent<TextMeshProUGUI>();
    }

    public void updateTreasureText(PlayerInventory playerInventory)
    {
        treasureText.text = playerInventory.NumberOfTreasures.ToString() + " / 5";
    }
}
