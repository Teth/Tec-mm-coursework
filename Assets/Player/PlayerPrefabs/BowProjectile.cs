using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BowProjectile : MonoBehaviour
{

    public float projectileDamage;
    private Rigidbody2D arrowrb2d;
    public SpriteRenderer projRenderer;

    private bool isStuck = false;
    private int orderMult = 100;
    private int force = 100;
    // Start is called before the first frame update
    void Start()
    {
        arrowrb2d = GetComponent<Rigidbody2D>();
        projRenderer = GetComponentInChildren<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log(arrowrb2d.velocity);
        projRenderer.sortingOrder = -(int)(transform.position.y * orderMult);

        if (transform.parent == null && arrowrb2d.velocity.magnitude <= 1f)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if ((collision.gameObject.layer == LayerMask.NameToLayer("Hittable") || collision.gameObject.layer == LayerMask.NameToLayer("MapObject")) && !isStuck)
        {
            transform.SetParent(collision.transform);
            arrowrb2d.isKinematic = true;
            Vector2 vel = arrowrb2d.velocity;
            arrowrb2d.velocity = Vector2.zero;

            if (collision.transform.GetComponent<Enemy>())
            {
                projRenderer.sortingOrder = collision.GetComponent<Enemy>().enemyRenderer.sortingOrder - 1;
                collision.transform.GetComponent<Rigidbody2D>().AddForce(vel * 10);
                collision.transform.GetComponent<Enemy>().ApplyDamage(projectileDamage);
            }
            isStuck = true;
        }
    }

}
