using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class MenuManager : MonoBehaviour
{
    // This constant is used to refer to all menus controlled by this menu manager
    [SerializeField]
    private Menu[] Menus;

    // This assigns this script to the class associated with this script instead of a particular instance of this script
    public static MenuManager Instance;
    private void Awake()
    {
        Instance = this;
    }

    // This opens the menu based on the inputted name and hence closes all other menus
    public void OpenMenu(string MenuName)
    {
        for (int i = 0; i < Menus.Length; i++)
        {
            if (Menus[i].MenuName == MenuName)
            {
                if (Menus[i].MenuName == "Create Room")
                {
                    Debug.Log("Huh");
                }
                Menus[i].Open();
            }
            else if (Menus[i].open)
            {
                Menus[i].Close();
            }
        }
    }

    // This opens the menu based on the inputted menu object and hence closes all other menus
    public void OpenMenu(Menu menu)
    {
        for (int i = 0; i < Menus.Length; i++)
        {
            if (Menus[i].open)
            {
                Menus[i].Close();
            }
        }
        menu.Open();
    }

    // This closes the inputted menu
    public void CloseMenu(Menu menu)
    {
        menu.Close();
    }
}
