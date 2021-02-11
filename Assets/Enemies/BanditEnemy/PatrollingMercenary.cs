using Pathfinding;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

class PatrollingMercenary : Enemy
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
    public GameObject weapon;

    public Vector2 pointTarget;

    public float damage = 10f;
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
        enemyid = "BanditTest";
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
    public override void ApplyDamage(float damage)
    {
        //Debug.Log(gameObject.GetInstanceID() + "is hit");
        //break sleep here
        if (changeRoutine != null)
            StopCoroutine(changeRoutine);
        changeRoutine = healthbar.changeHp(currenthp, currenthp - damage, hp);
        StartCoroutine(changeRoutine);
        currenthp = currenthp - damage;
        if (currenthp < hp * 0.75f)
        {
            agroRange = agroRange * 2;
        }
        if (currenthp <= 0)
        {
            GameObject.Find("Player").GetComponent<Player>().RiseXp(xp);
            Destroy(gameObject);
        }
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
        if((InitialPosition - (Vector2)target.position).magnitude < agroRange && !player.GetComponent<Player>().cloaked)
        {
            hasTarget = true;
        }
        else if ((InitialPosition - (Vector2)target.position).magnitude > 2*agroRange)
        {
            hasTarget = false;
            pointTarget = InitialPosition;
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

        if (rigidbody2d.velocity.magnitude > 0.1)
        {
            animator.Play("GnollWalk");
        }
        else
        {
            animator.Play("GnollIdle");
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
        

        if((target.position - transform.position).magnitude < range && canAttack && hasTarget)
        {
            if(canAttack)
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
        attacking = true;
        canAttack = false;
        direction.Normalize();
        float startAngle = UnityEngine.Random.value < .5f ? angle : -angle;
        int increment = startAngle < 0 ? 5 : -5;
        List<int> r2dl = new List<int>();
        var wpn = Instantiate(weapon);
        wpn.transform.Rotate(Vector3.forward, Vector2.SignedAngle(Vector2.up, direction));

        for (float a = startAngle; startAngle < 0 ? a < -startAngle : a > -startAngle; a = a + increment)
        {
            wpn.transform.Rotate(Vector3.forward, increment);
            wpn.transform.position = ((Vector2)transform.position + direction * rangeOffset) + (Vector2)(Quaternion.Euler(0, 0, a) * direction * range * rangeOffset) + ((Vector2)(Quaternion.Euler(0, 0, a) * direction * range) / 2);
            Debug.DrawRay((rigidbody2d.position + direction * rangeOffset) + (Vector2)(Quaternion.Euler(0, 0, a) * direction * range * rangeOffset), Quaternion.Euler(0, 0, a) * direction * range, Color.white, 1f);
            RaycastHit2D rch = Physics2D.Raycast((rigidbody2d.position + direction * rangeOffset) + (Vector2)(Quaternion.Euler(0, 0, a) * direction * range * rangeOffset), Quaternion.Euler(0, 0, a) * direction * range, range, LayerMask.GetMask("Player"));
            if (rch && !r2dl.Contains(rch.transform.GetInstanceID()))
            {
                r2dl.Add(rch.transform.GetInstanceID());
                rch.transform.GetComponent<Player>().RecieveDamage(damage);
                rch.transform.GetComponent<Rigidbody2D>().AddForce(Quaternion.Euler(0, 0, a) * direction * force * 10);
            }
            yield return new WaitForSeconds(cooldowntimer * 0.35f / (startAngle * 2));
        }
        Destroy(wpn);
        attacking = false;

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
            pointTarget = lastPosition + new Vector2(UnityEngine.Random.value * searchDistance - searchDistance/2, UnityEngine.Random.value * searchDistance - searchDistance / 2);
            yield return new WaitForSeconds(1f);
        }
        pointTarget = InitialPosition;

    }
}

