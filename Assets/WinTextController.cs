using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class WinTextController : MonoBehaviour
{

    private Text _text;
    
    void Start ()
    {
        _text = GetComponent<Text>();

        _text.text = GameDataController.Instance.WinningPlayer != 0 ? string.Format("Player {0} Wins!", GameDataController.Instance.WinningPlayer) : "Game was a draw!";
    }

    public void StartNewGame()
    {
        Application.LoadLevel("GameScreen");
    }
}
