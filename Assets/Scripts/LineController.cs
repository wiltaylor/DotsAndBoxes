using UnityEngine;
using System.Collections;

public class LineController : MonoBehaviour
{

    public Sprite Normal;
    public Sprite Player1;
    public Sprite Player2;

    private SpriteRenderer _spriteRenderer;

    public void Start()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }


    public void SetPlayerOwner(int Id)
    {
        switch (Id)
        {
            case 0:
                _spriteRenderer.sprite = Normal;
                break;
            case 1:
                _spriteRenderer.sprite = Player1;
                break;
            case 2:
                _spriteRenderer.sprite = Player2;
                break;
        }
    }
}
