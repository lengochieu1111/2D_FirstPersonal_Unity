using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathEvent_Anim : StateMachineBehaviour
{
    private BaseCharacter _character;

    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        this._character = animator.gameObject.GetComponentInParent<BaseCharacter>();
        this._character?.HandleDeadEnd();
    }

    /*    
        override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    */
}
