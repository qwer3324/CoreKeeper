using UnityEngine;
using UnityEngine.Events;

/// <summary>캐릭터에서 발생하는 이벤트 함수 리스트</summary>
public class CharacterEvents
{
    public static UnityAction<GameObject, float> playerDamaged;
    public static UnityAction<GameObject, float> enemyDamaged;
    public static UnityAction<GameObject, float> characterHeal;
    public static UnityAction characterFood;
}
