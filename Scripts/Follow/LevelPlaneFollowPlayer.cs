using Cinemachine;
using MVCS.Architecture.BaseCharacter;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelPlaneFollowPlayer : FollowTarget
{
    [SerializeField] private BaseCharacter _player;
    private float _target_xAxis;
    public BaseCharacter Player => _player;

    protected override void LoadComponents()
    {
        base.LoadComponents();

        this.LoadPlayerCharacter();
    }

    private void LoadPlayerCharacter()
    {
        if (this.target != null && this._player != null) return;

        this._player = GameObject.FindObjectOfType<BaseCharacter>();
        this.target = this._player.gameObject;
    }

    protected override void Following()
    {
        if (this.target != null)
        {
            this._target_xAxis = this.target.transform.position.x;
            this.transform.position = new Vector3(this._target_xAxis, this.transform.position.y, this.transform.position.z);
        }
    }


}
