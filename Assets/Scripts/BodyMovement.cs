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

    public List<string> playerMoves = new List<string>();
    public List<string> friendMoves = new List<string>();

    string[] storedMoves = new string[] { "LL", "RL", "LW", "RW"};   // Use this for initialization

    int moveNum = 0;
    int correctMoves = 0;

    private GameState _GS;
    //private GameMode gameMode;

    private void Start()
    {
        _GS = GameObject.Find("Main Camera").GetComponent<GameState>();
       // _GS.gameMode = GameMode.INTRO;

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
        yield return new WaitForSeconds(3.0f);
        _GS.SwitchState("friendDancing");
    }

    // Update is called once per frame
    private void FixedUpdate()
    {
       

        if (_GS.friendDancing)
        {
            if(!playerBird)
            {
                DoADance(3);
                moveNum = 0;
                //Carry Out Moves
            }
            if(playerBird)
            {
                
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
            Debug.Log(friendMoves[i]);
        }
        _GS.SwitchState("playerTurn");
        Debug.Log("Dance finished" + routineFinished);
        //START PLAYER INPUT
    }

  

    IEnumerator MovePart(int part)
    {
        switch (part)
        {
            case 0://LL
                LLrig.AddForceAtPosition(-Vector2.right, new Vector2(RightWing.transform.position.x, RightWing.transform.position.y - 2.0f), ForceMode2D.Impulse);
                friendMoves.Add("LL");
                break;
            case 1: //RL
                RLrig.AddForceAtPosition(Vector2.right, new Vector2(RightWing.transform.position.x, RightWing.transform.position.y - 2.0f), ForceMode2D.Impulse);
                friendMoves.Add("RL");

                break;
            case 2: //LW
                LWrig.AddForceAtPosition(-pushForce, new Vector2(RightWing.transform.position.x - 3.0f, RightWing.transform.position.y + 0.1f), ForceMode2D.Impulse); //use tranform points for more accurate
                friendMoves.Add("LW");

                break;
            case 3: //RW
                RWrig.AddForceAtPosition(-pushForce, new Vector2(RightWing.transform.position.x + 3.0f, RightWing.transform.position.y + 0.1f), ForceMode2D.Impulse);
                friendMoves.Add("RW");

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
                }
                else if (Input.GetAxis("Horizontal") < 0)
                {
                    StartCoroutine(MovePart("LW"));
                }
                break;

            case BodyRegion.BOTTOM:
                if (Input.GetAxis("Horizontal") > 0)
                {
                    StartCoroutine(MovePart("RL"));

                }
                if (Input.GetAxis("Horizontal") < 0)
                {
                    StartCoroutine(MovePart("LL"));

                }
                break;

            default:
                break;
        }

        if(moveNum == friendMoves.Count)
        {
            //CompareMoves();
            for(int i = 0; i < friendMoves.Count; i++)
            {
                if(friendMoves[i] == playerMoves[i])
                {
                    correctMoves++;
                }
            }
        }
        if(correctMoves == friendMoves.Count)
        {
            //Debug.Log("Danced Right");
            _GS.routineFinished = false;
            //YOU DID THE DANCE
        }
    }

    IEnumerator MovePart(string part)
    {
        switch (part)
        {
            case "RL":
                RLrig.AddForceAtPosition(Vector2.right, new Vector2(RightWing.transform.position.x, RightWing.transform.position.y - 3.0f), ForceMode2D.Impulse);
                playerMoves.Add("RL");
                Debug.Log("Player RL");
                moveNum++;
                break;
            case "LL":
                LLrig.AddForceAtPosition(-Vector2.right, new Vector2(RightWing.transform.position.x, RightWing.transform.position.y - 3.0f), ForceMode2D.Impulse);
                playerMoves.Add("LL");
                Debug.Log("Player LL");
                moveNum++;
                break;
            case "LW":
                LWrig.AddForceAtPosition(-Vector2.right, new Vector2(RightWing.transform.position.x - 3.0f, RightWing.transform.position.y - 1.5f), ForceMode2D.Impulse); //use tranform points for more accurate
                playerMoves.Add("LW");
                Debug.Log("Player LW");
                moveNum++;
                break;
            case "RW":
                RWrig.AddForceAtPosition(Vector2.right, new Vector2(RightWing.transform.position.x, RightWing.transform.position.y - 1.5f), ForceMode2D.Impulse);
                playerMoves.Add("RL");
                Debug.Log("Player RL");
                moveNum++;
                break;
      
            default:
                break;
        }

        yield return new WaitForSeconds(1.5f);
    }
    void SetGlow(string glowPart)
    {
        //if(glowPart)
        //{
        //    //setactivetrue
        //}
        //else
        //{
        //    //setactive false
        //}
    }
}