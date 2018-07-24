using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JoustGameManager_Master : MonoBehaviour {

    
    public static JoustGameManager_Master instance;
    
    private void Awake()
    {
        MakeSingleton();
    }

    private void MakeSingleton()
    {
        if (instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    public delegate void JoustGameManagerEventHandler();
    public event JoustGameManagerEventHandler GoToMenuToggleEvent;
    public event JoustGameManagerEventHandler ExitGameToggleEvent;
    public event JoustGameManagerEventHandler PauseMenuUIToggleEvent;
    public event JoustGameManagerEventHandler PlayGameToggleEvent;
    public event JoustGameManagerEventHandler RestartGameToggleEvent;
    public event JoustGameManagerEventHandler DeathMenuUIToggleEvent;
    public event JoustGameManagerEventHandler GameOverToggleEvent;


    //Manager Variable
    private bool isGameOver;
    private bool isPauseMenuOn;

    //Game Variable
    private float gameTime;
    private string gameLevelName;


    //Player Variable
    private bool playerDeath;
    private int playerScore;
    public int testScore;

    //
    private Dictionary<string, bool> gameEvent = new Dictionary<string, bool>();


    private GameObject DeathUI;
    private GameObject PauseUI;


    bool checkGameMode() {
        return true;
    }

    //GoToMenuToggleEvent
    public void CallEventGoToMenuToggle() {
        if (GoToMenuToggleEvent != null) {
            GoToMenuToggleEvent();
        }

    }

    //ExitGameToggleEvent
    public void CallEventExitGameToggle() {
        if (ExitGameToggleEvent != null)
        {
            ExitGameToggleEvent();
        }
    }

    //PauseMenuUIToggleEvent
    public void CallEventPauseMenuUIToggle() {
        if (PauseMenuUIToggleEvent != null)
        {
            PauseMenuUIToggleEvent();
        }
    }

    //PlayGameToggleEvent
    public void CallEventPlayGameToggle()
    {
        if (PlayGameToggleEvent != null)
        {
            PlayGameToggleEvent();
        }
    }

    //RestartGameToggleEvent
    public void CallEventRestartGameToggle()
    {
        if (RestartGameToggleEvent != null)
        {
            RestartGameToggleEvent();
        }
    }

    //DeathMenuUIToggleEvent
    public void CallEventDeathMenuUIToggle()
    {
        if (DeathMenuUIToggleEvent != null)
        {
            DeathMenuUIToggleEvent();
        }
    }

    //GameOverToggleEvent
    public void CallEventGameOverToggle()
    {
        if (GameOverToggleEvent != null)
        {
            GameOverToggleEvent();
        }
    }

}
