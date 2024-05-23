using UnityEngine;


public class WindMoveMent : MonoBehaviour
{
    [SerializeField] private ParticleSystem windEffect;
    [SerializeField] private float stopWindTime = 1.2f;

    [SerializeField] GameObject player;
    [SerializeField] float moveSpeed = 5.0f;
   
    private void Update()
    {
        WindStopPlayer();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            if(other.GetComponent<NetworkPlayerController>() != null)
            {
                other.GetComponent<NetworkPlayerController>().enabled = false;
            }

            player.transform.Translate(Vector3.zero);
            windEffect.gameObject.SetActive(true);
            Invoke("StopWind" , stopWindTime);

            other.GetComponent<NetworkPlayerController>().enabled = true;
        }
    }

    void StopWind()
    { 
        windEffect.gameObject.SetActive(false);
    }

    void WindStopPlayer()
    {
        if (windEffect.gameObject.activeSelf == true)
        {
            player.transform.Translate(Vector3.back * moveSpeed * Time.deltaTime);
        }
    }
}