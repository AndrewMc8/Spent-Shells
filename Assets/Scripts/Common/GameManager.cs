using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class GameManager : Singleton<GameManager>
{
    public enum State
    {
        TITLE,
        PLAYER_START,
        GAME,
        GAME_OVER,
        GAME_WIN,
        GAME_WAIT
    }

    [SerializeField] SceneLoader sceneLoader;
    [SerializeField] GameObject winUI;
    [SerializeField] GameObject loseUI;
    [SerializeField] AudioClip loseMusic;
    [SerializeField] AudioClip winMusic;

    public Pause pauser;
    public GameData gameData;

    public State state = State.TITLE;

    public override void Awake()
    {
        base.Awake();
        state = SceneManager.GetActiveScene().name == "MainMenu" ? State.TITLE : State.GAME;

        SceneManager.activeSceneChanged += OnSceneWasLoaded;
    }

    private void Start()
    {
        InitScene();
    }

    void InitScene()
    {        
        SceneDescriptor sceneDescriptor = FindObjectOfType<SceneDescriptor>();
        if (sceneDescriptor != null)
        {
            if (sceneDescriptor.player) Instantiate(sceneDescriptor.player, sceneDescriptor.playerSpawn.position, sceneDescriptor.playerSpawn.rotation);
            if (sceneDescriptor.music) AudioManager.Instance.PlayMusic(sceneDescriptor.music);
        }
    }

    private void Update()
    {
        switch (state)
        {
            case State.TITLE:
                if (pauser.canPause) pauser.canPause = false;
                break;
            case State.PLAYER_START:
                break;
            case State.GAME:
                if(!pauser.canPause) pauser.canPause = true;
                break;
            case State.GAME_OVER:
                break;
            case State.GAME_WIN:
                break;
            case State.GAME_WAIT:
                break;
            default:
                break;
        }
    }

    public void OnLoadScene(string sceneName)
    {
        sceneLoader.Load(sceneName);
    }

    void OnSceneWasLoaded(Scene current, Scene next)
    {
        InitScene();
    }

    public void RestartGame()
    {
        
    }
}
