using echo17.EndlessBook;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BookController : MonoBehaviour
{
    
    public EndlessBook book;


    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            book.SetState(EndlessBook.StateEnum.OpenMiddle);
        }
    }
}
