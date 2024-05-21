using UnityEngine;

public class TrapTrigger : MonoBehaviour
{
    public GameObject trap;
    public float speed;
    public float stopTime = 0;
    public bool down = false;

    private bool isMove = false;
    private int point = 0;

    private void Update()
    {
        MoveTrap();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            isMove = true;
        }
    }

    void MoveTrap()
    {
        if (isMove && point == 0 && down == false)
        {
            trap.transform.Translate(Vector3.up * speed * Time.deltaTime);
            Invoke("UpDownTrap", stopTime);
        }
        else if(isMove && point == 0 && down == true)
        {
            trap.transform.Translate(Vector3.down * speed * Time.deltaTime);
            Invoke("UpDownTrap", stopTime);
        }
    }

    void UpDownTrap()
    {
        isMove = false;
        point++;
        gameObject.SetActive(false);
    }
}
