using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "EquipmentShopDatabase", menuName = "Shopping/Equipments shop database")]
public class EquipmentShopDatabase : ScriptableObject
{
    public Equipment[] equipments;

    public int EquipmentsCount{
        get { return equipments.Length; }
    }

    public Equipment GetEquipment (int index)
    {
        return equipments[index];
    }
}
