using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class ItemDrop : MonoBehaviour
{
    public GameObject coinsPrefeb;
    public GameObject healingPrdfab;
    public GameObject[] coins;
    public GameObject[] heals;

    public Transform boxtransform;
    Vector3 randomposition;
    private Vector3 hideposition = new Vector3(0,-100,0);
    private int randomCoins;
    private int randomHeals;
    void Start()
    {
        randomCoins = Random.Range(0, 6);
        randomHeals = Random.Range(0, 6);

        coins = new GameObject[randomCoins];
        heals = new GameObject[randomHeals];
        for (int i = 0; i < randomCoins; i++)
        {
            coins[i] = Instantiate(coinsPrefeb, boxtransform.position, Quaternion.identity);
            coins[i].SetActive(false);
        }
        for(int i = 0;i<randomHeals; i++)
        {
            heals[i] = Instantiate(healingPrdfab, boxtransform.position, Quaternion.identity);
            heals[i].SetActive(false);
        }
    }
    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.L))
        {
            ItemSpawn();
        }
    }


    void ItemSpawn()
    {
       
        for (int i = 0;i < randomCoins;i++)
        {
            coins[i].SetActive(true);

            randomposition = (Vector3)Random.insideUnitSphere.normalized * 2 + boxtransform.position;
            randomposition.y = 1f;
            coins[i].transform.DOJump(randomposition, 2f, 1, 1f);
        }
        for (int i = 0;i<randomHeals;i++)
        {
            heals[i].SetActive(true);
            randomposition = (Vector3)Random.insideUnitSphere.normalized * 2 + boxtransform.position;
            randomposition.y = 1f;
            heals[i].transform.DOJump(randomposition, 2f, 1, 1f);
        }

    }
}
