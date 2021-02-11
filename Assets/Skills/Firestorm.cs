using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

class Firestorm : Skill
{
    private float duration;
    private float interval;
    private float radius;
    private float stormSpeed;
    private float pulseDamage;
    private float impactDamage;
    private float impactRadiusModifier;
    private int impactForce;

    private bool pulseEnded;

    private SpriteRenderer firestormRenderer;
    GameObject firestormPrefab;

    public override void Equipped()
    {
        skillIcon = Resources.Load<Sprite>("firestorm");
        firestormPrefab = Resources.Load<GameObject>("FirestormPrefab");
        skillName = "Firestorm";
        duration = 4f;
        interval = 1f;
        radius = 1f;
        stormSpeed = 3f;
        blocksMovement = true;
        blocksAttack = true;
        cooldowntimer = 6f;
        impactRadiusModifier = 1.5f;
        impactForce = 700;
        impactDamage = 50f;
        pulseDamage = 8f;
    }

    public override IEnumerator skillCoroutine(Vector2 playerPos, Vector2 direction)
    {
        skillBar.color = activeBar;
        skillBar.fillAmount = 1;
        movementEnded = true;
        var firestorm = Instantiate(firestormPrefab);
        firestorm.transform.position = playerPos + direction * radius * 2f;
        var rb2 = firestorm.AddComponent<Rigidbody2D>();

        rb2.gravityScale = 0;
        var player = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<CameraScript>().target;
        var offset = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<CameraScript>().mouse_offset_value;
        GameObject.FindGameObjectWithTag("MainCamera").GetComponent<CameraScript>().target = firestorm.transform;
        GameObject.FindGameObjectWithTag("MainCamera").GetComponent<CameraScript>().mouse_offset_value = 0;
        bool holding = true;
        //Debug.Log("Started pulse coroutine");
        IEnumerator fsr = FirestormPulse(firestorm);
        StartCoroutine(fsr);
        while (holding)
        {
            Vector2 mp = Input.mousePosition;
            Vector3 curp = Camera.main.ScreenToWorldPoint(mp);
            rb2.velocity = (curp - rb2.transform.position).normalized * stormSpeed;
            holding = Input.GetKey(skillKeyCode);
            if (pulseEnded)
                holding = false;
            yield return null;      
            
        }
        Debug.Log("Released C");
        StopCoroutine(fsr);

        Debug.Log("Ended pulse coroutine");
        firestorm.GetComponent<Animator>().speed = 1f;
        firestorm.GetComponent<Animator>().Play("FirestormFinish");
        Collider2D[] res = Physics2D.OverlapCircleAll(firestorm.transform.position, radius * impactRadiusModifier, LayerMask.GetMask("Hittable"));
        foreach (var coll in res)
        {
            Enemy enemy = coll.GetComponent<Enemy>();
            if (enemy)
            {
                enemy.ApplyDamage(impactDamage);
                enemy.GetComponent<Rigidbody2D>().AddForce((coll.transform.position - firestorm.transform.position).normalized * impactForce);
            }
        }

        yield return new WaitForSeconds(0.25f);
        Destroy(firestorm);
        //return default values to camera
        GameObject.FindGameObjectWithTag("MainCamera").GetComponent<CameraScript>().target = player;
        GameObject.FindGameObjectWithTag("MainCamera").GetComponent<CameraScript>().mouse_offset_value = offset;
        skillEnded = true;
        yield return null;
        skillBar.color = normal;
    }

    public override IEnumerator skillMovementStart(Rigidbody2D playerrb2, Vector2 direction)
    {
        playerrb2.velocity = Vector2.zero;
        skillEnded = false;
        movementEnded = true;
        yield return null;
    }

    private IEnumerator FirestormPulse(GameObject fs)
    {
        fs.GetComponent<Animator>().PlayInFixedTime("FirestormPulse",0, interval);
        fs.GetComponent<Animator>().speed = 0.1f;
        pulseEnded = false;
        int i = 0;
        for (float t = 0; t < duration - 0.01; t += interval)
        {

            Collider2D[] res = Physics2D.OverlapCircleAll(fs.transform.position, radius, LayerMask.GetMask("Hittable"));
            foreach (var coll in res)
            {

                Enemy enemy = coll.GetComponent<Enemy>();
                if (enemy)
                {
                    enemy.ApplyDamage(pulseDamage);
                    enemy.GetComponent<Rigidbody2D>().AddForce((coll.transform.position - fs.transform.position).normalized * impactForce/10);

                }
            }
            yield return new WaitForSeconds(interval);
        }
        fs.GetComponent<Animator>().Play("Empty");
        pulseEnded = true;
    }
}