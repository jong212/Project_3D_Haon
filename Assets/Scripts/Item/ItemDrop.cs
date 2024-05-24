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

    public void ItemSpawn(Transform transform)
    {
        randomCoins = Random.Range(5, 10);
        randomHeals = Random.Range(0, 3);
        // ·£´ý µ¿Àü »ý¼º
        SpawnItems(coins, randomCoins, transform);

        // ·£´ý Èú¸µ ¾ÆÀÌÅÛ »ý¼º
        SpawnItems(heals, randomHeals, transform);
    }
    private void SpawnItems(GameObject[] items, int itemCount, Transform spawnTransform)
    {
        for (int i = 0;i < itemCount;i++)
        {
            if (items[i].activeSelf == false)
            {
                items[i].transform.position = spawnTransform.position;
                items[i].SetActive(true);
            }
            else
            {
                i++;
                continue;
            }

            randomposition = (Vector3)Random.insideUnitSphere.normalized * 3 + spawnTransform.position;
            randomposition.y = 1f;
            items[i].transform.DOJump(randomposition, 2f, 1, 1f);
            
        }

       

    }
}
