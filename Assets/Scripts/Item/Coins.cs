using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using DG.Tweening;

public class Coins : MonoBehaviour
{
    public float rotateSpeed = 100.0f;
   
    public BoxCollider coincollider;
    public GameObject coinEffect;
    public GameObject magnet;
    
    private void Awake()
    {
        this.coincollider = GetComponent<BoxCollider>();
        
    }
    private void OnEnable()
    {
        coinEffect.SetActive(false);
        magnet.SetActive(false);
        Invoke("MagnetOn", 2f);
    }

    void Update()
    {
        transform.Rotate(Vector3.up*rotateSpeed*Time.deltaTime);

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            Setting_1.PlusCoins();
            GetCoin();
            coincollider.enabled = false;
            magnet.SetActive(false);
            coinEffect.SetActive(true);
           
        }
    }
    void GetCoin()
    {
        transform.DOMoveY(transform.position.y + 2,2.0f);
        rotateSpeed = 500.0f;
        Invoke("EraseCoin", 2.0f);
    }
    void EraseCoin()
    {
       
        coincollider.enabled =true; 
        gameObject.SetActive(false);
       
    }
    
    void MagnetOn()
    {
        magnet.SetActive(true);
     
    }
}
