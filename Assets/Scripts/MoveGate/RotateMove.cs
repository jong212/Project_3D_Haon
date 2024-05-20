using UnityEngine;

public class RotateMove : MonoBehaviour
{
    public GameObject rotationobj;
    public float speed = 30;
    public float inspeed = 1;

    public GameObject downGate;
    public float downSpeed = 1;
    public float upDownStopTime = 0;

    private bool active = false;
    private bool moveChange = false;
    private bool open = false;

    private void Update()
    {
        objMove();
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("Player"))
        {
            active = true;
        }
    }

    void objMove()
    {
        if (active)
        {
            float rotationAng = speed * Time.deltaTime;
            rotationobj.transform.Rotate(Vector3.forward, rotationAng);
            Invoke("moveChanged", 1.5f);

            if (moveChange == true)
            {
                Invoke("InObj", 1.5f);
            }
        }
    }

    void moveChanged()
    {
        moveChange = true;
        speed = 0;
    }

    void InObj()
    {
        rotationobj.transform.Translate(Vector3.forward * inspeed * Time.deltaTime);
        Invoke("stopObj", 0.35f);
    }

    void stopObj()
    {
        inspeed = 0;
        open = true;

        if (open == true)
        {
            Invoke("DownGate",0.5f);
        }
    }

    void DownGate()
    {
        downGate.transform.Translate(Vector3.down * downSpeed * Time.deltaTime);
        Invoke("DownActive", upDownStopTime);
    }

    void DownActive()
    {
        downGate.gameObject.SetActive(false);
        gameObject.SetActive(false);
    }
}

