using RPG.Combat;
using RPG.Control;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;

public class SkillBehaviour : StateMachineBehaviour
{

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex, AnimatorControllerPlayable controller)
    {
        if(animator.gameObject.GetComponent<SpecialAbilities>().GetDisableMove())
        {
            animator.gameObject.GetComponent<PlayerController>().ToggleMovement();
        }
    }
    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (animator.gameObject.GetComponent<SpecialAbilities>().GetDisableMove())
        {
            animator.gameObject.GetComponent<SpecialAbilities>().SetDisableMove(false);
            animator.gameObject.GetComponent<PlayerController>().ToggleMovement();
        }
    }
}
