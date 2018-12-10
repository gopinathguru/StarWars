using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Level : MonoBehaviour {

    public void LoadStartMenu()
    {
        SceneManager.LoadScene(0);
    }

    public void LoadGame()
    {
        SceneManager.LoadScene("StarWars");  
        FindObjectOfType<GameStatus>().ResetGame();

    }
    public void LoadEndGame()
    {
        StartCoroutine(DelayEndGame());
    }
    public IEnumerator DelayEndGame()
    {
        yield return new WaitForSeconds(2f);
        SceneManager.LoadScene(2);

    }

    public void Quitame()
    {
        Application.Quit();
    }
}
