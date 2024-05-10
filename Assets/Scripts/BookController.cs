using Cinemachine;
using echo17.EndlessBook;
using UnityEngine;

public class BookController : MonoBehaviour
{

    public EndlessBook book;

    [SerializeField] CinemachineVirtualCamera startView;
    [SerializeField] CinemachineVirtualCamera bookView;

    public AudioSource audioSource;
    public AudioClip openBook;
    public AudioClip pagingBook;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {


        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (book.CurrentState == EndlessBook.StateEnum.ClosedFront)
            {
                book.SetState(EndlessBook.StateEnum.OpenMiddle);
                BookSound(openBook);
                startView.Priority = 0;
                bookView.Priority = 1;
            }
            // book.CurrentLeftPageNumber 에 따라 페이지 넘기면서 UI설정가능
            Debug.Log(book.CurrentLeftPageNumber);
            if (!book.IsLastPageGroup)
            {
                book.TurnToPage(book.CurrentLeftPageNumber + 2, EndlessBook.PageTurnTimeTypeEnum.TimePerPage, 1f);
                BookSound(pagingBook);
            }
        }

        if (book.CurrentLeftPageNumber == 9)
        {
            
            FadeInFadeOutSceneManager.Instance.ChangeScene("TestScene_WJH");
          
        }
    }


    void BookSound(AudioClip audioClip)
    {
        audioSource.clip = audioClip;
        audioSource.PlayOneShot(audioClip);
    }
    
}
