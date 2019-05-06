using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public PlayerController player;
    private int lives;
    public GameObject deathWall;

    private Vector2 checkpoint;
    private Vector2 deathWallCheckpoint;

    private bool restart;
    private float timeToDie;

    public MainManager mainManager;
    
    // Start is called before the first frame update
    void Start()
    {
        lives = 3;
        checkpoint = player.transform.position;
        deathWallCheckpoint = deathWall.transform.position;
        restart = false;
        Debug.Log("i have trillion life");
        timeToDie = 0;
        mainManager = FindObjectOfType<MainManager>();
        //set checkpoint to base checkpoint
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
        lives--;
        if (lives <= 0)
            fullRestart();
        else
            Restart();
        Debug.Log("mr have " + lives + " lives");
    }

    void Restart()
    {
        //restart at checkpoint
        restart = false;
    }

    void fullRestart()
    {
        //reset game
        restart = false;
    }

    public void setRestart(bool b)
    {
        this.restart = b;
    }
}
