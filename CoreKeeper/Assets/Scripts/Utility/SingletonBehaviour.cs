using UnityEngine;

public class SingletonBehaviour<T> : MonoBehaviour where T : MonoBehaviour
{
    private static T instance;
    public static T Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<T>();
                DontDestroyOnLoad(instance.gameObject);
            }
            return instance;
        }
    }

    protected void Awake()
    {
        if (instance != null)
        {
            //  다른 게임 오브젝트가 있다면
            if(instance != this)
                Destroy(gameObject);

            //  Awake 호출 전 할당된 인스턴스가 자기 자신이라면
            return;
        }

        //  Instance 참조 전 Awake가 먼저 실행될 경우 들어오는 부분
        instance = GetComponent<T>();
        DontDestroyOnLoad(gameObject);
    }
}
