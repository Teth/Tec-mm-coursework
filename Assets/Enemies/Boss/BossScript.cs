using Pathfinding;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

class BossScript : Enemy
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
    public GameObject fireballPrefab;
    public GameObject bossKey;
    public Vector2 pointTarget;

    public float damage = 10f;
    public float force = 15f;
    public float range = 0.5f;
    public float angle = 30f;
    public float rangeOffset = 0.15f;
    public float cooldowntimer = 2f;
    Path path;
    int currentWaipoint = 0;
    bool reacheEndOfPath = false;
    public MusicControllerScript music;
    bool musicPlaying = false;
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
        bossKey = Resources.Load<GameObject>("BossKey");
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
        if (!musicPlaying)
        {
            StartCoroutine(music.ChangeToBossMusic());
            musicPlaying = true;
        }
        if (currenthp < hp * 0.75f)
        {
            agroRange = agroRange * 2;
        }
        if (currenthp <= 0)
        {
            GameObject.Find("Player").GetComponent<Player>().RiseXp(xp);
            Instantiate(bossKey,transform.position,transform.rotation);
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
        if ((transform.position - target.position).magnitude < agroRange && !player.GetComponent<Player>().cloaked)
        {
            hasTarget = true;
        }
        else if ((InitialPosition - (Vector2)target.position).magnitude > 2 * agroRange)
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

        float dist = Vector2.Distance(rigidbody2d.position, path.vectorPath[currentWaipoint]);

        if (dist < nextWaypointDist)
        {
            currentWaipoint++;
        }

        if (rigidbody2d.velocity.x >= 0.1f)
        {
            transform.localScale = new Vector3(-1f, 1f, 1f);
        }
        else if (rigidbody2d.velocity.x <= -0.1f)
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
        attacking = true;
        canAttack = false;
        direction.Normalize();
        Vector2 instantiatePosition = (Vector2)transform.position + (direction).normalized * 1f;
        GameObject arrowClone = Instantiate(fireballPrefab,
            instantiatePosition, Quaternion.Euler(0, 0, Vector2.SignedAngle(new Vector2(1, 0), direction)));
        Debug.DrawRay(instantiatePosition, direction, Color.red, 3f);
        arrowClone.GetComponent<FireballScript>().projectileDamage = damage;
        arrowClone.GetComponent<Rigidbody2D>().velocity = ((direction).normalized * 10f);
        yield return new WaitForSeconds(0.3f);

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
            pointTarget = lastPosition + new Vector2(UnityEngine.Random.value * searchDistance - searchDistance / 2, UnityEngine.Random.value * searchDistance - searchDistance / 2);
            yield return new WaitForSeconds(1f);
        }
        pointTarget = InitialPosition;

    }
}

