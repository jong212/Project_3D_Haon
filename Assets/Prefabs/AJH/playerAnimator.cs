using UnityEngine;
using UnityEngine.InputSystem;

public class playerAnimator : MonoBehaviour
{
    private Animator _animator;
    private Vector3 moveDirection;
    private bool isRunning = false; // 뛰기 상태를 추적하는 변수


    float attackTime = 0;

    void Start()
    {
        _animator = this.GetComponent<Animator>();
    }

    void Update()
    {


        bool hasControl = (moveDirection != Vector3.zero);
        if (hasControl)
        {
            transform.rotation = Quaternion.LookRotation(moveDirection);
            transform.Translate(Vector3.forward * Time.deltaTime * 2f); // 뛰기 상태일 때는 더 빠르게 이동
            _animator.SetBool("isRunning", isRunning);
        }
        else
        {
            _animator.SetBool("isRunning", false); // 이동하지 않을 때는 뛰기 상태 해제
        }


    }


    #region SEND_MESSAGE
    void OnMove(InputValue value)
    {
        Vector2 input = value.Get<Vector2>();                 // 입력 받은 값을 가져오기
        if (input != null)
        {
            moveDirection = new Vector3(input.x, 0f, input.y);

            // 이동 입력이 있을 때만 뛰기 상태로 변경
            isRunning = input.magnitude > 0;
        }
    }
    public void onWeaponAttack()
    {
        Debug.Log("ddd");
        _animator.SetTrigger("onWeaponAttack");
    }

    void OnClick()
    {

        onWeaponAttack();
    }
    #endregion
}