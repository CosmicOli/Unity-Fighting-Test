using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PositionCorrectionBehaviour : MonoBehaviour
{
    Vector2 previousTargetPosition;
    Vector2 targetPosition;
    Vector2 predictedTargetPosition;
    Vector2 playerVelocity;
    bool predicting;
    Rigidbody2D playerRigidBody2D;
    PlayerBehaviour playerBehaviour;

    float gravityScale;
    float HorizontalDrag;
    float HorizontalAccelerationPower;
    float MaximumHorizontalSpeedFromPower;
    float TerminalSpeed;
    float horizontalAccelerationDirection;

    private Transform playerCharacterTransform;
    private PlayerController playerController;
    private PhotonView ParentPV;
    private PhotonView PV;

    private void Awake()
    {
        ParentPV = transform.parent.GetComponent<PhotonView>();
        PV = GetComponent<PhotonView>();
    }

    // Start is called before the first frame update
    void Start()
    {
        playerCharacterTransform = transform.parent.Find("Player Character");
        playerController = transform.parent.GetComponent<PlayerController>();

        playerBehaviour = playerCharacterTransform.GetComponent<PlayerBehaviour>();

        playerRigidBody2D = playerCharacterTransform.GetComponent<Rigidbody2D>();
        gravityScale = playerRigidBody2D.gravityScale;
        HorizontalDrag = playerBehaviour.HorizontalDrag;
        HorizontalAccelerationPower = playerBehaviour.HorizontalAccelerationPower;
        MaximumHorizontalSpeedFromPower = playerBehaviour.MaximumHorizontalSpeedFromPower;
        TerminalSpeed = playerBehaviour.TerminalSpeed;
    }

    void FixedUpdate()
    { 
        if (!ParentPV.IsMine)
        {
            ParentPV.RPC("GetTruePositionAndVelocity", PV.Owner, PhotonNetwork.LocalPlayer);

            if (previousTargetPosition == targetPosition && playerVelocity.sqrMagnitude > 0)
            {
                if (!predicting)
                {
                    predicting = true;
                    predictedTargetPosition = targetPosition;
                }

                horizontalAccelerationDirection = playerBehaviour.horizontalAccelerationDirection;
                float newHorizontalVelocity = playerBehaviour.calculateGravitylessAxisVelocity(playerVelocity.x, HorizontalDrag, HorizontalAccelerationPower, horizontalAccelerationDirection, MaximumHorizontalSpeedFromPower);

                float newVerticalVelocity = playerVelocity.y;

                // If faster than the maximum speed then set to the maximum speed.
                // It is assumed that gravity acts downwards.
                if (Mathf.Abs(newVerticalVelocity) >= Mathf.Abs(TerminalSpeed) && Mathf.Sign(-1 * newVerticalVelocity) > 0)
                {
                    newVerticalVelocity = -1 * TerminalSpeed;
                }
                else
                {
                    newVerticalVelocity = playerVelocity.y + gravityScale * 1f / 50f;
                }

                playerVelocity = new Vector2(newHorizontalVelocity, newVerticalVelocity);

                predictedTargetPosition += 1f / 50f * playerVelocity;
            }
            else
            {
                predicting = false;
                predictedTargetPosition = targetPosition;
            }



            //Debug.Log(playerCharacterTransform.position);
            //Debug.Log(targetPosition);
            //Debug.Log("");

            // Neither update is definitively "later" than the other
            // It is less rubberbanding, more spring oscillationing
            // I could request the information simultaneously, hence guaranteeing they are synced?
            // The problem is that move inputs are less frequent than updates
            // The only reason move inputs are smooth is because of this, as tethering locations together 50 times per second shows the imperfections
            // I need to sync predicted and accurate inputs
            // Maybe put a time stamp in the sends?

            // If there is no lerping, then predicted movement is smooth
            // If I instead took the start location of the move input and projected where that would be and lerped to that position instead, it should be more accurate

            if ((predictedTargetPosition - new Vector2(playerCharacterTransform.position.x, playerCharacterTransform.position.y)).magnitude > 1)
            {
                playerCharacterTransform.position = predictedTargetPosition;
            }
            else
            {
                playerCharacterTransform.position = Vector2.Lerp(playerCharacterTransform.position, predictedTargetPosition, 0.15f);
            }
        }
    }

    
    [PunRPC]
    public void UpdateTruePositionAndVelocity(Vector3 correctPosition, Vector2 correctVelocity)
    {
        Debug.Log(correctPosition);
        previousTargetPosition = targetPosition;
        targetPosition = correctPosition;
        playerVelocity = correctVelocity;
    }
}
