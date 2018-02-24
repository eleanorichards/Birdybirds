using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameState : MonoBehaviour {
    public bool routineFinished = false;

    public bool friendDancing = false;
    public bool playerTurn = false;
    public bool winState = false;
    public bool loseState = false;

    private AudioSource audioSource;

    public AudioSource UIaudio;

    public AudioClip[] clips;

    public List<string> playerMoves = new List<string>();
    public List<string> friendMoves = new List<string>();

    public List<string> friendSounds = new List<string>();

    public enum GameMode
    {
        NONE,
        INTRO,
        FRIENDMOVE,
        PLAYERMOVE
    }

    public GameMode gameMode = GameMode.NONE;

	// Use this for initialization
	void Start ()
    {
        //audioSource = GameObject.GetComponent<AudioSource>();
	}
	
	// Update is called once per frame
	void Update ()
    {
		if(friendDancing)
        {

        }
        else if(playerTurn)
        {

        }
        else if(winState)
        {

        }
        else if(loseState)
        {

        }
	}

    public void PlayUI()
    {
        UIaudio.Play();
    }
    public void SwitchState(string state)
    {
        switch (state)
        {
            case "friendDancing":
                friendDancing = true;
                playerTurn = false;
                winState = false;
                loseState = false;
                break;
            case "playerTurn":
                friendDancing = false;
                playerTurn = true;
                winState = false;
                loseState = false;
                break;
                    case "winState":
                friendDancing = false;
                playerTurn = false;
                winState = true;
                loseState = false;
                break;
            case "loseState":
                friendDancing = false;
                playerTurn = false;
                winState = false;
                loseState = true;
                break;
            default:
                break;
        }
    }
}
