using System.Collections;
using System.Collections.Generic;
using UnityEngine;

    
public class Character : MonoBehaviour
{
    public string _name;
    public int _life;
    public Prof _prof;
    public Color col;

    public void SetBuilder(BuilderCharacter c)
    {
        _life = c.GetLife();
        _name = c.GetName();
        _prof = c.GetProf();
        GetComponent<Renderer>().material.color = c.GetColor();
        col = c.GetColor();
    }
}

public enum Prof
{
    MAGE,
    WARRIOR,
    PROGRAMMER
}
