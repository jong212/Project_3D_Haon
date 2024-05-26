using UnityEngine;


public class UpDownMoveGate : MonoBehaviour
{
    public GameObject gate;
    public GameObject Canvas_Boss;

    public float speed = 1;
    public bool down = false;
    public bool bossStart = false;
    public float upDownStopSecond = 0;
    [SerializeField] private BgmManager bgmManager;
    [SerializeField] private Boss boss;
    [SerializeField] private playerAnimator playerAnimatorScript;  // playerAnimator 스크립트 참조를 위한 변수

    [SerializeField] private bool isMoving = false;
    private int point = 0;


    private void Start()
    {
        playerAnimatorScript = playerAnimatorScript.GetComponent<playerAnimator>();
    }
    private void Update()
    {
        MoveGate();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
           isMoving = true;
        }
    }

    void MoveGate()
    {
        if (isMoving && point == 0 && down == true)
        {
            gate.transform.Translate(Vector3.down * speed * Time.deltaTime);
            Invoke("DownActive", upDownStopSecond);
         

        }
        else if (isMoving && point == 0 && !down)
        {
            if (!bossStart)
            {
                boss.bosssRoomStartCheck = true;
                bossStart = true;
                Canvas_Boss.active = true;
                playerAnimatorScript.BossStart = true;  // playerAnimator 스크립트의 BossStart 속성을 true로 설정
            }
            gate.transform.Translate(Vector3.up * speed * Time.deltaTime);
            Invoke("UpActive", upDownStopSecond);
        }
    }

    void DownActive()
    {
        isMoving = false;
        point++;
        
        gate.gameObject.SetActive(false);
        gameObject.SetActive(false );
    }

    void UpActive()
    {
        isMoving = false;
        point++;
        bgmManager.BgmSet = true;
        if (bgmManager == null) return;
        if (isMoving == false) { gameObject.SetActive(false); }
    }
}
