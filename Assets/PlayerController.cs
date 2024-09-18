using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
using Photon.Realtime;

public class PlayerController : MonoBehaviour
{
    private InputListener inputListener;
    private GameObject playerCharacterObject;
    private Rigidbody2D playerCharacterRigidBody2D;
    public PhotonView PV;
    private PhotonView ChildPV;

    private void Awake()
    {
        PV = GetComponent<PhotonView>();
        ChildPV = transform.Find("Position Corrector").GetComponent<PhotonView>();
    }

    // Start is called before the first frame update
    void Start()
    {
        GameObject inputListenerObject = transform.Find("Input Listener").gameObject;
        GameObject inputTransmitterObject = transform.Find("Input Transmitter").gameObject;
        GameObject cameraObject = transform.Find("Player Camera").gameObject;
        playerCharacterObject = transform.Find("Player Character").gameObject;
        playerCharacterRigidBody2D = playerCharacterObject.GetComponent<Rigidbody2D>();

        if (!PV.IsMine)
        {
            Destroy(cameraObject);
            Destroy(playerCharacterObject.GetComponent<PlayerInput>());
            Destroy(playerCharacterObject.transform.Find("Sword").GetComponent<PlayerInput>());
        }
        else
        {
            Destroy(transform.Find("Position Corrector").Find("Shadow Character").gameObject);
        }

        inputListener = transform.Find("Input Listener").GetComponent<InputListener>();

        this.transform.position += new Vector3(PV.Owner.ActorNumber, 0, 0);
        playerCharacterObject.SetActive(true);
    }

    
    [PunRPC]
    public void GetTruePositionAndVelocity(Player Requester)
    {
        ChildPV.RPC("UpdateTruePositionAndVelocity", Requester, playerCharacterObject.transform.position, playerCharacterRigidBody2D.velocity);
    }

    [PunRPC]
    public void Move(Vector2 contextValue, Vector3 correctPosition, Vector2 correctVelocity)
    {
        if (inputListener != null)
        {
            inputListener.Move(contextValue, correctPosition, correctVelocity);
        }
    }

    [PunRPC]
    public void Jump(bool contextPerformed, bool contextCanceled, Vector3 correctPosition, Vector2 correctVelocity)
    {
        if (inputListener != null)
        {
            inputListener.Jump(contextPerformed, contextCanceled, correctPosition, correctVelocity);
        }
    }

    [PunRPC]
    public void Attack(bool contextPerformed)
    {
        if (inputListener != null)
        {
            inputListener.Attack(contextPerformed);
        }
    }
}
