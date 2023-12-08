using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class FollowTarget : RyoMonoBehaviour
{
    [SerializeField] protected GameObject target;
    [SerializeField] protected bool isFollowing = true;
    public GameObject Target => target;

    protected virtual void FixedUpdate()
    {
        Following();
    }

    protected virtual void Following()
    {
        if (this.target && this.isFollowing)
            this.transform.position = this.target.transform.position;
    }

}
