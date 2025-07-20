using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UIElements;

public class Push : MovingObject
{
    public int sequenceID = 0;

    [field:SerializeField]private bool Push1Box = false;
    public int ID;

    public TextMeshPro text;
    [SerializeField] private string moveSFX;


    public void SetText(char c)
    {
        if (text != null)
        {
            text.text = c.ToString();
        }
    }

    protected override void PlayMoveSFX()
    {
        AudioManager.Instance.PlaySFX(moveSFX);
    }

    public bool Move(Vector2 direction)
    {

        if (Blocked(transform.position, direction, Push1Box))//1 push 1 box true/false
        {
            return false;
        }
        else
        {
            MovementAnimation(direction);

            return true;
        }
    }
}
