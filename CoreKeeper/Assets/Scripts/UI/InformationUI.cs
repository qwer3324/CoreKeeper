using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class InformationUI : MonoBehaviour
{
    private Inventory inventory;

    public TextMeshProUGUI nameText;
    public TextMeshProUGUI typeText;
    public TextMeshProUGUI abilityText;
    public TextMeshProUGUI descriptionText;

    private void Awake()
    {
        inventory = Inventory.Instance;
    }

    public void UpdateItemInfo(Item _item)
    {
        if (_item == null)
        {
            nameText.text = "이름";
            typeText.text = "종류";
            abilityText.text = "능력";
            descriptionText.text = "설명";
        }
        else
        {
            nameText.text = _item.name;
            typeText.text = inventory.itemDB.Datas[_item.id].type.ToString();

            if (inventory.itemDB.Datas[_item.id].type <= ItemType.Weapon)
            {
                abilityText.enabled = true;
                abilityText.text = "";

                for (int i = 0; i < _item.abilities.Length; ++i)
                {
                    abilityText.text += _item.abilities[i].type;
                    abilityText.text += " ";
                    abilityText.text += _item.abilities[i].value;
                    abilityText.text += "\n";
                }
            }
            if (inventory.itemDB.Datas[_item.id].type <= ItemType.Potion)
            {
                abilityText.enabled = true;
                abilityText.text = "";

                for (int i = 0; i < _item.abilities.Length; ++i)
                {
                    abilityText.text += _item.abilities[i].type;
                    abilityText.text += " ";
                    abilityText.text += _item.abilities[i].value;
                    abilityText.text += "\n";
                }
            }
            else
            {
                abilityText.text = "";
                abilityText.enabled = false;
            }

            descriptionText.text = inventory.itemDB.Datas[_item.id].description;
        }
    }
}
