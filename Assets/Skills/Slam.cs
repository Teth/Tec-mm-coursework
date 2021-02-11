using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

class Slam : Skill
{
    private float slamDistance = 3;
    private float slamTime = 0.4f;
    private float slamRadius = 1.5f;
    private float slamDamage = 30;
    private float slamKnockbackForce = 300;

    GameObject slamPrefab;

    GameObject instSlam;
    public override void Equipped()
    {
        skillName = "Slam";
        skillIcon = Resources.Load<Sprite>("slam");
        slamPrefab = Resources.Load<GameObject>("SlamEffectPrefab");
        blocksMovement = true;
        blocksAttack = true;
        cooldowntimer = 3f;
    }

    public override IEnumerator skillCoroutine(Vector2 playerPos, Vector2 direction)
    {
        Debug.Log("Yes3");
        StartCoroutine(animPlay());
        Collider2D[] col2d = Physics2D.OverlapCircleAll(playerPos, slamRadius, LayerMask.GetMask("Hittable"));
        foreach(Collider2D coll in col2d)
        {
            coll.GetComponent<Enemy>().ApplyDamage(slamDamage);
            coll.GetComponent<Rigidbody2D>().AddForce(((Vector2)coll.transform.position - playerPos).normalized * slamKnockbackForce);       
        }
        skillEnded = true;
        skillBar.color = normal;
        yield return null;
    }

    public IEnumerator animPlay()
    {
        yield return new WaitForSeconds(0.2f);
        Destroy(instSlam);
    }

    public override IEnumerator skillMovementStart(Rigidbody2D playerrb2, Vector2 direction)
    {
        skillBar.color = activeBar;
        skillBar.fillAmount = 1;
        skillEnded = false;
        movementEnded = false;
        var p1 = playerrb2.position;
        playerrb2.velocity = direction.normalized * slamDistance / slamTime;
        yield return new WaitForSeconds(slamTime);
        var p2 = playerrb2.position;
        Debug.Log((p2 - p1).magnitude);
        playerrb2.velocity = Vector2.zero;
        instSlam = Instantiate(slamPrefab);
        instSlam.transform.position = playerrb2.position;
        instSlam.GetComponent<Animator>().Play("SlamAnimation");
        movementEnded = true;
    }
}