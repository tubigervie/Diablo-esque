using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;
using RPG.Combat;
using RPG.Control;


public class LoopBehaviour : StateMachineBehaviour
{
    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex, AnimatorControllerPlayable controller)
    {
        if (animator.gameObject.GetComponent<SpecialAbilities>().GetDisableMove())
        {
            animator.gameObject.GetComponent<PlayerController>().DisableMovement();
        }
        else
            animator.gameObject.GetComponent<PlayerController>().AllowMovement();
    }


    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex, AnimatorControllerPlayable controller)
    {
        if (animator.gameObject.GetComponent<SpecialAbilities>().GetDisableMove())
        {
            animator.gameObject.GetComponent<PlayerController>().DisableMovement();
        }
        else
            animator.gameObject.GetComponent<PlayerController>().AllowMovement();
    }
    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (animator.gameObject.GetComponent<SpecialAbilities>().GetDisableMove())
        {
            animator.gameObject.GetComponent<PlayerController>().AllowMovement();
        }
    }
}