using UnityEngine;

public class PortalActiveMonster : MonoBehaviour
{
    [SerializeField] private MonsterInfo targetMonster;
    [SerializeField] private GameObject activeTrigger;

    private void Update()
    {
        ActiveTrigger();
    }

    void ActiveTrigger()
    {
        if (targetMonster._hp <= 0)
        { 
           activeTrigger.SetActive(true);
        }
    }
}
