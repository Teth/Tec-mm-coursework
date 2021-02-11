using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagicProjectile : MonoBehaviour
{

    public float baseProjectileDamage;
    private Rigidbody2D magicb2d;
    public SpriteRenderer projRenderer;
    private float currentSpeed;
    private int orderMult = 100;
    private bool collided = false;
    private float distance = 0;
    private int clockwise;
    // Start is called before the first frame update
    void Start()
    {
        magicb2d = GetComponent<Rigidbody2D>();
        projRenderer = GetComponent<SpriteRenderer>();
        currentSpeed = 1;
        StartCoroutine(speedIncrease());
        StartCoroutine(countDistance());
        clockwise = (UnityEngine.Random.value < .5 ? 1 : -1);
    }

    // Update is called once per frame
    private void Update()
    {
        //Debug.Log(magicb2d.velocity.magnitude * Time.deltaTime);
        projRenderer.sortingOrder = -(int)(transform.position.y * orderMult);
        Vector3 cursorPos = Camera.main.ScreenToWorldPoint(Input.mousePosition) - new Vector3(0, 0, -10);
        magicb2d.AddForce((new Vector2(cursorPos.x, cursorPos.y) - (Vector2)transform.position) * currentSpeed / 2);
        magicb2d.velocity = clockwise * Vector2.Perpendicular(new Vector2(cursorPos.x, cursorPos.y) - (Vector2)transform.position).normalized * currentSpeed;

    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        collided = true;
        magicb2d.velocity = Vector2.zero;
        magicb2d.isKinematic = true;
        if (collision.gameObject.layer == LayerMask.NameToLayer("Hittable"))
        {
            
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        magicb2d.isKinematic = true;
        collided = true;
        if (collision.gameObject.layer == LayerMask.NameToLayer("Hittable"))
        {
            if (collision.transform.GetComponent<Enemy>())
            {
                collision.transform.GetComponent<Enemy>().ApplyDamage(CalculateDamage());
            }
        }
        if (collision.transform.GetComponent<Player>())
        {
            collision.transform.GetComponent<Player>().RecieveDamage(CalculateDamage());
        }
        if (collision.gameObject.GetComponent<MagicProjectile>())
        {
            //Blow up
        }
    }


    // 
    private float CalculateDamage() 
    {
        return baseProjectileDamage + baseProjectileDamage + baseProjectileDamage/20 * magicb2d.velocity.magnitude;
    }
    private bool hasCollided()
    {
        return collided;
    }

    IEnumerator speedIncrease()
    {
        for (float t = 0; t < 10f; t = t + 0.25f)
        {
            currentSpeed = currentSpeed + 1f;
            Debug.Log(currentSpeed);
            yield return new WaitForSeconds(0.25f);
        }
    }

    IEnumerator countDistance()
    {
        while(!collided)
        {
            distance = distance + magicb2d.velocity.magnitude * Time.deltaTime;
            yield return null;
        }
        Debug.Log("Hi");
        Debug.Log(distance);
        Destroy(gameObject);
    }
}
