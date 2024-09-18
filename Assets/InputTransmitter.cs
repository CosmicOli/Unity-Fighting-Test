using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputTransmitter : MonoBehaviour
{
    private PlayerController playerController;
    private PhotonView PV;

    private GameObject playerCharacterObject;
    private Rigidbody2D playerCharacterRigidBody2D;

    private void Awake()
    {
        playerController = transform.parent.GetComponent<PlayerController>();
        PV = playerController.GetComponent<PhotonView>();
    }

    // Start is called before the first frame update
    void Start()
    {
        playerCharacterObject = transform.parent.Find("Player Character").gameObject;
        playerCharacterRigidBody2D = playerCharacterObject.GetComponent<Rigidbody2D>();
    }

    public void Move(Vector2 contextValue)
    {
        if (PV.IsMine)
        {
            PV.RPC("Move", RpcTarget.Others, contextValue, playerCharacterObject.transform.position, playerCharacterRigidBody2D.velocity);
        }
    }

    public void Jump(bool contextPerformed, bool contextCanceled)
    {
        if (PV.IsMine)
        {
            PV.RPC("Jump", RpcTarget.Others, contextPerformed, contextCanceled, playerCharacterObject.transform.position, playerCharacterRigidBody2D.velocity);
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
