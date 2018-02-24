using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BodyMovement : MonoBehaviour
{
    private enum BodyRegion
    {
        TOP,
        BOTTOM
    }
    private enum GameMode
    {
        NONE,
        INTRO,
        FRIENDMOVE,
        PLAYERMOVE
    }

    private bool playerBird;

    private BodyRegion region;
    public Transform LeftWing;
    public Transform RightWing;
    public Transform LeftLeg;
    public Transform RightLeg;

    public Rigidbody2D LWrig;
    public Rigidbody2D RWrig;
    public Rigidbody2D LLrig;
    public Rigidbody2D RLrig;

    private Vector2 pushForce = Vector2.up;
    private Transform LWGlow;
    private Transform RWGlow;
    private Transform LLGlow;
    private Transform RLGlow;

    private bool routineFinished = false;

   

    string[] storedMoves = new string[] { "LL", "RL", "LW", "RW"};   // Use this for initialization

    int moveNum = 0;
    int correctMoves = 0;

    private GameState _GS;
    //private GameMode gameMode;

    private Singing singingScript;

    private void Start()
    {
        _GS = GameObject.Find("Main Camera").GetComponent<GameState>();
        // _GS.gameMode = GameMode.INTRO;
        singingScript = GetComponent<Singing>();
        if(gameObject.CompareTag("Player"))
        {
            playerBird = true;
            Debug.Log(gameObject.name + "present");
        }
        else
        {
            Debug.Log("other bird");
            playerBird = false;
        }

        LeftWing = transform.Find("LeftWing");
        RightWing = transform.Find("RightWing");
        LeftLeg = transform.Find("LeftLeg");
        RightLeg = transform.Find("RightLeg");
        LWrig = LeftWing.GetComponent<Rigidbody2D>();
        RWrig = RightWing.GetComponent<Rigidbody2D>();
        LLrig = LeftLeg.GetComponent<Rigidbody2D>();
        RLrig = RightLeg.GetComponent<Rigidbody2D>();

        RLGlow = RightLeg.transform.Find("RLGlow");
        LLGlow = LeftLeg.transform.Find("LLGlow");
        LWGlow = LeftWing.transform.Find("LWGlow");
        RWGlow = RightWing.transform.Find("RWGlow");

        StartCoroutine(BeginRound());
    }

    IEnumerator BeginRound()
    {
        _GS.friendMoves.Clear();
        _GS.playerMoves.Clear();
        singingScript.InvokeRepeating("PlayCountdown", 0, 1.0f);
        yield return new WaitForSeconds(3.0f);
        singingScript.CancelInvoke();
        _GS.SwitchState("friendDancing");
    }

  
    // Update is called once per frame
    private void FixedUpdate()
    {
       if(Input.GetButtonDown("Start"))
        {
            StartCoroutine(BeginRound());
        }
       
        if (_GS.friendDancing)
        {
            if(!playerBird)
            {
                DoADance(1);
                moveNum = 0;
                //Carry Out Moves
            }
            
        }
        if (_GS.playerTurn)
        {
            // gameMode = _GS.gameMode;
            if (playerBird)
            {
                TakeInput();
                //Debug.Log("taking Input...");
            }
        }
        if(_GS.winState)
        {

        }
        if(_GS.loseState)
        {

        }
      
    }

    private void DoADance(int danceLength)
    {
        for(int i = 0; i < danceLength; i++)
        {
            StartCoroutine(MovePart(Random.Range(0, storedMoves.Length)));
            Debug.Log(_GS.friendMoves[i]);
        }
        _GS.SwitchState("playerTurn");
      
        //START PLAYER INPUT
    }

  

    IEnumerator MovePart(int part)
    {
        string currentMove = "";
        switch (part)
        {
            case 0://LL
                LLrig.AddForceAtPosition(-Vector2.right, new Vector2(RightWing.transform.position.x, RightWing.transform.position.y - 0.3f), ForceMode2D.Impulse);
                singingScript.PlayMelody(0);
                _GS.friendMoves.Add("LL");
                currentMove = "LL";
                break;
            case 1: //RL
                RLrig.AddForceAtPosition(Vector2.right, new Vector2(RightWing.transform.position.x, RightWing.transform.position.y - 0.3f), ForceMode2D.Impulse);
                singingScript.PlayMelody(1);
                _GS.friendMoves.Add("RL");
                currentMove = "RL";

                break;
            case 2: //LW
                LWrig.AddForceAtPosition(-pushForce, new Vector2(RightWing.transform.position.x - 2.5f, RightWing.transform.position.y + 0.1f), ForceMode2D.Impulse); //use tranform points for more accurate
                singingScript.PlayMelody(2);
                _GS.friendMoves.Add("LW");
                currentMove = "LW";

                break;
            case 3: //RW
                RWrig.AddForceAtPosition(-pushForce, new Vector2(RightWing.transform.position.x + 2.5f, RightWing.transform.position.y + 0.1f), ForceMode2D.Impulse);                singingScript.PlayMelody(3);
                _GS.friendMoves.Add("RW");
                currentMove = "RW";

                break;
            default:
                break;
        }
        
        yield return new WaitForSeconds(1.5f);
    }


   

    private void TakeInput()
    {       
        if (Input.GetAxis("Vertical") >= 0.5f)
        {
            RLGlow.gameObject.SetActive(false);
            LLGlow.gameObject.SetActive(false);
            RWGlow.gameObject.SetActive(true);
            LWGlow.gameObject.SetActive(true);
            region = BodyRegion.TOP;
        }
        else if (Input.GetAxis("Vertical") <= -0.5f)
        {
            RWGlow.gameObject.SetActive(false);
            LWGlow.gameObject.SetActive(false);
            RLGlow.gameObject.SetActive(true);
            LLGlow.gameObject.SetActive(true);
            region = BodyRegion.BOTTOM;
        }
        switch (region)
        {
            case BodyRegion.TOP:
                if (Input.GetAxis("Horizontal") > 0)
                {
                    StartCoroutine(MovePart("RW"));
                    CheckCurrentMove("RW");
                }
                else if (Input.GetAxis("Horizontal") < 0)
                {
                    StartCoroutine(MovePart("LW"));
                    CheckCurrentMove("LW");

                }
                break;

            case BodyRegion.BOTTOM:
                if (Input.GetAxis("Horizontal") > 0)
                {
                    StartCoroutine(MovePart("RL"));
                    CheckCurrentMove("RL");

                }
                if (Input.GetAxis("Horizontal") < 0)
                {
                    StartCoroutine(MovePart("LL"));
                    CheckCurrentMove("LL");

                }
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

            StartCoroutine(BeginRound());

        }
    }
    IEnumerator MovePart(string part)
    {
        switch (part)
        {
            case "RL":
                RLrig.AddForceAtPosition(Vector2.right, new Vector2(RightWing.transform.position.x, RightWing.transform.position.y - 3.0f), ForceMode2D.Impulse);
                _GS.playerMoves.Add("RL");
                Debug.Log("Player RL");
                moveNum++;
                break;
            case "LL":
                LLrig.AddForceAtPosition(-Vector2.right, new Vector2(RightWing.transform.position.x, RightWing.transform.position.y - 3.0f), ForceMode2D.Impulse);
                _GS.playerMoves.Add("LL");
                Debug.Log("Player LL");
                moveNum++;
                break;
            case "LW":
                LWrig.AddForceAtPosition(-Vector2.right, new Vector2(RightWing.transform.position.x - 3.0f, RightWing.transform.position.y - 1.5f), ForceMode2D.Impulse); //use tranform points for more accurate
                _GS.playerMoves.Add("LW");
                Debug.Log("Player LW");
                moveNum++;
                break;
            case "RW":
                RWrig.AddForceAtPosition(Vector2.right, new Vector2(RightWing.transform.position.x, RightWing.transform.position.y - 1.5f), ForceMode2D.Impulse);
                _GS.playerMoves.Add("RL");
                Debug.Log("Player RL");
                moveNum++;
                break;
      
            default:
                break;
        }

        yield return new WaitForSeconds(1.5f);
    }
   
}