using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Singing : MonoBehaviour
{

    private List<string> playerNotes = new List<string>();
    private List<string> friendNotes = new List<string>();

    string[] storedNotes = new string[] { "A", "B", "C", "D", "E", "F", "G" };

     
    public AudioClip[] notes; //7??
    private AudioSource audioSource;
    private AudioClip currentClip;
    private int clipnum = 0;

    private GameState _GS;

    // Use this for initialization
    void Start () {
        _GS = GameObject.Find("Main Camera").GetComponent<GameState>();
        audioSource = gameObject.GetComponent<AudioSource>();
	}
	
	// Update is called once per frame
	void Update ()
    {
        if(_GS.playerTurn)
        {
            TakeInput();

        }
        if(_GS.friendDancing)
        {

        }
	}

    void TakeInput()
    {
        if(Input.GetButtonDown("A"))
        {
            PlayMelody(0);
        }
        if (Input.GetButtonDown("B"))
        {
            PlayMelody(1);
        }
        if (Input.GetButtonDown("X"))
        {
            PlayMelody(2);

        }
        if (Input.GetButtonDown("Y"))
        {
            PlayMelody(3);
        }
    }


    void CompareMelody()
    {
        int NumCorrect = 0;
        for(int i = 0; i < friendNotes.Count; i++)
        if(playerNotes[i] == friendNotes[i])
        {
            NumCorrect++;
            //correct
        }

        if(NumCorrect == friendNotes.Count)
        {
            //MELODY CORRECT
        }
      
    }

    public void PlayCountdown()
    {
        
        audioSource.clip = notes[4];
        audioSource.Play();

    }

    public void PlayMelody(int clipID)
    {
        switch (clipID)
        {
            case 0:
                audioSource.clip = notes[0];
                audioSource.Play();
                //CheckCurrentMove("LL");
                break;
            case 1:
                audioSource.clip = notes[1];
                audioSource.Play();
                //CheckCurrentMove("RL");
                break;
            case 2:
                audioSource.clip = notes[2];
                audioSource.Play();
                //CheckCurrentMove("LW");
                break;
            case 3:
                audioSource.clip = notes[3];
                audioSource.Play();
                //CheckCurrentMove("RW");
                break;
            default:
                break;
        }
    }

    void CheckCurrentMove(string current)
    {
        if (current == _GS.friendMoves[0])
        {
            _GS.PlayUI();

            //StartCoroutine(BeginRound());

        }
    }

    IEnumerator ShowYes()
    {
        yield return new WaitForSeconds(1.5f);

    }
    public void PlayNextNote()
    {
        int noteNum = Random.Range(0, notes.Length);
        audioSource.clip = notes[noteNum]; //select random clip
        friendNotes.Add(storedNotes[noteNum]);
        audioSource.Play();
       // Invoke("PlayNextNote", audioSource.clip.length);

    }

    




}
