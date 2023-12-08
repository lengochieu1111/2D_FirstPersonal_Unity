using MVCS.Architecture.BaseCharacter;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallingEvent_Anim : StateMachineBehaviour
{
    private BaseCharacterView _characterView;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        this._characterView = animator.gameObject.GetComponent<BaseCharacterView>();
    }

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        this._characterView?.EA_FallingToLanding();
    }

    /*  
        override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    */

}
