using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

public class Staff : Weapon
{
    private float initialProjectileSpeed = 5;
    public GameObject magicPrefab;
    public GameObject staffModel;

    private GameObject floatingObject;

    public Sprite staff;
    public Sprite staffCharge;

    public SpriteRenderer weaponRenderer;

    private Player playerData;
    private float charge;
    private Vector2 instantiatePosition;
    private Vector2 staffPosition;

    private float orderMult = 100;

    private void FixedUpdate()
    {
        if (sliderCharge && sliderCd)
        {
            sliderCd.transform.position = Camera.main.WorldToScreenPoint(transform.parent.position + OffsetSlider);
            sliderCharge.transform.position = Camera.main.WorldToScreenPoint(transform.parent.position + OffsetSlider2);
        }
    }

    public override void Equipped(StatsStruct stats)
    {
        base.Equipped(stats);
        playerData = GetComponentInParent<Player>();
        cooldowntimer = CalculateCooldown(stats);
        weaponDamage = CalculateDamage(stats);
        chargeTime = cooldowntimer / 3;

        if(floatingObject == null)
        {
            floatingObject = Instantiate(staffModel, transform);
            weaponRenderer = floatingObject.GetComponent<SpriteRenderer>();
        }
    }

    public void Update()
    {
        if (playerData)
        {
            staffPosition = playerData.transform.position + playerData.playerCenterOffset + (Vector3)((Vector2)playerData.LookDirection).normalized * 0.35f;
            floatingObject.transform.position = staffPosition;
            weaponRenderer.sortingOrder = playerData.playerRenderer.sortingOrder - (playerData.LookDirection.y <= 0 ? -1 : 1);
            instantiatePosition = playerData.transform.position + playerData.playerCenterOffset + playerData.LookDirection.normalized * 2f;
        }
    }

   
    // Damage applied here
    public override IEnumerator attackCoroutine(Vector2 playerPos, Vector2 direction)
    {
        released = false;
        //show projectile

        //spawn projectile
        GameObject arrowClone = Instantiate(magicPrefab,
            instantiatePosition, Quaternion.Euler(0, 0, Vector2.SignedAngle(new Vector2(1, 0), playerData.LookDirection)));
        Debug.DrawRay(instantiatePosition, playerData.LookDirection, Color.red, 3f);
        arrowClone.GetComponent<MagicProjectile>().baseProjectileDamage = weaponDamage;
        arrowClone.GetComponent<Rigidbody2D>().velocity = (Vector2.Perpendicular(playerData.LookDirection.normalized) * initialProjectileSpeed);
        released = true;
        yield return null;
        //Debug.Log("Shot");
    }


    

    public override IEnumerator AWSMStart(Rigidbody2D playerrb2, Vector2 direction)
    {
        yield return new WaitForEndOfFrame();
    }

    public override IEnumerator AWSMFinished(Rigidbody2D playerrb2, Vector2 direction)
    {
        yield return new WaitForEndOfFrame();
    }

    public override IEnumerator startCharge()
    {
        charged = false;
        released = false;
        weaponRenderer.sprite = staffCharge;
        for (float t = 0; t < cooldowntimer; t = t + Time.deltaTime)
        {
            sliderCharge.value = t / cooldowntimer;


            if (!Input.GetMouseButton(0) && !Input.GetMouseButtonDown(0))
            {
                //Debug.Log("Nedocharged");
                charge = t;
                //Debug.Log(charge);
                charged = true;
                released = true;
                weaponRenderer.sprite = staff;

                yield break;
            }
            yield return null;
        }
        charged = true;
        weaponRenderer.sprite = staff;

    }

    public override IEnumerator startRelease()
    {
        for (float t = charge; t > 0; t = t - (Time.deltaTime * 2.5f))
        {
            sliderCharge.value = t / cooldowntimer;
            yield return null;
        }
        sliderCharge.value = 0;
        yield return null;
        released = true;
        //Debug.Log("Discharged");
    }
}
