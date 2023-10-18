using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackEvent_Anim : StateMachineBehaviour
{
    private IAttackInterface _attackInterface;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        this._attackInterface = animator.gameObject.GetComponentInParent<IAttackInterface>();
    }

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
         this._attackInterface?.I_AE_TraceHit();
    }


    /*    
        override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    */

}
