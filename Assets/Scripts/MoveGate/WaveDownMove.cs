using UnityEngine;

public class WaveDownMove : MonoBehaviour
{
    public GameObject upGate;
    public float upSpeed = 1;
    public float stopTime = 3.5f;
    
    private bool upMove = false;

    private void Update()
    {
        UpGate();
    }
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            upMove = true;
        }
    }

    void UpGate()
    {
        if(upMove)
        {
            upGate.transform.Translate(Vector3.up * upSpeed * Time.deltaTime);
            Invoke("StopUpGate", stopTime);
        }
    }

    void StopUpGate()
    {
        upMove = false;
    }
}