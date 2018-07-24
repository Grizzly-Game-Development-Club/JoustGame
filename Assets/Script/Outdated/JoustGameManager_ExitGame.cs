using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JoustGameManager_ExitGame : MonoBehaviour {

    private JoustGameManager_Master joustGameManagerMaster;

    void OnEnable()
    {
        setInitialReference();
        joustGameManagerMaster.ExitGameToggleEvent += ExitGame;
    }

    void OnDisable()
    {
        joustGameManagerMaster.ExitGameToggleEvent -= ExitGame;
    }

    void setInitialReference()
    {
        joustGameManagerMaster = GetComponent<JoustGameManager_Master>();
    }

    void ExitGame()
    {
    # if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
    #else
        Application.Quit();
    #endif
    }
}
