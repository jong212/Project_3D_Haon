using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossFieldSetting : MonoBehaviour
{

    [SerializeField] private GameObject player; 

    // Start is called before the first frame update
    void Start()
    {
        player.GetComponent<playerAnimator>()._hp = 9000;
        player.GetComponent<playerAnimator>()._str = 200;
    }

}
