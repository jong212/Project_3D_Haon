using UnityEngine;

public class BossWallSwich : MonoBehaviour
{
    public GameObject bossWall;

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("Player"))
        {
            bossWall.SetActive(true);
            Invoke("TriggerDelete", 2);
        }
    }

    void TriggerDelete()
    {
        gameObject.SetActive(false);
    }
}
