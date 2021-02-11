using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class Sword : Weapon
{
    public float range;
    public float angle;
    public float force;

    private Player playerData;

    public GameObject swordModel;

    private GameObject floatingObject;

    public Sprite sword;
    public Sprite swordSplash;

    public SpriteRenderer weaponRenderer;
    private Vector2 instantiatePosition;
    private int orderMult = 100;


    private void Update()
    {
        if (playerData)
        {
            instantiatePosition = playerData.transform.position + playerData.playerCenterOffset + (Vector3)((Vector2)playerData.LookDirection).normalized * 0.30f;
            floatingObject.transform.position = instantiatePosition;
            weaponRenderer.sortingOrder = playerData.playerRenderer.sortingOrder - (playerData.LookDirection.y <= 0 ? -1 : 1);
            weaponRenderer.transform.localScale = new Vector3(playerData.transform.position.x - instantiatePosition.x < 0 ? 1 : -1, 1, 1);
        }
    }

    private void FixedUpdate()
    {
        

        if (sliderCharge && sliderCd)
        {
            sliderCd.transform.position = Camera.main.WorldToScreenPoint(transform.parent.position + OffsetSlider);
            sliderCharge.transform.position = Camera.main.WorldToScreenPoint(transform.parent.position + OffsetSlider2);
        }
    }
    // Damage applied here
    public override IEnumerator attackCoroutine(Vector2 playerPos, Vector2 direction)
    {
        direction.Normalize();
        released = false;
        List<int> r2dl = new List<int>();
        float startAngle = Random.value < .5f ? angle : -angle;
        int increment = startAngle < 0 ? 5 : -5;
        var rotationInit = weaponRenderer.transform.rotation;

        weaponRenderer.transform.Rotate(Vector3.forward * weaponRenderer.transform.localScale.x * -45);
        weaponRenderer.transform.Rotate(Vector3.forward, Vector2.SignedAngle(new Vector2(weaponRenderer.transform.localScale.x, 0),direction));
        weaponRenderer.transform.Rotate(Vector3.forward, startAngle);

        for (float a = startAngle; startAngle < 0 ? a < -startAngle : a > -startAngle; a = a + increment)
        {
            weaponRenderer.transform.Rotate(Vector3.forward, increment);
            weaponRenderer.transform.position = (playerPos + direction * rangeOffset) + (Vector2)(Quaternion.Euler(0, 0, a) * direction * range * rangeOffset) + ((Vector2)(Quaternion.Euler(0, 0, a) * direction * range) / 2);
            Debug.DrawRay((playerPos + direction * rangeOffset) + (Vector2)(Quaternion.Euler(0, 0, a) * direction * range * rangeOffset), Quaternion.Euler(0, 0, a) * direction * range, Color.white, 1f);
            RaycastHit2D rch = Physics2D.Raycast((playerPos + direction * rangeOffset) + (Vector2)(Quaternion.Euler(0, 0, a) * direction * range * rangeOffset), Quaternion.Euler(0, 0, a) * direction * range, range, LayerMask.GetMask("Hittable"));
            if (rch && !r2dl.Contains(rch.transform.GetInstanceID()))
            {
                r2dl.Add(rch.transform.GetInstanceID());
                rch.transform.GetComponent<Enemy>().ApplyDamage(weaponDamage);
                rch.transform.GetComponent<Rigidbody2D>().AddForce(Quaternion.Euler(0, 0, a) * direction * force);
            }
            yield return new WaitForSeconds(cooldowntimer*1.35f/(Math.Abs(startAngle) * 2));
        }
        weaponRenderer.transform.rotation = rotationInit;
        weaponRenderer.transform.localPosition = Vector3.zero;
        released = true;
        
    }
   
    public override IEnumerator AWSMStart(Rigidbody2D playerrb2, Vector2 direction)
    {
        //playerrb2.AddForce(direction.normalized * force);
        blocksMovement = true;
        playerrb2.AddForce(direction.normalized * force);
        yield return new WaitForSeconds(0.45f);

    }
    public override IEnumerator AWSMFinished(Rigidbody2D playerrb2, Vector2 direction)
    {
        blocksMovement = false;
        yield return new WaitForEndOfFrame();
    }
    public override IEnumerator startCharge()
    {
        charged = false;
        released = false;
        for (float t = 0; t < chargeTime; t = t + Time.deltaTime)
        {
            sliderCharge.value = t / chargeTime;
            yield return null;
        }
        charged = true;
    }
    public override IEnumerator startRelease()
    {
        for (float t = chargeTime; t > 0; t = t - Time.deltaTime*3)
        {
            sliderCharge.value = t / chargeTime;
            yield return null;
        }
        sliderCharge.value = 0;
        released = true;
    }

    public override void Equipped(StatsStruct stats)
    {
        base.Equipped(stats);
        playerData = GetComponentInParent<Player>();
        blocksMovement = true;
        cooldowntimer = CalculateCooldown(stats);
        weaponDamage = CalculateDamage(stats);
        chargeTime = 0.25f;

        if (floatingObject == null)
        {
            floatingObject = Instantiate(swordModel, transform);
            weaponRenderer = floatingObject.GetComponentInChildren<SpriteRenderer>();
        }
    }
}
