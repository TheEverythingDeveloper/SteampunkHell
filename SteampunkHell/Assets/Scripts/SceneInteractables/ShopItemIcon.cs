using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopItemIcon : MonoBehaviour
{
    public int actualState;
    private Animator _anim;
    private Image _myImg;

    private void Awake()
    {
        _anim = GetComponent<Animator>();
        _myImg = GetComponent<Image>();
    }

    public void ChangeSprite(Sprite newSprite)
    {
        _myImg.sprite = newSprite;
    }

    private void Start()
    {
        _anim.SetInteger("ActualState", actualState);
        _anim.SetFloat("Left", 1f);
        _anim.SetTrigger("Activate");
    }

    public void Move(bool right)
    {
        if (right)
            MoveRight();
        else
            MoveLeft();
    }

    private void MoveRight()
    {
        actualState++;
        if (actualState > 4) actualState = 0;
        _anim.SetInteger("ActualState", actualState);
        _anim.SetFloat("Left", -0.1f);
        _anim.SetTrigger("Activate");
    }

    private void MoveLeft()
    {
        actualState--;
        if (actualState < 0) actualState = 4;
        _anim.SetInteger("ActualState", actualState);
        _anim.SetFloat("Left", 4f);
        _anim.SetTrigger("Activate");
    }
}