using UnityEngine;
using System.Collections;

public class MenuController : MonoBehaviour {

    public void StartGame()
    {
        Application.LoadLevel("GameScreen");   
    }

    public void HowToPlay()
    {
        Application.LoadLevel("HowToPlay"); 
    }

    public void Exit()
    {
        Application.Quit();
    }

}
