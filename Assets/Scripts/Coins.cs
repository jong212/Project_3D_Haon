using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using DG.Tweening;

public class Coins : MonoBehaviour
{
    public float rotateSpeed = 100.0f;
    public float upSpeed = 10.0f;
    Vector3 upposition;
    public BoxCollider coincollider;
    private void Awake()
    {
        this.coincollider = GetComponent<BoxCollider>();
        upposition.y=transform.position.y;
    }

    void Update()
    {
        transform.Rotate(Vector3.up*rotateSpeed*Time.deltaTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            PlayInfo.CoinPlus();
            GetCoin();
            coincollider.enabled = false;
        }
    }
    void GetCoin()
    {
        Debug.Log("?");
        transform.DOMoveY(upposition.y+2,2.0f);
        rotateSpeed = 500.0f;
        Invoke("EraseCoin", 2.0f);
    }
    void EraseCoin()
    {
        gameObject.SetActive(false);
    }

}
