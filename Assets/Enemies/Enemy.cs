using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public string enemyid;
    public float agroRange;
    public Rigidbody2D rigidbody2d;
    public Collider2D collider2d;
    public Healthbar healthbar;
    public SpriteRenderer enemyRenderer;
    public IEnumerator changeRoutine;
    public int orderMult = 100;

    public int xp = 3;
    public int hp;
    public float currenthp;
    public bool sleeping;


    protected GameObject sleepingParticles;
    // Update is called once per frame


    public virtual void ApplyDamage(float damage) { }

    public void SetSleeping(float duration)
    {
        StartCoroutine(SetSleepingCr(duration));
        Debug.Log(GetInstanceID() + ": I sleep");
    }

    private IEnumerator SetSleepingCr(float duration)
    {
        sleeping = true;
        var sleepObj = Instantiate(sleepingParticles, transform);
        sleepObj.transform.localScale = new Vector3(transform.localScale.x,1,1);
        sleepObj.transform.Translate(Vector3.up * 0.8f);
        sleepObj.GetComponent<SpriteRenderer>().sortingLayerName = "Overlayed";
        sleepObj.GetComponent<Animator>().Play("SleepAnimation");
        yield return new WaitForSeconds(duration);
        Destroy(sleepObj);
        sleeping = false;
    }
}
