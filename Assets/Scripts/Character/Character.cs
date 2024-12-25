using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Character
{
    public Sprite Image;
    public string name;
    public int health;

    public int attack;


    public int Exp;
    public int MaxExp;

    public int Level;

    public bool Unlocked;
    public int Price;

    public List<Skill> Skills; // Danh sách kỹ năng của nhân vật

}
