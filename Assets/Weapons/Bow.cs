using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

public class Bow : Weapon
{
    private float force = 300;
    private float projectileSpeed = 30;
    public GameObject ArrowPrefab;


    public GameObject bowModel;

    private GameObject floatingObject;

    public Sprite bow;
    public Sprite bowCharging;
    public Sprite bowReady;

    public SpriteRenderer weaponRenderer;

    private Player playerData;
    private bool attackFinished;
    private float charge;
    private Vector2 instantiatePosition;
    private Vector2 weaponPosition;

    private SpriteRenderer fakeProjectileRenderer;
    private GameObject arrowInTheBow;

    public override void Equipped(StatsStruct stats)
    {
        base.Equipped(stats);

        playerData = GetComponentInParent<Player>();
        arrowInTheBow = Instantiate(ArrowPrefab, transform);
        fakeProjectileRenderer = arrowInTheBow.GetComponentInChildren<SpriteRenderer>();
        Destroy(arrowInTheBow.GetComponent<Collider2D>());
        fakeProjectileRenderer.enabled = false;
        blocksMovement = false;
        cooldowntimer = CalculateCooldown(stats);
        weaponDamage = CalculateDamage(stats);
        chargeTime = cooldowntimer / 3;

        if (floatingObject == null)
        {
            floatingObject = Instantiate(bowModel, transform);
            weaponRenderer = floatingObject.GetComponentInChildren<SpriteRenderer>();
        }
    }

    public void FixedUpdate()
    {
        

        if (sliderCharge && sliderCd)
        {
            sliderCd.transform.position = Camera.main.WorldToScreenPoint(transform.parent.position + OffsetSlider);
            sliderCharge.transform.position = Camera.main.WorldToScreenPoint(transform.parent.position + OffsetSlider2);
        }
    }

    private void Update()
    {
        if (playerData)
        {
            instantiatePosition = playerData.transform.position + playerData.playerCenterOffset + (Vector3)((Vector2)playerData.LookDirection).normalized * 0.4f;
            arrowInTheBow.transform.position = instantiatePosition;
            arrowInTheBow.transform.rotation = Quaternion.Euler(0, 0, Vector2.SignedAngle(new Vector2(1, 0), playerData.LookDirection));

            weaponPosition = playerData.transform.position + playerData.playerCenterOffset + (Vector3)((Vector2)playerData.LookDirection).normalized * 0.5f;
            floatingObject.transform.position = weaponPosition;
            floatingObject.transform.rotation = Quaternion.Euler(0, 0, Vector2.SignedAngle(new Vector2(1, 0), playerData.LookDirection));
        }


    }

    // Damage applied here
    public override IEnumerator attackCoroutine(Vector2 playerPos, Vector2 direction)
    {
        released = false;
        //show projectile
        fakeProjectileRenderer.enabled = true;
        yield return new WaitUntil(() => Input.GetMouseButtonUp(0));
        weaponRenderer.sprite = bow;

        fakeProjectileRenderer.enabled = false;

        //spawn projectile
        GameObject arrowClone = Instantiate(ArrowPrefab, 
            instantiatePosition, Quaternion.Euler(0, 0, Vector2.SignedAngle(new Vector2(1,0), playerData.LookDirection)));
        Debug.DrawRay(instantiatePosition, playerData.LookDirection, Color.red, 3f);
        arrowClone.GetComponent<BowProjectile>().projectileDamage = weaponDamage;
        arrowClone.GetComponent<Rigidbody2D>().velocity = (((Vector2)playerData.LookDirection).normalized * projectileSpeed);
        released = true;
        //Debug.Log("Shot");
        attackFinished = true;
    }

    public override IEnumerator AWSMStart(Rigidbody2D playerrb2, Vector2 direction)
    {
        var player = playerrb2.GetComponent<Player>();
        player.speed = player.speed / 6;
        yield return null;
    }

    public override IEnumerator AWSMFinished(Rigidbody2D playerrb2, Vector2 direction)
    {
        blocksMovement = true;

        var player = playerrb2.GetComponent<Player>();
        player.speed = player.speed * 6;


        if (attackFinished)
            playerrb2.AddForce(-direction.normalized * force);
        //Debug.Log(-direction * force);

        yield return new WaitForSeconds(0.15f); // wait smth
        blocksMovement = false;

    }

    public override IEnumerator startCharge()
    {
        attackFinished = false;
        charged = false;
        released = false;
        weaponRenderer.sprite = bowCharging;
        for (float t = 0; t < chargeTime; t = t + Time.deltaTime)
        {
            sliderCharge.value = t / chargeTime;

            if (!Input.GetMouseButton(0) && !Input.GetMouseButtonDown(0))
            {
                //Debug.Log("Nedocharged");
                charge = t;
                //Debug.Log(charge);
                charged = true;
                released = true;
                weaponRenderer.sprite = bow;

                yield break;
            }
            yield return null;
        }
        charge = cooldowntimer;

        charged = true;
        weaponRenderer.sprite = bowReady;

    }

    public override IEnumerator startRelease()
    {
        for (float t = charge; t > 0; t = t - (Time.deltaTime*2.5f))
        {
            sliderCharge.value = t / chargeTime;
            yield return null;
        }
        sliderCharge.value = 0;
        yield return null;
        released = true;
        //Debug.Log("Discharged");
    }
}
