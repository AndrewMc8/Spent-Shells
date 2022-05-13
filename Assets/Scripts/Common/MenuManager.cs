using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuManager : Singleton<MenuManager>
{
    [SerializeField] GameObject credits;
    [SerializeField] GameObject settings;
    [SerializeField] GameObject pause;
    [SerializeField] Slider master;
    [SerializeField] Slider music;
    [SerializeField] Slider sfx;

    GameObject menu;

    public void SeeSettings(GameObject menu = null)
    {
        GameManager.Instance.gameObject.GetComponent<ScreenFade>().FadeOut();
        master.value = AudioManager.Instance.masterVolume;
        music.value = AudioManager.Instance.musicVolume;
        sfx.value = AudioManager.Instance.sfxVolume;
        this.menu = menu;
        StartCoroutine(GoToSettings(menu));
    }
    public void SeePause()
    {
        GameManager.Instance.gameObject.GetComponent<ScreenFade>().FadeOut();
        StartCoroutine(GoToPause());
    }

    public void SeeCredits()
    {
        GameManager.Instance.gameObject.GetComponent<ScreenFade>().FadeOut();
        StartCoroutine(GoToCredits());
    }
    public void SelectLevel(string sceneName)
    {
        GameManager.Instance.state = GameManager.State.GAME;
        GameManager.Instance.OnLoadScene(sceneName);
    }
    public void ReloadScene()
    {
        Time.timeScale = 1;
        GameManager.Instance.gameObject.GetComponent<ScreenFade>().FadeOut();
        StartCoroutine(FadeToReload());
    }
    public void QuitGame()
    {

        GameManager.Instance.gameObject.GetComponent<ScreenFade>().FadeOut();
        StartCoroutine(FadeToQuit());
    }

    public void BackFromSettings()
    {
        string currentScene = SceneManager.GetActiveScene().name;
        if (currentScene != "MainMenu")
        {
            SeePause();
        }
        else
        {
            GameManager.Instance.gameObject.GetComponent<ScreenFade>().FadeOut();
            StartCoroutine(BackToMainMenu());
        }
    }

    IEnumerator BackToMainMenu()
    {
        yield return new WaitUntil(() => GameManager.instance.gameObject.GetComponent<ScreenFade>().isDone);
        GameManager.Instance.gameObject.GetComponent<ScreenFade>().FadeIn();
        settings.SetActive(false);
        menu?.SetActive(true);
    }

    IEnumerator GoToCredits()
    {
        yield return new WaitUntil(() => GameManager.Instance.gameObject.GetComponent<ScreenFade>().isDone);
        GameManager.Instance.gameObject.GetComponent<ScreenFade>().FadeIn();

        credits.SetActive(true);
    }

    IEnumerator GoToSettings(GameObject menu)
    {
        yield return new WaitUntil(() => GameManager.Instance.gameObject.GetComponent<ScreenFade>().isDone);
        GameManager.Instance.gameObject.GetComponent<ScreenFade>().FadeIn();
        if (pause) pause.SetActive(false);
        settings.SetActive(true);
        menu?.SetActive(false);
    }

    IEnumerator GoToPause()
    {
        yield return new WaitUntil(() => GameManager.Instance.gameObject.GetComponent<ScreenFade>().isDone);
        GameManager.Instance.gameObject.GetComponent<ScreenFade>().FadeIn();

        settings.SetActive(false);
        pause.SetActive(true);
    }

    IEnumerator FadeToReload()
    {
        yield return new WaitUntil(() => GameManager.Instance.gameObject.GetComponent<ScreenFade>().isDone);
        GameManager.Instance.OnLoadScene(SceneManager.GetActiveScene().name);
    }
    IEnumerator FadeToQuit()
    {
        yield return new WaitUntil(() => GameManager.Instance.gameObject.GetComponent<ScreenFade>().isDone);
        Application.Quit();
        print("quit");
    }


    public void changeMaster()
    {
        AudioManager.Instance.masterVolume = master.value;
    }

    public void changeSFX()
    {
        AudioManager.Instance.sfxVolume = sfx.value;
    }

    public void changeMusic()
    {
        AudioManager.Instance.musicVolume = music.value;
    }

    public void GoFromCanvasToCanvas(GameObject canvasFrom, GameObject canvasTo)
    {
        GameManager.Instance.gameObject.GetComponent<ScreenFade>().FadeOut();
        StartCoroutine(FromCanvasToCanvas(canvasFrom, canvasTo));
    }

    IEnumerator FromCanvasToCanvas(GameObject canvasFrom, GameObject canvasTo)
    {
        yield return new WaitUntil(() => GameManager.instance.gameObject.GetComponent<ScreenFade>().isDone);
        GameManager.Instance.gameObject.GetComponent<ScreenFade>().FadeIn();

        canvasFrom.SetActive(false);
        canvasTo.SetActive(true);
    }
}
