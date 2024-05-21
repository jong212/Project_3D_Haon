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
        boxtransform = GetComponent<Transform>();
        randomCoins = Random.Range(5, 10);
        randomHeals = Random.Range(0, 3);

        coins = new GameObject[20];
        heals = new GameObject[10];
        for (int i = 0; i < 20; i++)
        {
            coins[i] = Instantiate(coinsPrefeb, boxtransform.position, Quaternion.identity);
            coins[i].SetActive(false);
        }
        for(int i = 0;i<10; i++)
        {
            heals[i] = Instantiate(healingPrdfab, boxtransform.position, Quaternion.identity);
            heals[i].SetActive(false);
        }
    }
    private void Update()
    {
        
    }
     

    public void ItemSpawn(Transform transform)
    {
        randomCoins = Random.Range(5, 10);
        randomHeals = Random.Range(0, 3);

        for (int i = 0;i < randomCoins;i++)
        {
            if (coins[i].activeSelf == false)
            {
                coins[i].transform.position = transform.position;
                coins[i].SetActive(true);
            }
            else
            {
                i++;
                continue;
            }

            randomposition = (Vector3)Random.insideUnitSphere.normalized * 3 + transform.position;
            randomposition.y = 1f;
            coins[i].transform.DOJump(randomposition, 2f, 1, 1f);
            
        }

        for (int i = 0;i<randomHeals;i++)
        {
            if (heals[i].activeSelf == false)
            {
                heals[i].transform.position = transform.position;
                heals[i].SetActive(true);
            }
            else
            {
                i++;
                continue;
            }
            heals[i].SetActive(true);
            randomposition = (Vector3)Random.insideUnitSphere.normalized * 2 + transform.position;
            randomposition.y = 1f;
            heals[i].transform.DOJump(randomposition, 2f, 1, 1f);
        }

    }
}
