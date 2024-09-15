using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Menu : MonoBehaviour
{
    // This variable is used to refer to the name of the menu
    public string MenuName;

    // This variable is used to refer to the status of the menu
    public bool open;

    void Start()
    {
        open = gameObject.activeInHierarchy;
    }

    // This opens the menu
    public void Open()
    {
        open = true;
        gameObject.SetActive(true);
    }

    // This closes the menu
    public void Close()
    {
        open = false;
        gameObject.SetActive(false);
    }
}
