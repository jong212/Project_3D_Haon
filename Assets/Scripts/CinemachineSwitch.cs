using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CinemachineSwitch : MonoBehaviour
{

    private Animator anim;
    private bool startViewCamera = true;

    void Start()
    {
        anim = GetComponent<Animator>();
    }

    private void SwitchState()
    {
        if(startViewCamera)
        {
          //  anim.Play();
        }
    }

    void Update()
    {
        
    }


}
