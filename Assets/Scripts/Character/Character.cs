using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct Character
{
    public Sprite image;
    public string name;
    public int health;
    public int attack;

    public int Exp;
    public int Level;
    public bool isUsing;
}
