using System.Collections;
using UnityEngine;

public class BoxInfo : MonoBehaviour
{
    public GameObject itemDrop;
    public Transform box;
    [SerializeField] private ParticleSystem effect;
    
    [SerializeField] private float boxHp = 5;
    [SerializeField] private int itemProbability = 2;
    public GameObject[] drapItems;

    private ParticleSystem particle;
    private int itemDrap;
    private int itemNum;
   
    public void BoxDamaged(int damage)
    {
        boxHp -= damage;

        if (boxHp <= 0)
        {
            StartCoroutine(BoxEffectDestory());
            DrapItem();
        }
        else
        {
            StartCoroutine(TakeDamage());
            Debug.Log($"BoxHp : {boxHp}");
        }
    }

    IEnumerator TakeDamage()
    {
        SpriteRenderer spr = gameObject.GetComponent<SpriteRenderer>();
        transform.rotation = new Quaternion(2, 1, 2, 0);
        spr.transform.localScale = Vector3.one * 1.2f;
        yield return new WaitForSeconds(0.2f);
        transform.rotation = new Quaternion(1, 1, 2, 0);
        yield return new WaitForSeconds(0.2f);
        spr.transform.localScale = Vector3.one;
        transform.rotation = Quaternion.identity;
    }

    IEnumerator BoxEffectDestory()
    {
        particle = Instantiate(effect, transform.position, Quaternion.identity);
        gameObject.SetActive(false);
        yield return new WaitForSeconds(0.5f);
        Destroy(particle.gameObject, 1.0f);
    }

    void DrapItem()
    {
        //itemDrap = Random.Range(0, itemProbability);
        //if(itemDrap == 0)
        //{
        //  itemNum = drapItems.Length;
        //  itemDrap = Random.Range(0, itemNum);
        //  Instantiate(drapItems[itemDrap], transform.position, Quaternion.identity);
        //}
        itemDrop.GetComponent<ItemDrop>().ItemSpawn(box);


    }
}
