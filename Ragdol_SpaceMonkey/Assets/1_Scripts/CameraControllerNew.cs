using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraControllerNew : MonoBehaviour
{
    [SerializeField]
    private CinemachineVirtualCamera playerCamera;
    [SerializeField]
    private CinemachineVirtualCamera bossCamera;

    [ExecuteInEditMode]
    [ContextMenu("PlayerCamera")]
    public void PlayerCamera ()
    {
        playerCamera.Priority = 99;
        bossCamera.Priority = 1;
    }

    [ExecuteInEditMode]
    [ContextMenu("BossCamera")]
    public void BossCamera()
    {
        playerCamera.Priority = 1;
        bossCamera.Priority = 99;
    }
}
