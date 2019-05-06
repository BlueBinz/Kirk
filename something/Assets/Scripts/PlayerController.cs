using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    Rigidbody2D rb2d;
    public int direction; //1 - right, -1 - left
    public Animator anim;
    public List<Collider2D> ObjectsTouchingFeet;
    public Collider2D footCollider;

    public Vector2 playerPos;
    public CapsuleCollider2D characterCollider;

    public GameManager gameManager;

    public float jumpForce;
    public float moveSpeed;
    public float jumpSpeedMulti = 1.7f;

    private bool jump;
    private bool fastFall;
    //private bool dead;
    public bool passedCheckpoint;
    public bool won;

    private Vector2 lastCheckpoint;

    private float timeToCheck;

    // Start is called before the first frame update
    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
        ObjectsTouchingFeet = new List<Collider2D>();
        playerPos = new Vector2(transform.position.x, transform.position.y);
        jumpForce = 1.7f;
        moveSpeed = 9f;
        characterCollider.size = new Vector2(1.2f, 1.6f);
        footCollider.offset = new Vector2(0, -.86f);
        jumpForce *= 732;
        jump = false;
        //dead = false;
        if (anim != null)
            anim.SetInteger("Direction", -1);
        timeToCheck = 0;
    }

    // Update is called once per frame
    void Update()
    {
        //direction = Input.GetAxis("Horizontal")<0f?-1:1;
        playerPos = new Vector2(transform.position.x, transform.position.y + 0.5f);
        if (anim != null && ObjectsTouchingFeet.ToArray().Length > 0)
            anim.SetBool("Jump", false);
        else if (anim != null)
            anim.SetBool("Jump", true);
        if (Input.GetAxis("Horizontal") < 0)
            anim.SetInteger("Direction", -1);   
        else if (Input.GetAxis("Horizontal") > 0)
            anim.SetInteger("Direction", 1);
        jump = ObjectsTouchingFeet.ToArray().Length > 0 && Input.GetAxis("Vertical") > 0 ? true : false;
        fastFall = Input.GetAxis("FastFall") > 0 ? true : false;
        if (timeToCheck > 0)
            timeToCheck -= Time.deltaTime;
        /*Debug.DrawRay(new Vector2(playerPos.x - .55f * anim.GetInteger("Direction"), playerPos.y), new Vector2(-1, 0));
        if (Physics2D.Raycast(new Vector2(playerPos.x - .55f * anim.GetInteger("Direction"), playerPos.y), new Vector2(-1, 0)).distance < .1f)
        {
            Debug.Log("dead");
            dead = true;
        }*/
    }

    //For physics stuff
    void FixedUpdate()
    {
        rb2d.gravityScale = fastFall ? 4 : 1;
        if (anim.GetBool("Jump"))
            rb2d.velocity = (new Vector2(moveSpeed * Input.GetAxis("Horizontal") * jumpSpeedMulti, rb2d.velocity.y));
        else
            rb2d.velocity = (new Vector2(moveSpeed * Input.GetAxis("Horizontal"), rb2d.velocity.y));
        if(ObjectsTouchingFeet.ToArray().Length > 0 && jump)
            Jump(jumpForce);
    }

    void Jump(float jump)
    {
        rb2d.velocity = new Vector2(rb2d.velocity.x, 0);
        rb2d.isKinematic = true;
        rb2d.isKinematic = false;
        rb2d.AddForce(Vector2.up * jump);
    }

    /*public bool GetDead()
    {
        return dead;
    }*/

    public Vector2 GetCheckpoint()
    {
        return lastCheckpoint;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        try
        {
            Debug.Log(collision.name);
            if (timeToCheck <= 0 && collision.name == "Checkpoint")
            {
                passedCheckpoint = true;
                lastCheckpoint = new Vector2(collision.transform.position.x, playerPos.y);
                timeToCheck = 1f;
            }
            if (collision.name == "Goal")
                won = true;
            if (collision.name == "Death_Wall")
                gameManager.setRestart(true);
        }
        catch (Exception e) { }
    }
}
