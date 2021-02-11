using Pathfinding;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

class ShroomScript : Enemy
{

    public Transform target;
    public GameObject player;

    public float speed = 5f;
    public float nextWaypointDist = 1f;
    private bool canAttack = true;
    private bool attacking = false;
    public bool hasTarget = false;
    public Vector2 InitialPosition;
    public Animator animator;

    IEnumerator attackCoroutine;

    public Vector2 pointTarget;

    public float damage = 25f;
    public float force = 15f;
    public float range = 0.5f;
    public float angle = 30f;
    public float rangeOffset = 0.15f;
    public float cooldowntimer = 3f;
    Path path;
    int currentWaipoint = 0;
    bool reacheEndOfPath = false;

    Seeker seeker;

    void Start()
    {
        player = GameObject.FindGameObjectsWithTag("Player")[0];
        seeker = GetComponent<Seeker>();
        healthbar = GetComponentInChildren<Healthbar>();
        healthbar.setHp(currenthp, hp);
        enemyid = "Shroom";
        rigidbody2d = GetComponent<Rigidbody2D>();
        collider2d = GetComponent<Collider2D>();
        enemyRenderer = GetComponentInChildren<SpriteRenderer>();
        target = player.transform;
        sleepingParticles = Resources.Load<GameObject>("SleepParticles");
        animator = GetComponentInChildren<Animator>();
        InitialPosition = transform.position;

        pointTarget = InitialPosition;


        InvokeRepeating("CalcPath", 0, 0.25f);
    }

    void CalcPath()
    {
        if (hasTarget)
        {
            if (seeker.IsDone())
                seeker.StartPath(rigidbody2d.position, target.position, OnPathComplete);
        }
        else
        {
            if (seeker.IsDone())
                seeker.StartPath(rigidbody2d.position, pointTarget, OnPathComplete);
        }

    }

    public override void ApplyDamage(float damage)
    {
        //Debug.Log(gameObject.GetInstanceID() + "is hit");
        //break sleep here
        if (changeRoutine != null)
            StopCoroutine(changeRoutine);
        changeRoutine = healthbar.changeHp(currenthp, currenthp - damage, hp);
        StartCoroutine(changeRoutine);
        currenthp = currenthp - damage;
        if (currenthp < hp * 0.5f)
        {
            agroRange = agroRange * 2;
            speed = speed * 3;
        }
        if (currenthp <= 0)
        {
            GameObject.Find("Player").GetComponent<Player>().RiseXp(xp);
            Destroy(gameObject);
        }
    }

    void OnPathComplete(Path p)
    {
        if (!p.error)
        {
            path = p;
            currentWaipoint = 0;
        }
    }
    void Update()
    {
        enemyRenderer.sortingOrder = -(int)(transform.position.y * orderMult);

        if (sleeping)
        {
            return;
        }

        // patrol zone
        if ((transform.position - target.position).magnitude < agroRange && !player.GetComponent<Player>().cloaked)
        {
            hasTarget = true;
        }


        if (hasTarget && player.GetComponent<Player>().cloaked)
        {
            Vector2 lastPosition = target.position;
            hasTarget = false;
            StartCoroutine(confusedMovement(lastPosition));
        }

        if (path == null)
            return;

        if (currentWaipoint >= path.vectorPath.Count)
        {
            reacheEndOfPath = true;
            return;
        }
        else
        {
            reacheEndOfPath = false;
        }

        Vector2 direction = ((Vector2)path.vectorPath[currentWaipoint] - rigidbody2d.position).normalized;
        Vector2 force = direction * speed * Time.deltaTime;

        if (!attacking)
        {
            rigidbody2d.AddForce(force);
        }

        if (rigidbody2d.velocity.magnitude > 0.1 && !attacking)
        {
            animator.Play("MushroomMove");
        }
        else if (!attacking)
        {
            animator.Play("idle");
        }

        float dist = Vector2.Distance(rigidbody2d.position, path.vectorPath[currentWaipoint]);

        if (dist < nextWaypointDist)
        {
            currentWaipoint++;
        }

        if (direction.x >= 0.01f)
        {
            transform.localScale = new Vector3(-1f, 1f, 1f);
        }
        else if (direction.x <= -0.01f)
        {
            transform.localScale = new Vector3(1f, 1f, 1f);
        }


        if ((target.position - transform.position).magnitude < range && canAttack && hasTarget)
        {
            if (canAttack)
            {
                Debug.Log("Attacking");
                Attack((target.position - transform.position).normalized);

            }
            Debug.Log("Cooldown Started");
            StartCoroutine(AttackCooldown(cooldowntimer));
        }
    }


    private void Attack(Vector2 direction)
    {
        attackCoroutine = AttackPlayer(direction);
        StartCoroutine(attackCoroutine);
    }

    public IEnumerator AttackPlayer(Vector2 direction)
    {
        animator.Play("blowUp");

        float impactForce = 50f;
        float radius = 2f;
        attacking = true;
        canAttack = false;
        Collider2D[] res = Physics2D.OverlapCircleAll(transform.position, radius, LayerMask.GetMask("Player"));
        foreach (var coll in res)
        {
            Player player = coll.GetComponent<Player>();
            if (player)
            {
                player.RecieveDamage(damage);
                player.GetComponent<Rigidbody2D>().AddForce((coll.transform.position - transform.position).normalized * impactForce);
            }
        }
        //play blow animation
        Debug.Log("AAAA");
        yield return new WaitForSeconds(0.35f);
        Debug.Log("BOOM");
        attacking = false;
        Destroy(gameObject);

    }

    public IEnumerator AttackCooldown(float cd)
    {
        yield return new WaitForSeconds(cd);
        Debug.Log("Cooldown Ended");
        canAttack = true;
    }

    public IEnumerator confusedMovement(Vector2 lastPosition)
    {
        float searchDistance = 4;
        for (int i = 0; i < 3; i++)
        {
            pointTarget = lastPosition + new Vector2(UnityEngine.Random.value * searchDistance - searchDistance / 2, UnityEngine.Random.value * searchDistance - searchDistance / 2);
            yield return new WaitForSeconds(1f);
        }
        pointTarget = InitialPosition;

    }
}

