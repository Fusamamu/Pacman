using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateManager : MonoBehaviour
{
    public static StateManager sharedInstance { get; set; }

    public enum GAMESTATE
    {
        PLAYING, GAMEOVER
    }

    public GAMESTATE currentGameState = GAMESTATE.PLAYING;

    private void Awake()
    {
        sharedInstance = this;
    }
}
