using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using System.Reflection.Emit;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputListener : MonoBehaviour
{
    private PlayerBehaviour playerBehaviour;
    private SwordBehaviour swordBehaviour;

    private PhotonView PV;

    private void Awake()
    {
        PV = transform.parent.GetComponent<PhotonView>();

        Transform playerTransform = transform.parent.Find("Player Character");
        playerBehaviour = playerTransform.GetComponent<PlayerBehaviour>();
        swordBehaviour = playerTransform.Find("Sword").GetComponent<SwordBehaviour>();
    }

    public void Move(Vector2 contextValue)
    {
        if (!PV.IsMine)
        {
            playerBehaviour.Move(contextValue);
        }
    }

    public void Jump(bool contextPerformed, bool contextCanceled)
    {
        if (!PV.IsMine)
        {
            playerBehaviour.Jump(contextPerformed, contextCanceled);
        }
    }

    public void Attack(bool contextPerformed)
    {
        if (!PV.IsMine)
        {
            swordBehaviour.Attack(contextPerformed);
        }
    }
}
