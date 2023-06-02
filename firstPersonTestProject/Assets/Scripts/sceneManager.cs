using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class sceneManager : MonoBehaviour
{   
    private int thisScene;

    void Start()
    {
        thisScene = SceneManager.GetActiveScene().buildIndex;
    }
    public void ReloadScene()
    {   
        SceneManager.LoadScene(thisScene);
        Time.timeScale = 1;
    }

    public void NextScene()
    {   
        SceneManager.LoadScene(thisScene + 1);
        Time.timeScale = 1;
    }

    public void PreviousScene()
    {   
        SceneManager.LoadScene(thisScene - 1);
        Time.timeScale = 1;
    }
    public void SceneMainMenu() 
    {  
        SceneManager.LoadScene("MainMenu");
        Time.timeScale = 1;
    }

    public void CustomScene(int sceneIndex) 
    {   
        SceneManager.LoadScene(sceneIndex);
        Time.timeScale = 1;
    }
     public void ExitGame()
    {  
        Debug.Log("exitgame");  
        Application.Quit();  
    }
}
