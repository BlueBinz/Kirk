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
    
    // Start is called before the first frame update
    void Start()
    {
        lives = 3;
        checkpoint = player.transform.position;
        deathWallCheckpoint = deathWall.transform.position;
        //set checkpoint to base checkpoint
    }

    // Update is called once per frame
    void Update()
    {
        if (player.GetDead())
            TakeLives(1);
    }

    void TakeLives(int take)
    {
        lives--;
        if (lives <= 0)
            Restart();
    }

    void Restart()
    {
        //restart at checkpoint
    }
}
