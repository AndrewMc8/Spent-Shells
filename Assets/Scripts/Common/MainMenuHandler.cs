using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuHandler : MonoBehaviour
{
    [SerializeField] GameObject credits;
    [SerializeField] GameObject mainMenu;

    public void ToMenu()
    {

        MenuManager.Instance.GoFromCanvasToCanvas(credits, mainMenu);

    }

    public void LoadLevel(string name)
    {
        MenuManager.Instance.SelectLevel(name);
    }

    public void LoadSettings()
    {
        MenuManager.Instance.SeeSettings(mainMenu);
    }
    public void LoadCredits()
    {
        MenuManager.Instance.GoFromCanvasToCanvas(mainMenu, credits);

    }

    public void QuitGame()
    {
        MenuManager.Instance.QuitGame();
    }
}
