using TMPro;
using UnityEngine;

public class DescriptionUI : MonoBehaviour
{
    private Inventory inventory;

    public TextMeshProUGUI nameText;
    public TextMeshProUGUI abilityText;
    public TextMeshProUGUI materialText;

    private void Awake()
    {
        inventory = Inventory.Instance;
    }

    public void UpdateDescriptionUI(CraftData _data)
    {
        nameText.text = inventory.itemDB.Datas[_data.itemID].name;

        abilityText.text = "";

        for (int i = 0; i < inventory.itemDB.Datas[_data.itemID].info.abilities.Length; ++i)
        {
            abilityText.text += inventory.itemDB.Datas[_data.itemID].info.abilities[i].type;
            abilityText.text += " " + inventory.itemDB.Datas[_data.itemID].info.abilities[i].Min + " ~ " + inventory.itemDB.Datas[_data.itemID].info.abilities[i].Max + "\n";
        }

        materialText.text = "Àç·á\n";

        for (int i = 0; i < _data.materialsID.Length; ++i)
        {
            materialText.text += inventory.itemDB.Datas[_data.materialsID[i]].info.name + " " + _data.materialsAmount[i] + "\n";
        }
        
    }
}
