using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
public class TutorialKeyController : MonoBehaviour
{
    public Image AKey;
    public Image SKey;
    public Image DKey;
    public Image WKey;
    public Transform Camera;

    void Start()
    {
        
    }

    
    void Update()
    {
        //transform.LookAt(Camera);
        if (Input.GetKeyDown(KeyCode.A)) AKey.color = Color.red;
        if (Input.GetKeyUp(KeyCode.A)) AKey.color = Color.white;

        if (Input.GetKeyDown(KeyCode.S)) SKey.color = Color.red;
        if (Input.GetKeyUp(KeyCode.S)) SKey.color = Color.white;

        if (Input.GetKeyDown(KeyCode.D)) DKey.color = Color.red;
        if (Input.GetKeyUp(KeyCode.D)) DKey.color = Color.white;

        if (Input.GetKeyDown(KeyCode.W)) WKey.color = Color.red;
        if (Input.GetKeyUp(KeyCode.W)) WKey.color = Color.white;

    }

    


}
