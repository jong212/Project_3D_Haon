using UnityEngine;


public class UpDownMoveGate : MonoBehaviour
{
    public GameObject gate;
    public float speed = 1;
    public bool down = false;
    public float upDownStopSecond = 0;


    [SerializeField] private bool isMoving = false;
    private int point = 0;


    private void Update()
    {
        MoveGate();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
           isMoving = true;
        }
    }

    void MoveGate()
    {
        if (isMoving && point == 0 && down == true)
        {
            gate.transform.Translate(Vector3.down * speed * Time.deltaTime);
            Invoke("DownActive", upDownStopSecond);
        }
        else if (isMoving && point == 0 && !down)
        {
            gate.transform.Translate(Vector3.up * speed * Time.deltaTime);
            Invoke("UpActive", upDownStopSecond);
        }
    }

    void DownActive()
    {
        isMoving = false;
        point++;
        gate.gameObject.SetActive(false);
    }

    void UpActive()
    {
        isMoving = false;
        point++;
        if (isMoving == false) { gameObject.SetActive(false); }
    }
}
