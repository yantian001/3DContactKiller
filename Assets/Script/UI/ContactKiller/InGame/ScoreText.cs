﻿using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ScoreText : MonoBehaviour
{

    public Text scoreText;

    // Use this for initialization
    void Start()
    {
        if (!scoreText)
        {
            scoreText = GetComponent<Text>();
        }
    }

    // Update is called once per frame
    void Update()
    {
        scoreText.text = ScoresManager.instance.currentScore.ToString();
    }
}
