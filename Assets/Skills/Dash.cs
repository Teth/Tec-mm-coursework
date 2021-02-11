using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

class Dash : Skill
{
    
    private float dashDistance = 6;
    private float dashTime = 0.15f;


    public override void Equipped()
    {
        skillIcon = Resources.Load<Sprite>("dash");
        skillName = "Dash";
        description = "Quick movement skill in the direction of sight";
        blocksAttack = false;
        blocksMovement = true;
        cooldowntimer = 6f;
    }

    public override IEnumerator skillCoroutine(Vector2 playerPos, Vector2 direction)
    {
        skillEnded = true;
        yield return null;
        skillBar.color = normal;

    }

    public override IEnumerator skillMovementStart(Rigidbody2D playerrb2, Vector2 direction)
    {
        skillBar.color = activeBar;
        skillBar.fillAmount = 1;
        skillEnded = false;
        movementEnded = false;
        playerrb2.velocity = direction.normalized * dashDistance / dashTime;
        yield return new WaitForSeconds(dashTime);
        playerrb2.velocity = Vector2.zero;
        movementEnded = true;
    }
}