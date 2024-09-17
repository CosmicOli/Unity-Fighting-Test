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
    private PositionCorrectionBehaviour positionCorrectionBehaviour;

    private PhotonView PV;

    private void Awake()
    {
        PV = transform.parent.GetComponent<PhotonView>();

        Transform playerTransform = transform.parent.Find("Player Character");
        playerBehaviour = playerTransform.GetComponent<PlayerBehaviour>();
        swordBehaviour = playerTransform.Find("Sword").GetComponent<SwordBehaviour>();
        positionCorrectionBehaviour = transform.parent.Find("Position Corrector").GetComponent<PositionCorrectionBehaviour>();
    }

    public void Move(Vector2 contextValue, Vector3 correctPosition, Vector2 correctVelocity)
    {
        if (!PV.IsMine)
        {
            playerBehaviour.Move(contextValue);
            //positionCorrectionBehaviour.UpdateTruePositionAndVelocity(correctPosition, correctVelocity);
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
