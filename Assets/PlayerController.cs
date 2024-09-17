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

        inputListener = transform.Find("Input Listener").GetComponent<InputListener>();

        playerCharacterObject.transform.position = new Vector2(PV.Owner.ActorNumber, 7);
        playerCharacterObject.SetActive(true);
    }

    
    [PunRPC]
    public void GetTruePositionAndVelocity(Player Requester)
    {
        ChildPV.RPC("UpdateTruePositionAndVelocity", Requester, playerCharacterObject.transform.position, playerCharacterRigidBody2D.velocity);
    }

    [PunRPC]
    public void Move(Vector2 contextValue)
    {
        if (inputListener != null)
        {
            inputListener.Move(contextValue, playerCharacterObject.transform.position, playerCharacterRigidBody2D.velocity);
        }
    }

    [PunRPC]
    public void Jump(bool contextPerformed, bool contextCanceled)
    {
        if (inputListener != null)
        {
            inputListener.Jump(contextPerformed, contextCanceled);
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
