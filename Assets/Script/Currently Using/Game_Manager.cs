using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Global_Enum;

public class Game_Manager : MonoBehaviour
{

    //Store script own instance
    public static Game_Manager instance;

    private float gameTime;

    public GameObject DeathUI;
    public GameObject PauseUI;

    
    public GameObject[] player;
    private int[] playerScore;
    public GameObject[] playerScoreObject;

    private GameStatus currentGameStatus;

    void Start()
    {
        playerScore = new int[2];

        if (player[0] != null)
        {
            player[0].GetComponent<Player_Controller>().PlayerID = 0;
            playerScore[0] = 0;
        }
        if (player[1] != null)
        {
            player[1].GetComponent<Player_Controller>().PlayerID = 1;
            playerScore[1] = 0;
        }
        CurrentGameStatus = GameStatus.WAITING;
    }

    void Update()
    {
        UpdatePlayerScore();
        

        switch (CurrentGameStatus) {
            case GameStatus.PLAYING:
                PlayGameStatus();
                CurrentGameStatus = GameStatus.WAITING;
                break;
            case GameStatus.RESUME:
                ResumeGameStatus();
                CurrentGameStatus = GameStatus.WAITING;
                break;
            case GameStatus.WAITING:
                break;
            case GameStatus.PAUSED:
                PauseGameStatus();
                CurrentGameStatus = GameStatus.WAITING;
                break;
            case GameStatus.DEATH:
                Debug.Log("Work");
                GameOverStatus();
                CurrentGameStatus = GameStatus.WAITING;
                break;

        }
    }


    void UpdatePlayerScore() {
        if (player[0] != null)
        {
            playerScoreObject[0].GetComponent<Player_Score>().Score = playerScore[0];
        }
        if (player[1] != null)
        {
            playerScoreObject[1].GetComponent<Player_Score>().Score = playerScore[1];
        }
    }
    public void addPlayerScore(int value, int playerID)
    {
        playerScore[playerID] += value;
    }
    public void decreasePlayerScore(int value, int playerID)
    {
        playerScore[playerID] -= value;
    }

    void PlayGameStatus()
    {
        Time.timeScale = 1;
        PauseUI.SetActive(false);
        DeathUI.SetActive(false);
    }

    void ResumeGameStatus()
    {
        Time.timeScale = 1;
        PauseUI.SetActive(false);
        DeathUI.SetActive(false);
    }

    void PauseGameStatus()
    {
        PauseUI.SetActive(true);
        Time.timeScale = 0;
    }

    void GameOverStatus()
    {
        DeathUI.SetActive(true);
        Time.timeScale = 0;
    }

    public GameStatus CurrentGameStatus
    {
        get { return currentGameStatus; }
        set { currentGameStatus = value; }
    }

    public GameObject[] Player
    {
        get { return player;  }
        set  {  player = value;  }
    }

    public int[] PlayerScore
    {
        get { return playerScore; }
        set { playerScore = value; }
    }



    /*
    // Use this for initialization
    void Start () {
        gameStatus.Add("playGameStatus", true);
        gameStatus.Add("playerPauseStatus", false);
        gameStatus.Add("resumeGameStatus", false);
        gameStatus.Add("gameOverStatus", false);
        setPlayerDeath(false);
        setPlayerScore(000);
        foreach (GameObject go in Resources.FindObjectsOfTypeAll(typeof(GameObject)) as GameObject[]) {
            Debug.Log(go.name);
            switch (go.name) {
                case "Death Menu":
                    setDeathUI(go);
                    setDeathUIActive(false);
                    Debug.Log(DeathUI.name + "123");
                    break;
                case "Pause Menu":
                    setPauseUI(go);
                    setPauseUIActive(false);
                    break;
            }
        }
        
    }
	
	// Update is called once per frame
	void Update () {
	}
    //Deals with Player Death
    private bool getPlayerDeath() {
        return playerDeath;
    }
    public void setPlayerDeath(bool playerDeath) {
        this.playerDeath = playerDeath;
    }
    //Deals with Player Score
    
    void setPlayerScore(int playerScore) {
        this.playerScore = playerScore;
    }
    
    //Deals with DeathUI
    private GameObject getDeathUI() {
        return DeathUI;
    }
    void setDeathUI(GameObject DeathUI) {
        this.DeathUI = DeathUI;
    }
    void setDeathUIActive(bool value) {
        DeathUI.SetActive(value);
    }
    //Deals with PauseUI
    private GameObject getPauseUI()
    {
        return PauseUI;
    }
    void setPauseUI(GameObject PauseUI)
    {
        this.PauseUI = PauseUI;
    }
    void setPauseUIActive(bool value)
    {
        PauseUI.SetActive(value);
    }
    //Change status of the game
    public void changeGameStatus(string key, bool value) {
        if (value == true) {
            switch (key) {
                case "gameOverStatus":
                    if (getPlayerDeath() == true)
                    {
                        gameStatus["playGameStatus"] = false;
                        gameStatus["gameOverStatus"] = true;
                        gameOverEvent();
                    }
                    else {
                        Debug.Log("Invalid Condition");
                    }
                    break;
                case "playerPauseStatus":
                    gameStatus["playGameStatus"] = false;
                    gameStatus["playerPauseStatus"] = true;
                    playerPauseEvent();
                    break;
                case "resumeGameStatus":
                    gameStatus["resumeGameStatus"] = false;
                    gameStatus["playGameStatus"] = true;
                    playGameStatus();
                    break;
            }
        }
    }
    
    */
}