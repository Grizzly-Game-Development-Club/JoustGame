using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class JoustGameManager_PlayGame : MonoBehaviour {

    private JoustGameManager_Master joustGameManagerMaster;

    void OnEnable(){
        setInitialReference();
        joustGameManagerMaster.PlayGameToggleEvent += PlayGame;
    }

    void OnDisable(){
        joustGameManagerMaster.PlayGameToggleEvent -= PlayGame;
    }

    void setInitialReference() {
        joustGameManagerMaster = GetComponent<JoustGameManager_Master>();
    }

    void PlayGame() {
        SceneManager.LoadScene(0);
    }


}
