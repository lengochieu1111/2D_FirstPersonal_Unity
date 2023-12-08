using MVCS.Architecture.BaseCharacter;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RisingEvent_Anim : StateMachineBehaviour
{
    private BaseCharacterView characterView;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        this.characterView = animator.gameObject.GetComponent<BaseCharacterView>();
    }

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        this.characterView?.EA_RisingToFalling();
    }

/*
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
*/
}
