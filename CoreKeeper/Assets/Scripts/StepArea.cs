using UnityEngine;

public class StepArea : MonoBehaviour
{
    [SerializeField] private Player.FootStep footstep;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Player player = collision.GetComponent<Player>();

        if(player != null )
        {
            player.footStep = footstep;
        }
    }
}
