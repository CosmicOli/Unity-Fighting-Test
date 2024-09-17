using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputTransmitter : MonoBehaviour
{
    private PlayerController playerController;
    private PhotonView PV;

    private void Awake()
    {
        playerController = transform.parent.GetComponent<PlayerController>();
        PV = playerController.GetComponent<PhotonView>();
    }

    public void Move(Vector2 contextValue)
    {
        if (PV.IsMine)
        {
            PV.RPC("Move", RpcTarget.Others, contextValue);
        }
    }

    public void Jump(bool contextPerformed, bool contextCanceled)
    {
        if (PV.IsMine)
        {
            PV.RPC("Jump", RpcTarget.Others, contextPerformed, contextCanceled);
        }
    }

    public void Attack(bool contextPerformed)
    {
        if (PV.IsMine)
        {
            PV.RPC("Attack", RpcTarget.Others, contextPerformed);
        }
    }
}
