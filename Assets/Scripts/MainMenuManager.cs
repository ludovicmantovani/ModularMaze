using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    public void GoPlay()
    {
        SceneManager.LoadScene("MazePlay");
    }

    public void GoGallery()
    {
        SceneManager.LoadScene("Gallery");
    }

    public void GoMenu()
    {
        SceneManager.LoadScene("MainMenu");

    }
    public void Quit()
    {
        Application.Quit();
    }


    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            Debug.Log("Quit !");
            Application.Quit();
        }
    }

}
