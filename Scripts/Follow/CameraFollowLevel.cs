using Cinemachine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CinemachineVirtualCamera))]
public class CameraFollowLevel : FollowTarget
{
    [SerializeField] private CinemachineVirtualCamera _virtualCamera;
    [SerializeField] private float _target_xAxis;

    protected override void Start()
    {
        this.SetTargetForCamera();
    }

    protected override void LoadComponents()
    {
        base.LoadComponents();

        this.LoadLevelPlane();
        this.LoadVirtualCamera();
    }

    protected override void SetupValues()
    {
        base.SetupValues();

        isFollowing = false;
    }

    private void LoadVirtualCamera()
    {
        if (_virtualCamera == null)
            _virtualCamera = GetComponent<CinemachineVirtualCamera>();
    }

    private void LoadLevelPlane()
    {
        if (target == null)
            target = GameObject.FindObjectOfType<LevelPlaneFollowPlayer>().gameObject;
    }

    private void SetTargetForCamera()
    {
        if (_virtualCamera && target)
            _virtualCamera.Follow = target.transform;
    }




}
