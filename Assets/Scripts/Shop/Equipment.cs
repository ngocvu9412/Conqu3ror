using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct Equipment 
{
    public int IDs;
    public Sprite image;
    public string name;
    public string description;
    public int price;
    public bool isPurchased;
    public bool Level;
    public void SetEquipmentID(int id)
    {
        IDs=id;
    }
}
