using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class WeaponClass
    {
    public enum WeaponClassEnum
    {
        Sword,
        Staff,
        Bow,
        None
    }

    public static GameObject getWeaponPrefab(WeaponClassEnum cl)
    {
        switch (cl)
        {
            case WeaponClassEnum.Sword:
                return Resources.Load<GameObject>("SwordObject");
            case WeaponClassEnum.Staff:
                return Resources.Load<GameObject>("Staff");
            case WeaponClassEnum.Bow:
                return Resources.Load<GameObject>("Bow");
            default:
                return null;
        }
    }

    public static WeaponClassEnum getWeaponClassFromWeapon(Weapon w)
    {
        if (!w)
        {
            return WeaponClassEnum.None;
        }
        if (w.GetType() == typeof(Sword))
        {
            return WeaponClassEnum.Sword;
        }
        else if (w.GetType() == typeof(Staff))
        {
            return WeaponClassEnum.Staff;
        }
        else if (w.GetType() == typeof(Bow))
        {
            return WeaponClassEnum.Bow;
        }
        else
        {
            Debug.LogError("Error");
            return WeaponClassEnum.None;
            }
    }
}

