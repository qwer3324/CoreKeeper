using UnityEngine;

public class Interactive : MonoBehaviour
{
    private SpriteRenderer sr;

    private Material defaultMtrl;
    public Material outlineMtrl;

    public UIManager.UI_Mode mode;

    public void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
        defaultMtrl = sr.material;
    }

    public void Selected()
    {
        sr.material = outlineMtrl;
    }

    public void Interaction()
    {
        UIManager.Instance.ChangeUiMode(mode);
    }

    public void Deselected()
    {
        sr.material = defaultMtrl;

        UIManager.Instance.ChangeUiMode(UIManager.UI_Mode.Normal);
    }
}
