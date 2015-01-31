using UnityEngine;
using System.Collections;

public class BoxController : MonoBehaviour
{
    public Sprite Player1;
    public Sprite Player2;

    private SpriteRenderer _render;

    void Start()
    {
        _render = GetComponent<SpriteRenderer>();
    }

    public void SetPlayer(int Player)
    {
        if (Player == 1)
        {
            _render.sprite = Player1;
            _render.enabled = true;
        }

        if (Player == 2)
        {
            _render.sprite = Player2;
            _render.enabled = true;
        }
    }

}
