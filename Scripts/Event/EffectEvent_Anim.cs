using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectEvent_Anim : StateMachineBehaviour
{
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        EffectSpawner.Instance.DestroyObject(animator.gameObject);
    }

/*    
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
*/

}
