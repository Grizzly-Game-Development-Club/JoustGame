using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JoustGameManager : MonoBehaviour {

    public string gameLevelName = "";
    private Dictionary<string, bool> gameStatus = new Dictionary<string, bool>();

    private bool playerDeath;
    private int playerScore;

    private GameObject DeathUI;
    private GameObject PauseUI;


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
    public int getPlayerScore() {
        return playerScore;
    }
    void setPlayerScore(int playerScore) {
        this.playerScore = playerScore;
    }
    public void addPlayerScore(int value) {
        playerScore += value;
    }
    public void decreasePlayerScore(int value)
    {
        playerScore -= value;
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

    //Events of the game
    void gameOverEvent() {
        setDeathUIActive(true);
        Time.timeScale = 0;
    }
    void playGameStatus() {
        Time.timeScale = 1;
        setDeathUIActive(false);
        setPauseUIActive(false);
    }
    void playerPauseEvent() {
        setPauseUIActive(true);
        Time.timeScale = 0;
    }


}
