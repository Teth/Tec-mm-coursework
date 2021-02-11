using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

public abstract class Weapon : MonoBehaviour
{
    public Canvas sliderCanvas;
    public Slider sliderCharge;
    public Slider sliderCd;
    protected Vector3 OffsetSlider = new Vector3(0,-0.1f,0);
    protected Vector3 OffsetSlider2 = new Vector3(0,-0.2f, 0);

    public string weaponName;
    public float weaponDamage;
    public float cooldowntimer;
    public bool blocksMovement;
    public float chargeTime;

    public bool charged = false;
    public bool released = false;

    public float baseDamage;
    public float baseCd;
    public float rangeOffset;
    public float strDmgCoef;
    public float prowDmgCoef;
    public float insDmgCoef;
    public float strCdCoef;
    public float prowCdCoef;
    public float insCdCoef;

    public virtual void Equipped(StatsStruct stats) 
    {
        sliderCanvas.gameObject.SetActive(true);
        var arr = transform.parent.GetComponentsInChildren<Slider>();
        sliderCharge = arr[0];
        sliderCd = arr[1];
    }
    public float CalculateCooldown(StatsStruct stats)
    {
        return baseCd / (strCdCoef * stats.Strength + prowCdCoef * stats.Prowess + insCdCoef * stats.Insight);
    }
    public float CalculateDamage(StatsStruct stats)
    {
        return (strDmgCoef * stats.Strength + prowDmgCoef * stats.Prowess + insDmgCoef * stats.Insight) * baseDamage;
    }
    public abstract IEnumerator attackCoroutine(Vector2 playerPos, Vector2 direction);

    //apply weapon specific movement as soon as attack started
    public abstract IEnumerator AWSMStart(Rigidbody2D playerrb2, Vector2 direction);

    //apply weapon specific movement after attack finished
    public abstract IEnumerator AWSMFinished(Rigidbody2D playerrb2, Vector2 direction);
    public abstract IEnumerator startCharge();
    public abstract IEnumerator startRelease();
    public IEnumerator startCooldown()
    {
        for (float t = 0; t < cooldowntimer; t = t + Time.deltaTime)
        {
            sliderCd.value = t / cooldowntimer;
            yield return null;
        }
        sliderCd.value = 1;
    }


}
