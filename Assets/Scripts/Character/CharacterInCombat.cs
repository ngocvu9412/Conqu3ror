using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class CharacterInCombat
{
    public int MaxHealth;
    public int CurrentHealth;
    public int BaseAttack;
    public int CurrentAttack;
    public int MaxEnergy = 300;
    public int CurrentEnergy;
    public float MaxTime = 90;
    public float CurrentTime;
    public int CharacterIndex = 0;
    public int Gold = 0;
    public int Experience = 0;

    public List<Skill> Skills; // Danh sách kỹ năng của nhân vật
}