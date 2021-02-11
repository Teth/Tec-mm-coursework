using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SaveFile
{
    public StatsStruct stats;

    public PlayerClass.PlayerClassEnum playerClass;

    public int xp;
    public int lvl;

    public System.Type currentSkill;

    public WeaponClass.WeaponClassEnum currentWeapon;

    public float x_WaypointLocation;
    public float y_WaypointLocation;

    public string levelName;
}
