using UnityEngine;
using System.Collections;

public class DotController : MonoBehaviour
{
    public Sprite NormalSprite;
    public Sprite SelectedSprite;

    public DotController Up;
    public DotController Down;
    public DotController Left;
    public DotController Right;

    public GameObject LineUp;
    public GameObject LineDown;
    public GameObject LineLeft;
    public GameObject LineRight;

    public bool JoinedUp;
    public bool JoinedDown;
    public bool JoinedLeft;
    public bool JoinedRight;

    private SpriteRenderer _spriteRenderer;

    public void Start()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void SetSelect(bool selected)
    {
        _spriteRenderer.sprite = selected ? SelectedSprite : NormalSprite;
    }


    public void OnMouseUpAsButton()
    {
        if (GameController.Instance.FirstSelectedDot == null)
        {
            SetSelect(true);
            GameController.Instance.FirstSelectedDot = this;
            return;
        }

        if (Up == GameController.Instance.FirstSelectedDot && GameController.Instance.FirstSelectedDot != null)
        {
            LineUp.renderer.enabled = true;
            JoinedUp = true;
            GameController.Instance.FirstSelectedDot.JoinedDown = true;
            GameController.Instance.DeselectDot();
        }

        if (Down == GameController.Instance.FirstSelectedDot && GameController.Instance.FirstSelectedDot != null)
        {
            LineDown.renderer.enabled = true;
            JoinedDown = true;
            GameController.Instance.FirstSelectedDot.JoinedUp = true;
            GameController.Instance.DeselectDot();
        }

        if (Left == GameController.Instance.FirstSelectedDot && GameController.Instance.FirstSelectedDot != null)
        {
            LineLeft.renderer.enabled = true;
            JoinedLeft = true;
            GameController.Instance.FirstSelectedDot.JoinedRight = true;
            GameController.Instance.DeselectDot();
        }

        if (Right == GameController.Instance.FirstSelectedDot && GameController.Instance.FirstSelectedDot != null)
        {
            LineRight.renderer.enabled = true;
            JoinedRight = true;
            GameController.Instance.FirstSelectedDot.JoinedLeft = true;
            GameController.Instance.DeselectDot();
        }

        GameController.Instance.CheckBoxes();
    }
}
