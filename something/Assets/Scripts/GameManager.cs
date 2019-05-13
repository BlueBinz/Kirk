using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public PlayerController player;
    public int lives;
    public GameObject deathWall;

    public Vector2 checkpoint;
    public Vector2 deathWallCheckpoint;

    public Vector2 originalCheckpoint;
    public Vector2 originalDeathWallCheckpoint;

    private bool restart;
    private float timeToDie;

    public MainManager mainManager;

    private void Awake()
    {
        checkpoint = player.transform.position;
        originalCheckpoint = checkpoint;
        deathWallCheckpoint = deathWall.transform.position;
        originalDeathWallCheckpoint = deathWallCheckpoint;
    }
    // Start is called before the first frame update
    void Start()
    {
        lives = 3;
        restart = false;
        Debug.Log("i have trillion life");
        timeToDie = 0;

        //post death
        mainManager = FindObjectOfType<MainManager>();
        lives = int.Parse(mainManager.livesText.text);
        checkpoint = mainManager.checkpoint;
        deathWallCheckpoint = mainManager.deathWallCheckpoint;
        player.transform.position = checkpoint;
        deathWall.transform.position = deathWallCheckpoint;
        //set checkpoint to base checkpoint
        Debug.Log("game manager has " + lives + " life.");
    }

    // Update is called once per frame
    void Update()
    {
        if (!player.won)
        {
            if (timeToDie > 0)
                timeToDie -= Time.deltaTime;
            if (player.passedCheckpoint)
            {
                //change checkpoint if too close
                checkpoint = player.GetCheckpoint();
                deathWallCheckpoint = deathWall.transform.position;
                player.passedCheckpoint = false;
            }
            if (restart && timeToDie <= 0)
            {
                TakeLives(1);
                mainManager.Die();
                timeToDie = 1f;
            }
        }
    }

    void TakeLives(int take)
    {
        lives -= take;
        restart = false;
        Debug.Log("mr have " + lives + " lives");
    }

    /*public void FullReset()
    {
        mainManager = FindObjectOfType<MainManager>();
        lives = 3;
        restart = false;
        Debug.Log("i have trillion life");
        timeToDie = 0;
    }*/

    public void setRestart(bool b) { this.restart = b; }
}
