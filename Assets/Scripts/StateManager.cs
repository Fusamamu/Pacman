using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class StateManager : MonoBehaviour
{
    public static StateManager sharedInstance { get; set; }

    public TextMeshProUGUI GAMEOVER;
    public TextMeshProUGUI CountDown;

    public enum GAMESTATE
    {
        PLAYING, DEAD, GAMEOVER, MENU
    }

    public GAMESTATE currentGameState = GAMESTATE.PLAYING;

    private void Awake()
    {
        sharedInstance = this;
    }

    private void Start()
    {
        GAMEOVER.enabled = false;
        CountDown.enabled = false;
    }

    private void Update()
    {
        switch (currentGameState)
        {
            case GAMESTATE.GAMEOVER:
                GAMEOVER.enabled = true;
                CountDown.enabled = true;
                break;
        }
    }
}
