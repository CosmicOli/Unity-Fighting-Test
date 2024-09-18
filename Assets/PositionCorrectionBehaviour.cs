using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PositionCorrectionBehaviour : MonoBehaviour
{
    Vector2 previousTargetPosition;
    Vector2 targetPosition;
    Vector2 playerVelocity;
    bool doShadowLerp = true;
    Rigidbody2D playerRigidBody2D;
    Rigidbody2D shadowRigidBody2D;
    PlayerBehaviour playerBehaviour;

    int frameCount = 0;

    float gravityScale;
    float HorizontalDrag;
    float HorizontalAccelerationPower;
    float MaximumHorizontalSpeedFromPower;
    float TerminalSpeed;
    float horizontalAccelerationDirection;

    private Transform playerCharacterTransform;
    private Transform shadowCharacterTransform;
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
        shadowCharacterTransform = transform.Find("Shadow Character");
        playerController = transform.parent.GetComponent<PlayerController>();

        playerBehaviour = playerCharacterTransform.GetComponent<PlayerBehaviour>();

        playerRigidBody2D = playerCharacterTransform.GetComponent<Rigidbody2D>();
        shadowRigidBody2D = shadowCharacterTransform.GetComponent<Rigidbody2D>();
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
            if (frameCount == 0)
            {
                //ParentPV.RPC("GetTruePositionAndVelocity", PV.Owner, PhotonNetwork.LocalPlayer);
            }
            frameCount = (frameCount + 1) % 50;

            Vector2 shadowVelocity = shadowRigidBody2D.velocity;

            horizontalAccelerationDirection = playerBehaviour.horizontalAccelerationDirection;
            float newHorizontalVelocity = playerBehaviour.calculateGravitylessAxisVelocity(shadowVelocity.x, HorizontalDrag, HorizontalAccelerationPower, horizontalAccelerationDirection, MaximumHorizontalSpeedFromPower);

            float newVerticalVelocity = shadowVelocity.y;

            // If faster than the maximum speed then set to the maximum speed.
            // It is assumed that gravity acts downwards.
            if (Mathf.Abs(newVerticalVelocity) >= Mathf.Abs(TerminalSpeed) && Mathf.Sign(-1 * newVerticalVelocity) > 0)
            {
                newVerticalVelocity = -1 * TerminalSpeed;
            }

            shadowVelocity = new Vector2(newHorizontalVelocity, newVerticalVelocity);

            shadowRigidBody2D.velocity = shadowVelocity;

            targetPosition = shadowCharacterTransform.position;

            if ((targetPosition - new Vector2(playerCharacterTransform.position.x, playerCharacterTransform.position.y)).magnitude > 1)
            {
                playerCharacterTransform.position = targetPosition;
            }
            else //if (doShadowLerp)
            {
                playerCharacterTransform.position = Vector2.Lerp(playerCharacterTransform.position, targetPosition, 0.3f);
            }
            //else
            //{
                //ParentPV.RPC("GetTruePositionAndVelocity", PV.Owner, PhotonNetwork.LocalPlayer);
                //playerCharacterTransform.position = targetPosition;

                // Teleport both objects in contact to their "correct" locations ig?
            //}
        }
    }

    public void ShadowLerp(bool state)
    {
        doShadowLerp = state;
    }
    
    [PunRPC]
    public void UpdateTruePositionAndVelocity(Vector3 correctPosition, Vector2 correctVelocity)
    {
        previousTargetPosition = targetPosition;
        targetPosition = correctPosition;
        shadowCharacterTransform.position = targetPosition;
        shadowRigidBody2D.velocity = correctVelocity;
    }
}
