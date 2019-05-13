using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainManager : MonoBehaviour
{
    public int LevelBuildIndex = 1;
    public float timeToReset = 0;
    public GameObject transition;
    public GameObject finalTransition;
    public GameObject winTransition;
    public GameManager gameManager;

    public Text livesText;

    private bool fullRestartNeeded = false;
    
    public Vector2 checkpoint;
    public Vector2 deathWallCheckpoint;

    private void Awake()
    {
        SceneManager.LoadSceneAsync(LevelBuildIndex, LoadSceneMode.Additive);
    }

    private void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
        livesText.text = 3.ToString();
        checkpoint = gameManager.checkpoint;
        deathWallCheckpoint = gameManager.deathWallCheckpoint;
    }

    public void ResetLevel()
    {
        if (timeToReset <= 0 && int.Parse(livesText.text) > 0)
        {
            checkpoint = gameManager.checkpoint;
            deathWallCheckpoint = gameManager.deathWallCheckpoint;
            transition.SetActive(true);
            StartCoroutine(FollowThroughA(SceneManager.UnloadSceneAsync(LevelBuildIndex)));
            timeToReset = 1f;
        }
        else if(int.Parse(livesText.text) <= 0)
        {
            finalTransition.SetActive(true);
            StartCoroutine(FollowThroughC(SceneManager.UnloadSceneAsync(LevelBuildIndex)));
            fullRestartNeeded = true;
        }
    }

    private void Update()
    {
        if (Input.GetAxis("Reset") > 0)
            ResetLevel();
        if (timeToReset > 0)
            timeToReset -= Time.deltaTime;
        if (gameManager.player.won)
        {
            winTransition.SetActive(true);
            NextLevel();
        }
    }

    public void Die()
    {
        livesText.text = /*(int.Parse(livesText.text) - 1).ToString();*/ gameManager.lives.ToString();
        Debug.Log("stored " + livesText.text + " lives");
        ResetLevel();
    }

    private void NextLevel()
    {
        //unload current scene
        //load next scene
        //reset main manager
        StartCoroutine(FollowThroughD(SceneManager.UnloadSceneAsync(LevelBuildIndex)));
        ResetMain();
    }

    private void ResetMain()
    {
        gameManager = FindObjectOfType<GameManager>();
        livesText.text = 3.ToString();
        checkpoint = gameManager.checkpoint;
        deathWallCheckpoint = gameManager.deathWallCheckpoint;
    }

    private void FullReset()
    {
        livesText.text = 3.ToString();
        gameManager.lives = 3;

        //checkpoints
        checkpoint = gameManager.originalCheckpoint;
        gameManager.checkpoint = checkpoint;
        deathWallCheckpoint = gameManager.originalDeathWallCheckpoint;
        gameManager.deathWallCheckpoint = deathWallCheckpoint;

        //position
        gameManager.player.transform.position = checkpoint;
        gameManager.deathWall.transform.position = deathWallCheckpoint;

        fullRestartNeeded = false;
    }

    //regular restart
    IEnumerator FollowThroughA(AsyncOperation op)
    {
        while (!op.isDone)
        {
            yield return new WaitForEndOfFrame();
        }
        yield return new WaitForSeconds(1.5f);
        StartCoroutine(FollowThroughB(SceneManager.LoadSceneAsync(LevelBuildIndex, LoadSceneMode.Additive)));
    }

    IEnumerator FollowThroughB(AsyncOperation op)
    {
        while (!op.isDone)
        {
            yield return new WaitForEndOfFrame();
        }
        transition.SetActive(false);
        gameManager = FindObjectOfType<GameManager>();
        if (fullRestartNeeded)
        {
            FullReset();
            finalTransition.SetActive(false);
        }
        if (gameManager.player.won)
        {
            FullReset();
            winTransition.SetActive(false);
        }
        foreach (PlayerController player in FindObjectsOfType<PlayerController>())
        {

        }
    }

    //full restart
    IEnumerator FollowThroughC(AsyncOperation op)
    {
        while (!op.isDone && Input.GetAxis("Reset")<0)
        {
            yield return new WaitForEndOfFrame();
        }
        yield return new WaitForSeconds(1.5f);
        StartCoroutine(FollowThroughB(SceneManager.LoadSceneAsync(LevelBuildIndex, LoadSceneMode.Additive)));
    }

    //win restart
    IEnumerator FollowThroughD(AsyncOperation op)
    {
        while (!op.isDone && Input.GetAxis("Reset") < 0)
        {
            yield return new WaitForEndOfFrame();
        }
        yield return new WaitForSeconds(1.5f);
        LevelBuildIndex += 1;
        StartCoroutine(FollowThroughB(SceneManager.LoadSceneAsync(LevelBuildIndex, LoadSceneMode.Additive)));
    }
}
