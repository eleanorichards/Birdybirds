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

    private BodyRegion region;
    public GameObject LeftWing;
    public GameObject RightWing;
    public GameObject LeftLeg;
    public GameObject RightLeg;

    public Rigidbody2D LWrig;
    public Rigidbody2D RWrig;
    public Rigidbody2D LLrig;
    public Rigidbody2D RLrig;

    private Vector2 pushForce = Vector2.up;
    private GameObject LWGlow;
    private GameObject RWGlow;
    private GameObject LLGlow;
    private GameObject RLGlow;

    // Use this for initialization
    private void Start()
    {
        LeftWing = GameObject.Find("LeftWing");
        RightWing = GameObject.Find("RightWing");
        LeftLeg = GameObject.Find("LeftLeg");
        RightLeg = GameObject.Find("RightLeg");
        LWrig = LeftWing.GetComponent<Rigidbody2D>();
        RWrig = RightWing.GetComponent<Rigidbody2D>();
        LLrig = LeftLeg.GetComponent<Rigidbody2D>();
        RLrig = RightLeg.GetComponent<Rigidbody2D>();

        RLGlow = GameObject.Find("RLGlow");
        LLGlow = GameObject.Find("LLGlow");
        LWGlow = GameObject.Find("LWGlow");
        RWGlow = GameObject.Find("RWGlow");
    }

    // Update is called once per frame
    private void Update()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            RWrig.AddForceAtPosition(pushForce, new Vector2(RightWing.transform.position.x + 3.0f, RightWing.transform.position.y + 0.1f), ForceMode2D.Impulse);
        }
        if (Input.GetButtonDown("Fire2"))
        {
            RWrig.AddForceAtPosition(-pushForce, new Vector2(RightWing.transform.position.x + 3.0f, RightWing.transform.position.y + 0.1f), ForceMode2D.Impulse);
        }

        TakeInput();
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
        if (Input.GetAxis("Vertical") <= -0.5f)
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
                }
                if (Input.GetAxis("Horizontal") < 0)
                {
                    LWrig.AddForceAtPosition(-pushForce, new Vector2(RightWing.transform.position.x + 3.0f, RightWing.transform.position.y + 0.1f), ForceMode2D.Impulse);
                }
                break;

            case BodyRegion.BOTTOM:
                if (Input.GetAxis("Horizontal") > 0)
                {
                    RLrig.AddForceAtPosition(-pushForce, new Vector2(RightWing.transform.position.x + 3.0f, RightWing.transform.position.y + 0.1f), ForceMode2D.Impulse);
                }
                if (Input.GetAxis("Horizontal") < 0)
                {
                    LLrig.AddForceAtPosition(-pushForce, new Vector2(RightWing.transform.position.x + 3.0f, RightWing.transform.position.y + 0.1f), ForceMode2D.Impulse);
                }
                break;

            default:
                break;
        }
    }
}