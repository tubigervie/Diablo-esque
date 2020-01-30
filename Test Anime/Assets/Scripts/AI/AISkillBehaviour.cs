using RPG.Control;
using RPG.Movement;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;

public class AISkillBehaviour : StateMachineBehaviour
{

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex, AnimatorControllerPlayable controller)
    {
        if (animator.gameObject.GetComponent<AIController>().disableMove)
        {
            animator.gameObject.GetComponent<Mover>().Cancel();
        }
    }

    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.SetBool("abilityOneShot", false);
        if (animator.gameObject.GetComponent<AIController>().disableMove)
        {
            animator.gameObject.GetComponent<AIController>().disableMove = true;
        }
    }
}
