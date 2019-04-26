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
    
    // Start is called before the first frame update
    void Start()
    {
        lives = 3;
        checkpoint = player.transform.position;
        deathWallCheckpoint = deathWall.transform.position;
        restart = false;
        //set checkpoint to base checkpoint
    }

    // Update is called once per frame
    void Update()
    {
        if (restart)
            TakeLives(1);
    }

    void TakeLives(int take)
    {
        lives--;
        if (lives <= 0)
            fullRestart();
        else
            Restart();
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
