using UnityEngine;

public class TrapDamaged : MonoBehaviour
{
    public playerAnimator player;
    public int damageIntPoint = 1;

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (hit.gameObject.CompareTag("Trap"))
        {
            player.TakeDamage(damageIntPoint);
            Debug.Log("Trap Damaged");
        }
    }
}