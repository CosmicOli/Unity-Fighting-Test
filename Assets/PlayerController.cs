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
    public PhotonView PV;

    private void Awake()
    {
        PV = GetComponent<PhotonView>();
    }

    // Start is called before the first frame update
    void Start()
    {
        GameObject inputListenerObject = transform.Find("Input Listener").gameObject;
        GameObject inputTransmitterObject = transform.Find("Input Transmitter").gameObject;
        GameObject cameraObject = transform.Find("Player Camera").gameObject;
        GameObject playerCharacterObject = transform.Find("Player Character").gameObject;

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
    public void Move(Vector2 contextValue)
    {
        if (inputListener != null)
        {
            inputListener.Move(contextValue);
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
