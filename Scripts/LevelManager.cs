using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    public static bool isGameOver = false; //Check if game is lost
    public Text gameText; //Hidden text that is displayed when game is won or lost

    public float startTimer = 0.00f; //Initialize the time we want the stopwatch to start at
    private float stopWatch; //Stopwatch to time how long player takes

    void Start()
    {
        isGameOver = false;
        stopWatch = startTimer;
        gameText.gameObject.SetActive(false); // Need to uncheck the gameText box in inspector


    }

    void InitializeLevel() // Function that will reset everything when the scene is loaded, for now using the start function
    {

    }

    
    void Update()
    {
        if (!isGameOver) // If the game is not over, execute
        {
            stopWatch += Time.deltaTime;
        }
    }

    public void LevelLost() // Execute if the level is lost
    {
        isGameOver = true;
        gameText.text = "GAME OVER!";
        gameText.gameObject.SetActive(true); //Game text is initially hidden, unhide it when game lost

        //AudioSource.PlayClipAtPoint(gameOverSFX, Camera.main.transform.position); //Audio Clip if we want

        Invoke("LoadCurrentLevel", 2);

    }

    void LoadCurrentLevel() // This function reloads the current scene
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void LevelBeat()
    {
        isGameOver = true;
        gameText.text = "YOU WON!";
        gameText.gameObject.SetActive(true);
        //AudioSource.PlayClipAtPoint(gameWonSFX, Camera.main.transform.position); //Audio Clip if we want

        Invoke("LoadNextLevel", 2);

        //Call Reset Hearts, Timer and Score function
    }

    void LoadNextLevel()
    {
        //Gets the current scene #, and the total scene #. If it is the last scene execute the else statement
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        int totalScenes = SceneManager.sceneCountInBuildSettings;

        if (currentSceneIndex < totalScenes - 1)
        {
            SceneManager.LoadScene(currentSceneIndex + 1);
        }
        else
        {
            gameText.text = "GAME DONE!";
            gameText.gameObject.SetActive(true);
        }
    }
}
