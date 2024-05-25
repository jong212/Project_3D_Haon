using UnityEngine;
using UnityEngine.InputSystem;

public class TrapDamaged : MonoBehaviour
{
    public playerAnimator player;
    public NetworkPlayerController playerController;
    public int damageIntPoint = 1;
    public float sladeSpeed = 1;

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (hit.gameObject.CompareTag("Trap"))
        {
            if (!playerController && player)
            {
                player.TakeDamage(damageIntPoint);
                Debug.Log("Trap Damaged(single)");
            }
            else if (!player && playerController)
            {
                playerController.TakeDamage(damageIntPoint);
                Debug.Log("Trap Damaged(Network)");
            }
            else { Debug.Log("playerControler 부착되지 않음"); }
           
        }
        else if(hit.gameObject.name == "SladeRock")
        {
            hit.gameObject.transform.Translate(gameObject.transform.forward * Time.deltaTime);
        }
    }
}