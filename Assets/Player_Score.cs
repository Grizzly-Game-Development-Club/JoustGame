using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using TMPro;

public class Player_Score : MonoBehaviour
{

    TextMeshProUGUI textmeshPro;  

    public int score;

    public int Score
    {
        get { return score; }
        set { score = value; }
    }

    // Use this for initialization
    void Start()
    {
        textmeshPro = this.GetComponent<TextMeshProUGUI>();
        Score = 0;
    }

    // Update is called once per frame
    void Update()
    {
        textmeshPro.SetText(string.Format("{000000}", Score));
    }
}