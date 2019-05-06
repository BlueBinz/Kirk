using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainManager : MonoBehaviour
{
    public int LevelBuildIndex = 2;
    public float timeToReset = 0;
    public GameObject transition;
    public GameObject finalTransition;

    int lives;

    public Text livesText;
    
    private void Awake()
    {
        SceneManager.LoadSceneAsync(LevelBuildIndex, LoadSceneMode.Additive);
        livesText.text = 3.ToString();
    }

    public void ResetLevel()
    {
        if (timeToReset <= 0 && livesText.text != 0.ToString())
        {
            transition.SetActive(true);
            StartCoroutine(FollowThroughA(SceneManager.UnloadSceneAsync(LevelBuildIndex)));
            timeToReset = 1f;
        }
        else if(livesText.text == 0.ToString())
        {
            finalTransition.SetActive(true);
        }
    }

    private void Update()
    {
        if (Input.GetAxis("Reset") > 0)
            ResetLevel();
        if (timeToReset > 0)
            timeToReset -= Time.deltaTime;
    }

    public void Die()
    {
        livesText.text = (--lives).ToString();
        ResetLevel();
    }

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
        foreach (PlayerController player in FindObjectsOfType<PlayerController>())
        {

        }
    }
}
