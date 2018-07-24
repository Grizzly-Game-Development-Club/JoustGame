using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JoustGameManager_GameOver : MonoBehaviour {

    private JoustGameManager_Master joustGameManagerMaster;
    public GameObject panelGameOver;

    void OnEnable()
    {
        SetInitialReferences();
        joustGameManagerMaster.GameOverToggleEvent += TurnOnGameOverPanel;
    }

    void OnDisable()
    {
        joustGameManagerMaster.GameOverToggleEvent -= TurnOnGameOverPanel;
    }

    void SetInitialReferences()
    {
        joustGameManagerMaster = GetComponent<JoustGameManager_Master>();
    }

    void TurnOnGameOverPanel()
    {
        if (panelGameOver != null)
        {
            panelGameOver.SetActive(true);
        }
    }
}
