using UnityEngine;

public class LazerAttackState1 : StateMachineBehaviour
{
    Transform player;
    playerAnimator playerinfo;
    MonsterInfo monsterinfo;
    [SerializeField] ParticleSystem Lazer1;
    [SerializeField] lazer_top_hp top;

    float interval = 1.0f; // Set the interval to 1 second for particle system playback
    float timer = 0;
    GameObject playerObject;
    GameObject TargetChange;
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        playerObject = GameObject.FindGameObjectWithTag("Lazer_point");
        TargetChange = GameObject.FindGameObjectWithTag("Player");

        if (playerObject != null)
        {
            top = playerObject.GetComponent<lazer_top_hp>();

            // Find the specific child object by name (e.g., "Lazer_Effect1")
            Transform specificChild = playerObject.transform.Find("Lazer_Effect1");

            if (specificChild != null)
            {
                // Get the ParticleSystem component attached to the specific child
                Lazer1 = specificChild.GetComponent<ParticleSystem>();

                // Check if the particle system is found
                if (Lazer1 == null)
                {
                    Debug.LogWarning("No ParticleSystem found on the specific child.");
                }
            }
            else
            {
                Debug.LogWarning("Specific child not found.");
            }
        }
        //playerinfo = player.GetComponent<playerAnimator>();
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (playerObject != null)
        {

            animator.transform.LookAt(player);
            float distance = Vector3.Distance(playerObject.transform.position, animator.transform.position);
            if (distance > 3.5f)
                animator.SetBool("isAttacking", false);

            timer += Time.deltaTime;

            // Play the particle system every interval seconds
            if (timer >= interval)
            {
                if (Lazer1 != null)
                {
                    if (playerObject.activeSelf == false)
                    {
                        animator.SetBool("isAttacking", false);
                    }
                    top.hitDamage(1);
                    Lazer1.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
                    Lazer1.Play();
                }

                timer = 0f; // Reset the timer
            }
        }
        else if (TargetChange != null)
        {
            float distance = Vector3.Distance(TargetChange.transform.position, animator.transform.position);
            if (distance > 3.5f)
                animator.SetBool("isAttacking", false);
        }

    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        // Optionally stop the particle system when exiting the state
        if (playerObject != null)
        {
            if (Lazer1 != null)
            {
                Lazer1.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
            }
        }

    }

    // OnStateMove is called right after Animator.OnAnimatorMove()
    //override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that processes and affects root motion
    //}

    // OnStateIK is called right after Animator.OnAnimatorIK()
    //override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that sets up animation IK (inverse kinematics)
    //}
}
