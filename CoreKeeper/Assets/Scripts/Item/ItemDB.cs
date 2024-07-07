using UnityEngine;

[CreateAssetMenu(fileName = "Item Database", menuName = "Inventory/Item Database")]
public class ItemDB : ScriptableObject
{
    [SerializeField] private ItemData[] datas;
    public ItemData[] Datas {  get { return datas; } }

    //  스크립트의 값이 변경될 때 호출
    //  에디터에서만 호출됨, 런타임 중에는 호출X
    public void OnValidate()
    {
        //  아이템의 id값을 데이터베이스 등록 순서대로 세팅
        for (int i = 0; i < datas.Length; i++)
        {
            datas[i].info.id = i;
        }
    }
}
