using UnityEngine;
using System.Collections;

public class HowToPlayController : MonoBehaviour {

    public void Back()
    {
        Application.LoadLevel("MainMenu");
    }
}
