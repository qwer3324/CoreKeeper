using UnityEngine;

public class Interaction : MonoBehaviour
{
    private Player player;
    private Interactive interactive;
    private BoxCollider2D col;

    private Character.Direction dir;
    public Character.Direction Dir 
    { 
        set 
        { 
            dir = value;

            switch (dir)
            {
                case Character.Direction.Front:
                    col.offset = new Vector2(0, -0.15f);
                    col.size = new Vector2(0.2f, 1);
                    transform.localScale = new Vector3(1, 1, 1);
                    break;
                case Character.Direction.Back:
                    col.offset = new Vector2(0, 0.85f);
                    col.size = new Vector2(0.2f, 1);
                    transform.localScale = new Vector3(1, 1, 1);
                    break;
                case Character.Direction.Left:
                    col.offset = new Vector2(-0.85f, 0.5f);
                    col.size = new Vector2(1, 0.2f);
                    transform.localScale = new Vector3(-1, 1, 1);
                    break;
                case Character.Direction.Right:
                    col.offset = new Vector2(0.85f, 0.5f);
                    col.size = new Vector2(1, 0.2f);
                    transform.localScale = new Vector3(1, 1, 1);
                    break;
            }
        }
    }

    private void Awake()
    {
        player = GetComponentInParent<Player>();
        col = GetComponent<BoxCollider2D>();
    }

    private void Update()
    {
        if (interactive != null)
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                interactive.Interaction();
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D _collision)
    {
        if(interactive != null)
        {
            interactive.Deselected();
            interactive = null;
        }

        interactive = _collision.GetComponent<Interactive>();

        if(interactive != null)
        {
            interactive.Selected();
        }
    }

    private void OnTriggerExit2D(Collider2D _collision)
    {
        if (interactive == null)
            return;

        if(interactive.gameObject == _collision.gameObject)
        {
            interactive.Deselected();
            interactive = null;
        }
    }
}
