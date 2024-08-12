using UnityEngine;
using UnityEngine.UI;

public class Fader : MonoBehaviour
{
    public Image img;
    public AnimationCurve curve;

    // Start is called before the first frame update
    void Start()
    {
        img.color = new Color(0f, 0f, 0f, 1f);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
