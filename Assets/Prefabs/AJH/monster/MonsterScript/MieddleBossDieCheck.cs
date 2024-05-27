using UnityEngine;

public class MieddleBossDieCheck : MonoBehaviour
{
    public MonsterInfo mieddleMonser;
    public bool dieCheck = false;
    private void Update()
    {
        MieddleBossDie();
    }
    
    void MieddleBossDie()
    { 
        if(mieddleMonser._hp <=0)
        {
            dieCheck = true;
        }
    }
}
