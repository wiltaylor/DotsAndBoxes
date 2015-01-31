using UnityEngine;
using System.Collections;

public class GameDataController : MonoBehaviour
{

    public static GameDataController Instance;

    public int WinningPlayer = 0;
	
	void Awake () 
    {
	    if (Instance != null)
	    {
	        Destroy(gameObject);
	        return;
	    }

	    Instance = this;
        DontDestroyOnLoad(gameObject);
    }
	
}
