using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class Player_Score : MonoBehaviour
{

    public Text text;  

    public int score;

    public int Score
    {
        get { return score; }
        set { score = value; }
    }

    // Use this for initialization
    void Start()
    {
        Score = 0;
    }

    // Update is called once per frame
    void Update()
    {
        text.text = Score.ToString();
    }
}