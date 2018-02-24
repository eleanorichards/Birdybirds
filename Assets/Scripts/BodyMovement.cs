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
    private GameObject LWGlow;
    private GameObject RWGlow;
    private GameObject LLGlow;
    private GameObject RLGlow;

    private bool routineFinished = false;

    public List<string> playerMoves = new List<string>();
    public List<string> friendMoves = new List<string>();

    string[] storedMoves = new string[] { "LL", "RL", "LW", "RW"};   // Use this for initialization

    int moveNum = 0;
    int correctMoves = 0;

    private void Start()
    {
        if(gameObject.CompareTag("Player"))
        {
            playerBird = true;
            Debug.Log(gameObject.name + "present");
        }
        else
        {
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

        RLGlow = RightLeg.transform.Find("RLGlow").GetComponent<GameObject>();
        LLGlow = LeftLeg.transform.Find("LLGlow").GetComponent<GameObject>();
        LWGlow = LeftWing.transform.Find("LWGlow").GetComponent<GameObject>();
        RWGlow = RightWing.transform.Find("RWGlow").GetComponent<GameObject>();
    }

    // Update is called once per frame
    private void Update()
    {
       // if(playerBird)
       // {
            //if(routineFinished)
            //{
                Debug.Log("taking Input...");
                TakeInput();

           // }
       // }

       if(!routineFinished)
        {
            if(!playerBird)
            {
                DoADance(3);
                moveNum = 0;
                //Carry Out Moves
            }
        }
      
    }

    private void DoADance(int danceLength)
    {
        for(int i = 0; i < danceLength; i++)
        {
            MovePart(Random.Range(0, storedMoves.Length));
            StartCoroutine(WaitForSeconds(2.0f));
            Debug.Log(friendMoves[i]);
        }
        routineFinished = true;
        Debug.Log("Dance finished" + routineFinished);
        //START PLAYER INPUT
    }

    IEnumerator WaitForSeconds(float time)
    {
        yield return new WaitForSeconds(time);
    }

    void MovePart(int part)
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
    }

    private void TakeInput()
    {       
        if (Input.GetAxis("Vertical") >= 0.5f)
        {
            RLGlow.SetActive(false);
            LLGlow.SetActive(false);
            RWGlow.SetActive(true);
            LWGlow.SetActive(true);
            region = BodyRegion.TOP;
        }
        else if (Input.GetAxis("Vertical") <= -0.5f)
        {
            RWGlow.SetActive(false);
            LWGlow.SetActive(false);
            RLGlow.SetActive(true);
            LLGlow.SetActive(true);
            region = BodyRegion.BOTTOM;
        }
        switch (region)
        {
            case BodyRegion.TOP:
                if (Input.GetAxis("Horizontal") > 0)
                {
                    RWrig.AddForceAtPosition(-pushForce, new Vector2(RightWing.transform.position.x + 3.0f, RightWing.transform.position.y + 0.1f), ForceMode2D.Impulse);
                    playerMoves.Add("RW");
                    Debug.Log("Player RW");
                    moveNum++;
                }
                else if (Input.GetAxis("Horizontal") < 0)
                {
                    LWrig.AddForceAtPosition(-pushForce, new Vector2(RightWing.transform.position.x - 3.0f, RightWing.transform.position.y + 0.1f), ForceMode2D.Impulse); //use tranform points for more accurate
                    playerMoves.Add("LW");
                    Debug.Log("Player LW");
                    moveNum++;
                }
                break;

            case BodyRegion.BOTTOM:
                if (Input.GetAxis("Horizontal") > 0)
                {
                    RLrig.AddForceAtPosition(Vector2.right, new Vector2(RightWing.transform.position.x, RightWing.transform.position.y - 2.0f), ForceMode2D.Impulse);
                    playerMoves.Add("RL");
                    Debug.Log("Player RL");
                    moveNum++;
                }
                if (Input.GetAxis("Horizontal") < 0)
                {
                    LLrig.AddForceAtPosition(-Vector2.right, new Vector2(RightWing.transform.position.x, RightWing.transform.position.y - 2.0f), ForceMode2D.Impulse);
                    playerMoves.Add("LL");
                    Debug.Log("Player LL");
                    moveNum++;
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
            Debug.Log("Danced Right");
            //YOU DID THE DANCE
        }
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