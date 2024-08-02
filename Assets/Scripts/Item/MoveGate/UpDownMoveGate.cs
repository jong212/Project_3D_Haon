using Cinemachine;
using System.Collections;
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
    [SerializeField] private playerAnimator playerAnimatorScript;  // playerAnimator ��ũ��Ʈ ������ ���� ����

    [SerializeField] private bool isMoving = false;
    private int point = 0;

    public CinemachineVirtualCamera vCam1; // �⺻ ��ġ ī�޶�
    public CinemachineVirtualCamera vCam2; // �̵��� ��ġ ī�޶�

    private bool isCoroutineRunning = false;

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
            //MoveCamera();
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
                playerAnimatorScript.BossStart = true;  // playerAnimator ��ũ��Ʈ�� BossStart �Ӽ��� true�� ����
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
        gameObject.SetActive(false);
    }

    void UpActive()
    {
        isMoving = false;
        point++;
        bgmManager.BgmSet = true;
        if (bgmManager == null) return;

        if (isMoving == false)
        { gameObject.SetActive(false); }
    }


    public void MoveCamera()
    {
        StartCoroutine(MoveCameraRoutine());
    }

    private IEnumerator MoveCameraRoutine()
    {
        isCoroutineRunning = true;
        // ī�޶� �̵��� ��ġ�� ��ȯ
        vCam1.Priority = 0;
        vCam2.Priority = 10;

        // ī�޶� �̵��� �ð��� ��ٸ� (��: 2��)
        yield return new WaitForSeconds(2f);

        // �ٽ� ���� ��ġ�� ��ȯ
        vCam1.Priority = 10;
        vCam2.Priority = 0;
        isCoroutineRunning = false;
    }


}
