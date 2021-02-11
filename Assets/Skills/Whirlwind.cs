using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

class Whirlwind : Skill
{
    private float duration;
    private float range;
    List<int> r2dl;
    private float rangeOffset = 0.1f;
    private float damage;
    private float force;
    private bool spinning;
    public Vector3 playerCenterOffset = new Vector3(0, 0.3f, 0);

    private GameObject spinEffect;
    private void Start()
    {
    }
    public override void Equipped()
    {
        skillIcon = Resources.Load<Sprite>("whirlwind");
        spinEffect = Resources.Load<GameObject>("WhirlwindObj");
        skillName = "Whirlwind";
        blocksMovement = false;
        blocksAttack = true;
        cooldowntimer = 1f;
        duration = 8f;
        rangeOffset = 0.5f;
        range = 1.4f;
        damage = 10f;
        force = 300;
        r2dl = new List<int>();
        spinning = false;
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
        direction.Normalize();
        StartCoroutine(startSpin());
        int increment = 5;
        int a = 0;
        var spinner = Instantiate(spinEffect,playerrb2.transform);
        spinner.transform.Translate(playerCenterOffset);
        spinner.transform.Rotate(Vector3.forward, Vector2.SignedAngle(Vector2.up, direction));
        while (spinning)
        {
            Vector2 playerCenterPos = playerrb2.position + (Vector2)playerCenterOffset;
            spinner.transform.Rotate(Vector3.forward, increment);
            Vector2 dirVectorRange = (Vector2)(Quaternion.Euler(0, 0, a) * direction * range);
            Debug.DrawRay(playerCenterPos + dirVectorRange * rangeOffset, dirVectorRange, Color.white, 0.05f);
            RaycastHit2D rch = Physics2D.Raycast(playerCenterPos + dirVectorRange * rangeOffset, dirVectorRange, range, LayerMask.GetMask("Hittable"));
            if (rch && !r2dl.Contains(rch.transform.GetInstanceID()))
            {
                StartCoroutine(enlistEnemy(rch.transform.GetInstanceID()));
                rch.transform.GetComponent<Enemy>().ApplyDamage(damage);
                rch.transform.GetComponent<Rigidbody2D>().AddForce(playerCenterPos + dirVectorRange * force); // ХУЕТА
            }
            a = a + increment;
            yield return new WaitForSeconds(0.01f);
        }
        movementEnded = true;
        Destroy(spinner);
        yield return null;
    }

    private IEnumerator enlistEnemy(int id)
    {
        r2dl.Add(id);
        yield return new WaitForSeconds(0.5f);
        r2dl.Remove(id);
    }

    private IEnumerator startSpin()
    {
        spinning = true;
        yield return new WaitForSeconds(duration);
        spinning = false;
    }

    public void OnDestroy()
    {
        spinning = false;
        Debug.Log("Destroyed ww");
        StopAllCoroutines();
    }
}
