using Cinemachine;
using echo17.EndlessBook;
using System.Collections;
using UnityEngine;

public class BookController : MonoBehaviour
{

    public EndlessBook book;

    [SerializeField] CinemachineVirtualCamera startView;
    [SerializeField] CinemachineVirtualCamera bookView;
    

    void Update()
    {
        
        
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (book.CurrentState == EndlessBook.StateEnum.ClosedFront)
            {
                book.SetState(EndlessBook.StateEnum.OpenMiddle);
                startView.Priority = 0;
                bookView.Priority = 1;
            }
            // book.CurrentLeftPageNumber 에 따라 페이지 넘기면서 UI설정가능
            Debug.Log(book.CurrentLeftPageNumber);
            if (!book.IsLastPageGroup)
            {
                book.TurnToPage(book.CurrentLeftPageNumber + 2, EndlessBook.PageTurnTimeTypeEnum.TimePerPage, 1f);
            }
        }
    }


}
