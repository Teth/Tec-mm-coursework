using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

class Sleep : Skill
{
    private float sleepRadius;
    private float sleepDuration;
    private float channelDuration;
    private bool channeled;

    private void Start()
    {
        
    }
    public override void Equipped()
    {
        skillIcon = Resources.Load<Sprite>("sleep");
        skillName = "Sleep";

        blocksMovement = true;
        blocksAttack = true;
        cooldowntimer = 45f;
        sleepDuration = 15f;
        channelDuration = 4f;
        sleepRadius = 10f;
    }

    public override IEnumerator skillCoroutine(Vector2 playerPos, Vector2 direction)
    {
        skillBar.color = activeBar;
        skillBar.fillAmount = 1;
        //channel for a long time 
        bool holding = true;
        IEnumerator cd = channelRoutine(channelDuration);
        StartCoroutine(cd);
        while (holding)
        {
            holding = Input.GetKey(skillKeyCode);
            if (channeled)
                holding = false;
            yield return null;
        }
        if(channeled)
        {
            Collider2D[] c2darr = Physics2D.OverlapCircleAll(playerPos, sleepRadius, LayerMask.GetMask("Hittable"));
            foreach (var c2d in c2darr)
            {
                Enemy enemy = c2d.GetComponent<Enemy>();
                if (enemy && !enemy.sleeping)
                {
                    enemy.SetSleeping(sleepDuration);
                }
            }
            skillBar.color = normal;
            skillEnded = true;
        }
        skillBar.color = normal;
        skillEnded = true;

    }

    public override IEnumerator skillMovementStart(Rigidbody2D playerrb2, Vector2 direction)
    {
        skillEnded = false;
        movementEnded = true;
        yield return null;
    }


    private IEnumerator channelRoutine(float seconds)
    {
        channeled = false;
        yield return new WaitForSeconds(seconds);
        channeled = true;
    }
}
