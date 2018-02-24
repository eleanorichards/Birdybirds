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


    // Use this for initialization
    void Start () {
        audioSource = gameObject.GetComponent<AudioSource>();
	}
	
	// Update is called once per frame
	void Update ()
    {
        TakeInput();
	}

    void TakeInput()
    {
        if(Input.GetButtonDown("A"))
        {

        }
        if (Input.GetButtonDown("B"))
        {

        }
        if (Input.GetButtonDown("X"))
        {

        }
        if (Input.GetButtonDown("Y"))
        {

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

    void PlayMelody(int clipID)
    {
        switch (clipID)
        {
            case 0:
                break;
            case 1:
                break;
            case 2:
                break;
            default:
                break;
        }
    }

    void PlayNextNote()
    {
        int noteNum = Random.Range(0, notes.Length);
        audioSource.clip = notes[noteNum]; //select random clip
        friendNotes.Add(storedNotes[noteNum]);
        audioSource.Play();
        Invoke("PlayNextNote", audioSource.clip.length);

    }




}
