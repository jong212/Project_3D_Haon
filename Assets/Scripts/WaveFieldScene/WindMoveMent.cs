using UnityEngine;
using UnityEngine.InputSystem;

public class WindMoveMent : MonoBehaviour
{
    [SerializeField] private ParticleSystem windEffect;
    [SerializeField] private float stopWindTime = 1.0f;

    [SerializeField] GameObject player;
    [SerializeField] float moveSpeed = 1.0f;
   
    private void Update()
    {
        WindStopPlayer();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            PlayerInput playerInput = other.GetComponent<PlayerInput>();
            playerInput.enabled = false;
            player.gameObject.layer = 7;
            player.transform.Translate(Vector3.zero);
            
            windEffect.gameObject.SetActive(true);
            Invoke("StopWind" , stopWindTime);
            playerInput.enabled = true;
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