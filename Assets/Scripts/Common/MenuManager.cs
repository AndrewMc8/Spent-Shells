using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{
    [SerializeField] GameObject credits;
    [SerializeField] GameObject menu;
    [SerializeField] GameObject settings;
    [SerializeField] GameObject pause;
    [SerializeField] Slider master;
    [SerializeField] Slider music;
    [SerializeField] Slider sfx;



    public void SeeSettings()
    {
        GameManager.Instance.gameObject.GetComponent<ScreenFade>().FadeOut();
        master.value = AudioManager.Instance.masterVolume;
        music.value = AudioManager.Instance.musicVolume;
        sfx.value = AudioManager.Instance.sfxVolume;
        StartCoroutine(GoToSettings());
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
    public void SeeMainMenu()
    {
        GameManager.Instance.gameObject.GetComponent<ScreenFade>().FadeOut();
        StartCoroutine(GoToMainMenu());

    }
    public void SelectLevel(string sceneName)
    {
        GameManager.Instance.pauser.canPause = true;
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

    IEnumerator GoToCredits()
    {
        yield return new WaitUntil(() => GameManager.Instance.gameObject.GetComponent<ScreenFade>().isDone);
        GameManager.Instance.gameObject.GetComponent<ScreenFade>().FadeIn();
        menu.SetActive(false);
        credits.SetActive(true);
    }
    
    IEnumerator GoToSettings()
    {
        yield return new WaitUntil(() => GameManager.Instance.gameObject.GetComponent<ScreenFade>().isDone);
        GameManager.Instance.gameObject.GetComponent<ScreenFade>().FadeIn();
        if (menu) menu.SetActive(false);
        else if (pause) pause.SetActive(false);
        settings.SetActive(true);
    }

    IEnumerator GoToPause()
    {
        yield return new WaitUntil(() => GameManager.Instance.gameObject.GetComponent<ScreenFade>().isDone);
        GameManager.Instance.gameObject.GetComponent<ScreenFade>().FadeIn();
        
        settings.SetActive(false);
        pause.SetActive(true);
    }
    IEnumerator GoToMainMenu()
    {
        yield return new WaitUntil(() => GameManager.Instance.gameObject.GetComponent<ScreenFade>().isDone);
        GameManager.Instance.gameObject.GetComponent<ScreenFade>().FadeIn();
        settings.SetActive(false);
        credits.SetActive(false);
        menu.SetActive(true);
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
}
