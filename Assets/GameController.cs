using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    public DotController FirstSelectedDot;
    public static GameController Instance;
    public Text CurrentPlayerText;

    public int Rows = 9;
    public int Cols = 9;
    public float dotOffSetX = 0.38f;
    public float dotOffSetY = 0.38f;
    public float lineOffSetX = 0.19f;
    public float lineOffSetY = 0.19f;
    public float WorldStartX = 0f;
    public float WorldStartY = 0f;
    public int CurrentPlayer = 1;

    public GameObject DotPrefab;
    public GameObject HorizontalLinePrefab;
    public GameObject VerticalLinePrefab;

    private DotController[,] DotArray;
    private GameObject[,] HorizontalLines;
    private GameObject[,] VerticalLines;
    private int[,] WonBoxes;

    public void Start()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        WonBoxes = new int[Rows - 1,Cols - 1];
        DotArray = new DotController[Rows, Cols];
        HorizontalLines = new GameObject[Rows, Cols];
        VerticalLines = new GameObject[Rows, Cols];

        for (var x = 0; x < Cols; x++)
        {
            for (var y = 0; y < Rows; y++)
            {
                DotArray[x, y] = ((GameObject)Instantiate(DotPrefab)).GetComponent<DotController>();
                DotArray[x, y].transform.position = new Vector3(WorldStartX + x * dotOffSetX, WorldStartY + y * dotOffSetY);
                DotArray[x, y].transform.parent = transform;
                DotArray[x, y].gameObject.name += string.Format("[{0}/{1}]", x, y);

                if (x != Cols - 1)
                {
                    HorizontalLines[x, y] = (GameObject)Instantiate(HorizontalLinePrefab);
                    HorizontalLines[x, y].transform.position = new Vector3(WorldStartX + x * dotOffSetX + lineOffSetX, WorldStartY + y * dotOffSetY);
                    HorizontalLines[x, y].transform.parent = transform;
                    HorizontalLines[x, y].gameObject.name += string.Format("[{0}/{1}]", x, y);
                }

                if (y != Rows - 1)
                {
                    VerticalLines[x, y] = (GameObject)Instantiate(VerticalLinePrefab);
                    VerticalLines[x, y].transform.position = new Vector3(WorldStartX + x * dotOffSetX, WorldStartY + y * dotOffSetY + lineOffSetY);
                    VerticalLines[x, y].transform.parent = transform;
                    VerticalLines[x, y].gameObject.name += string.Format("[{0}/{1}]", x, y);
                }

            }
        }

        for (var x = 0; x < Cols; x++)
        {
            for (var y = 0; y < Rows; y++)
            {
                if (x != 0)
                {
                    DotArray[x, y].Left = DotArray[x - 1, y];
                    DotArray[x, y].LineLeft = HorizontalLines[x - 1, y];
                }

                if (x != Cols - 1)
                {
                    DotArray[x, y].Right = DotArray[x + 1, y];
                    DotArray[x, y].LineRight = HorizontalLines[x, y];
                }

                if (y != 0)
                {
                    DotArray[x, y].Down = DotArray[x, y - 1];
                    DotArray[x, y].LineDown = VerticalLines[x, y - 1];
                }

                if (y != Rows - 1)
                {
                    DotArray[x, y].Up = DotArray[x, y + 1];
                    DotArray[x, y].LineUp = VerticalLines[x, y];
                }
            }
        }

    }

    public void DeselectDot()
    {
        if (FirstSelectedDot != null)
        {
            FirstSelectedDot.SetSelect(false);
            FirstSelectedDot = null;
        }
    }

    public void CheckBoxes()
    {
        var freeSpaces = false;
        var NewBox = false;
        var playerScores = new int[3];

        for (var x = 0; x < Cols - 1; x++)
        {
            for (var y = 0; y < Rows - 1; y++)
            {
                if (WonBoxes[x, y] != 0)
                {
                    playerScores[WonBoxes[x, y]]++;
                    continue;
                }

                if (DotArray[x, y].JoinedUp && DotArray[x, y].JoinedRight)
                {
                    if (DotArray[x + 1, y + 1].JoinedDown && DotArray[x + 1, y + 1].JoinedLeft)
                    {
                        WonBoxes[x, y] = CurrentPlayer;
                        HorizontalLines[x,y].GetComponent<LineController>().SetPlayerOwner(CurrentPlayer);
                        HorizontalLines[x,y + 1].GetComponent<LineController>().SetPlayerOwner(CurrentPlayer);
                        VerticalLines[x, y].GetComponent<LineController>().SetPlayerOwner(CurrentPlayer);
                        VerticalLines[x + 1, y].GetComponent<LineController>().SetPlayerOwner(CurrentPlayer);
                        playerScores[CurrentPlayer]++;
                        NewBox = true;
                    }
                }
                else
                {
                    freeSpaces = true;
                }
            }
        }

        if (!NewBox)
        {
            CurrentPlayer = CurrentPlayer == 1 ? 2 : 1;

            CurrentPlayerText.text = string.Format("Player {0}'s Turn!", CurrentPlayer);
        }

        if (!freeSpaces)
        {
            if (playerScores[1] > playerScores[2])
            {
                GameDataController.Instance.WinningPlayer = 1;
            }

            if (playerScores[2] > playerScores[1])
            {
                GameDataController.Instance.WinningPlayer = 2;
            }

            if (playerScores[1] == playerScores[2])
            {
                GameDataController.Instance.WinningPlayer = 0;
            }

            Application.LoadLevel("VictoryScreen");
        }
            
            
    }

}
