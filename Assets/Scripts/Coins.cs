using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using DG.Tweening;

public class Coins : MonoBehaviour
{
    public float rotateSpeed = 100.0f;
    Vector3 upposition;
    public BoxCollider coincollider;
    public ParticleSystem coinEffect;
    Vector3 randomposition;
    public GameObject magnet;
    public SphereCollider sphereCollider;
    private void Awake()
    {
        this.coincollider = GetComponent<BoxCollider>();
        upposition.y=transform.position.y;


    }
   
    void Update()
    {
        transform.Rotate(Vector3.up*rotateSpeed*Time.deltaTime);

        if(Input.GetKeyDown(KeyCode.N)) 
        {
            randomposition = (Vector3)Random.insideUnitSphere.normalized*2+transform.position;
            randomposition.y = 0.3f;
            transform.DOJump(randomposition, 1f, 1, 2f);
        }
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            PlayInfo.CoinPlus();
            GetCoin();
            coinEffect.Play();
            coincollider.enabled = false;
            sphereCollider.enabled = false;
        }
    }
    void GetCoin()
    {
        transform.DOMoveY(upposition.y+2,2.0f);
        rotateSpeed = 500.0f;
        Invoke("EraseCoin", 2.0f);
    }
    void EraseCoin()
    {
        gameObject.SetActive(false);
        magnet.SetActive(false);
    }
    void SpawnCoins()
    {
        transform.DOJump((Vector2)Random.insideUnitCircle, 2f, 1, 2f);
    }

}
