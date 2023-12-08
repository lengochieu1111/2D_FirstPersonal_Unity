using MVCS.Architecture.BaseCharacter;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpStartEvent_Anim : StateMachineBehaviour
{
    private BaseCharacterView _characterView;

    /*override public void OnStateMachineEnter(Animator animator, int stateMachinePathHash)
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    override public void OnStateMachineExit(Animator animator, int stateMachinePathHash)
*/
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        this._characterView = animator.gameObject.GetComponent<BaseCharacterView>();
        this._characterView?.EA_JumpStart();
    }

}
