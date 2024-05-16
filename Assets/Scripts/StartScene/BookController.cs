using Cinemachine;
using echo17.EndlessBook;
using TMPro;
using UnityEngine;

public class BookController : MonoBehaviour
{

    public EndlessBook book;

    [SerializeField] TextMeshProUGUI gameStart;

    [SerializeField] CinemachineVirtualCamera startView;
    [SerializeField] CinemachineVirtualCamera bookView;

    public AudioSource audioSource;
    public AudioClip openBook;
    public AudioClip pagingBook;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    private void OnMouseDown()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, Mathf.Infinity, LayerMask.GetMask("Book")))
        {
            if (book.CurrentState == EndlessBook.StateEnum.ClosedFront)
            {
                gameStart.gameObject.SetActive(false);
                book.SetState(EndlessBook.StateEnum.OpenMiddle);
                BookSound(openBook);
                startView.Priority = 0;
                bookView.Priority = 1;
            }
        }

        if (!book.IsLastPageGroup && book.CurrentState == EndlessBook.StateEnum.OpenMiddle)
        {
            book.TurnToPage(book.CurrentLeftPageNumber + 2, EndlessBook.PageTurnTimeTypeEnum.TimePerPage, 1f);
            BookSound(pagingBook);
        }

        if (book.CurrentLeftPageNumber == 7)
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
