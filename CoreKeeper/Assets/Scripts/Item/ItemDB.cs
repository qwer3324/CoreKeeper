using UnityEngine;

[CreateAssetMenu(fileName = "Item Database", menuName = "Inventory/Item Database")]
public class ItemDB : ScriptableObject
{
    [SerializeField] private ItemData[] datas;
    public ItemData[] Datas {  get { return datas; } }

    //  ��ũ��Ʈ�� ���� ����� �� ȣ��
    //  �����Ϳ����� ȣ���, ��Ÿ�� �߿��� ȣ��X
    public void OnValidate()
    {
        //  �������� id���� �����ͺ��̽� ��� ������� ����
        for (int i = 0; i < datas.Length; i++)
        {
            datas[i].info.id = i;
        }
    }
}
