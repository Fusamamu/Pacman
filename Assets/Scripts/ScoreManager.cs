using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager sharedInstance { get; set; }

    public TextMeshProUGUI liveText;
    public TextMeshProUGUI ScoreText;
    public TextMeshProUGUI DistanceText;

    public int live = 3;
    public int score;
    public double distance;
    

    private void Awake()
    {
        sharedInstance = this;
    }

    public void OnPacmanRunning()
    {
        distance += 1;
        DistanceText.text = "DISTANCE: " + distance + " M";
    }

    public void OnSmallCoinCollected()
    {
        score += 10;
        ScoreText.text = "SCORE: " + score;
    }

    public void DecreaseLive()
    {
        live--;
        liveText.text = "LIVE x " + live; 
    }

    
}
