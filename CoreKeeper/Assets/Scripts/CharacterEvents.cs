using UnityEngine;
using UnityEngine.Events;

/// <summary>ĳ���Ϳ��� �߻��ϴ� �̺�Ʈ �Լ� ����Ʈ</summary>
public class CharacterEvents
{
    public static UnityAction<GameObject, float> playerDamaged;
    public static UnityAction<GameObject, float> enemyDamaged;
    public static UnityAction<GameObject, float> characterHeal;
    public static UnityAction characterFood;
}
