using UnityEngine;

/// <summary>
/// Component 추가 기능
/// </summary>
public class ComponentHelper
{
    //부모 오브젝트들 중에서 원하는 컴포넌트(T) 찾기
    public static T FindInParents<T>(GameObject go) where T : Component
    {
        if (go == null) return null;

        var comp = go.GetComponent<T>();
        if (comp != null)
            return comp;

        var t = go.transform.parent;
        while (t != null && comp == null)
        {
            comp = t.gameObject.GetComponent<T>();
            t = t.parent;
        }

        return comp;
    }
}
