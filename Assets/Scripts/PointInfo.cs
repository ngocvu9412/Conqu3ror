using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PointInfo
{
    public string PointName;
    public bool status; //red or green (true: win, false: not yet)

    //Enemy
    public Sprite Image;
    public string EnemyName;
    public int EnemyHealth;
    public int EnemyAttack;
    public int EnemyLevel;
    public int skillIndex;
    public string AIDifficulty;
    public List<Skill> Skills; // Danh sách kỹ năng của kẻ địch

    public List<(Sprite, string, string)> conversationBefore; //ảnh, tên nhân vật, hội thoại
    public List<(Sprite, string, string, bool)> conversationAfter; //ảnh, tên nhân vật, hội thoại, win/lose
    public void SetStatus(bool S)
    {
        status = S;
    }
}
