using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public abstract class Skill : MonoBehaviour
{
    public bool canUseSkill = true;
    public string skillName;
    public float cooldowntimer;
    public Sprite skillIcon;
    public string description;
    public KeyCode skillKeyCode;
    public Color activeBar = new Color(0, 255, 0, 0.3f);
    public Color cooldownBar = new Color(255, 0, 0, 0.7f);
    public Color normal = new Color(255, 255, 255, 0f);

    public Image skillBar;

    public bool skillEnded = true;
    public bool movementEnded = true;
    public bool blocksAttack;
    public bool blocksMovement;
    public bool blocksSkill;
    public abstract void Equipped();
    public abstract IEnumerator skillCoroutine(Vector2 playerPos, Vector2 direction);
    //apply weapon specific movement as soon as attack started
    public abstract IEnumerator skillMovementStart(Rigidbody2D playerrb2, Vector2 direction);
}
