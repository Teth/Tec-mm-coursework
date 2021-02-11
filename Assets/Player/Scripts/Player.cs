using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    // Gameplay variables
    public int freePointsStats;

    public float speed;
    public float currenthp;
    [SerializeField]
    public float healthpoints;
    [SerializeField]
    public StatsStruct stats;
    [SerializeField]
    public Weapon weapon;
    public Skill skillClass;
    [SerializeField]
    public Skill skillPrimary;
    [SerializeField]
    public int xp;
    public int ReqXp;
    [SerializeField]
    public int level;
    // Statuses 

    public bool cloaked;
    public bool stunned;

    private bool canUseSkill = true;
    private bool usingSkill = false;

    public bool canAttack = true;
    public bool attacking = false;

    private Skill usedSkill;


    // Code variables 
    public IEnumerator changeRoutine;
    public IEnumerator xpChangeRoutine;

    public PlayerUIController uiController;
    public Rigidbody2D rigidbody2d;
    public Vector3 camOffset;
    public SpriteRenderer playerRenderer;
    public Vector3 LookDirection;
    public Vector3 playerCenterOffset = new Vector3(0, 0.1f, 0);
    private int orderMult = 100;
    public Animator animator;
    public Slider cooldownSlider;
    public Slider chargeSlider;
    public PlayerClass.PlayerClassEnum playerClass;
    // Start is called before the first frame update

    SaveDataScript saver;

    public void LoadData(SaveFile save)
    {
        playerClass = save.playerClass;
        var type = PlayerClass.getClassSkill(save.playerClass);
        Skill skillCl = (Skill)gameObject.AddComponent(type);
        EquipSkill(skillCl, false);
        stats = save.stats;
        if (save.currentSkill != null)
        {
            var typePr = save.currentSkill;
            Skill skillPrim = (Skill)gameObject.AddComponent(typePr);
            EquipSkill(skillPrim);
        }

        if (save.currentWeapon != WeaponClass.WeaponClassEnum.None)
        {
            var wpn = Instantiate(WeaponClass.getWeaponPrefab(save.currentWeapon));
            PickupWeapon(wpn);
        }
        playerRenderer.sprite = PlayerClass.getClassTexture(playerClass);
        xp = save.xp;
        level = save.lvl;
        ReqXp = level ^ 3 * 2;
        animator.runtimeAnimatorController = PlayerClass.getClassAnimator(save.playerClass);
        Debug.Log(save.playerClass);

        playerRenderer.sprite = PlayerClass.getClassTexture(save.playerClass);
        var var_x = save.x_WaypointLocation;
        var var_y = save.y_WaypointLocation;
        transform.position = new Vector2(var_x, var_y);
    }
    void Start()
    {
        animator = GetComponentInChildren<Animator>();
        rigidbody2d = GetComponent<Rigidbody2D>();
        playerRenderer = GetComponentInChildren<SpriteRenderer>();
        saver = GameObject.FindGameObjectsWithTag("SaveObject")[0].GetComponent<SaveDataScript>();
        LoadData(saver.LoadData());
        Debug.Log("Start");
        //stats = new StatsStruct(PlayerStats.insight, PlayerStats.prowess, PlayerStats.strength);
        var type = PlayerClass.getClassSkill(playerClass);
        Skill skillCl = (Skill)gameObject.AddComponent(type);
        EquipSkill(skillCl, false);
        //constants

        uiController.SetLevelText(level);
        uiController.changeXp(0, 0, ReqXp);

        


        camOffset = new Vector3(0,0,Camera.main.transform.position.z);
        UpdateStats();
    }

    // Update is called once per frame
    void Update()
    {
        if (stunned)
        {
            return;
        }
        //rendering properly
         playerRenderer.sortingOrder = -(int)(transform.position.y * orderMult);
        // calc playerData
        Vector3 playerCenterPosition = this.transform.position + playerCenterOffset;
        LookDirection = Camera.main.ScreenToWorldPoint(Input.mousePosition) - camOffset - playerCenterPosition;
        if (LookDirection.x < 0)
            playerRenderer.flipX = true;
        else
            playerRenderer.flipX = false;

        bool skClassBlocksAttack = false;
        if (skillClass)
            skClassBlocksAttack = !skillClass.skillEnded && skillClass.blocksAttack;
        bool skPrimBlocksAttack = false;
        if (skillPrimary)
            skPrimBlocksAttack = !skillPrimary.skillEnded && skillPrimary.blocksAttack;


        //Pressed left mouse btn
        if (weapon && Input.GetMouseButtonDown(0) && canAttack && !skPrimBlocksAttack && !skClassBlocksAttack)
        {
            Attack();
        }

        bool skClassBlocks = false;
        if (skillClass)
            skClassBlocks = !skillClass.skillEnded && skillClass.blocksSkill;
        bool skPrimBlocks = false;
        if (skillPrimary)
            skPrimBlocks = !skillPrimary.skillEnded && skillPrimary.blocksSkill;
        if (!skClassBlocks && !skPrimBlocks)
        {
            if (skillClass && Input.GetKeyDown(KeyCode.C) && skillClass.canUseSkill)
            {
                UseSkill(skillClass);
            }

            if (skillPrimary && Input.GetKeyDown(KeyCode.V) && skillPrimary.canUseSkill)
            {
                UseSkill(skillPrimary);
            }
        }

        //movement
        float hv = Input.GetAxis("Horizontal");
        float vv = Input.GetAxis("Vertical");

        bool skClassBlocksMovement = false;
        if (skillClass)
            skClassBlocksMovement = !skillClass.skillEnded && skillClass.blocksMovement;
        bool skPrimBlocksMovement = false;
        if (skillPrimary)
            skPrimBlocksMovement = !skillPrimary.skillEnded && skillPrimary.blocksMovement;

        Vector2 movement = new Vector2(hv, vv);
        if (!(attacking && weapon.blocksMovement) && !skClassBlocksMovement && !skPrimBlocksMovement)
        {
            if (movement.magnitude > 1f)
                movement.Normalize();
            rigidbody2d.velocity = movement * speed;
            if(rigidbody2d.velocity.magnitude > 0.1)
                animator.Play("movement");
            else
            {
                animator.Play("idle");
            }
        }      
    }

    public void UpdateStats()
    {
        healthpoints = stats.Insight * 1.5f + stats.Strength * 7f + stats.Prowess * 3f;
        if(weapon)
            weapon.Equipped(stats);
        if(skillPrimary)
            skillPrimary.Equipped();
        if(skillClass)
            skillClass.Equipped();
    }

    void UseSkill(Skill skill)
    {
        skill.canUseSkill = false;
        StartCoroutine(skillCoroutine(skill));
    }

    IEnumerator skillCooldown(Skill skill)
    {
        Debug.Log("Cooldown");
        StartCoroutine(skillBarCooldown(skill));
        yield return new WaitForSeconds(skill.cooldowntimer);
        skill.canUseSkill = true;
    }

    IEnumerator skillCoroutine(Skill skill)
    {
        usingSkill = true;
        StartCoroutine(skill.skillMovementStart(rigidbody2d, LookDirection.normalized));
        yield return new WaitUntil(() => skill.movementEnded);
        StartCoroutine(skill.skillCoroutine(transform.position, LookDirection.normalized));
        yield return new WaitUntil(() => skill.skillEnded);
        usingSkill = false;
        StartCoroutine(skillCooldown(skill));
    }

    IEnumerator skillBarCooldown(Skill skill)
    {
        skill.skillBar.color = skill.cooldownBar;
        for (float t = skill.cooldowntimer; t > 0; t = t - Time.deltaTime)
        {
            skill.skillBar.fillAmount = t / skill.cooldowntimer;
            yield return null;
        }
        skill.skillBar.color = skill.normal;
    }

    void Attack()
    {
        canAttack = false;
        StartCoroutine(attack());
    }

    IEnumerator attack()
    {
        attacking = true;

        //weapon specific start movement
        StartCoroutine(weapon.AWSMStart(rigidbody2d, LookDirection.normalized));
        //Charge/pre-swing weapon


        StartCoroutine(weapon.startCharge());
        yield return new WaitUntil(() => weapon.charged);
        //Attack
        if (!weapon.released)
        {
            //Debug.Log("Att");
            //Release/swing weapon
            //StartCoroutine(weapon.startRelease());
            var playerCenterPos = transform.position + playerCenterOffset;
            StartCoroutine(weapon.attackCoroutine(playerCenterPos, LookDirection.normalized));
            //Debug.Log("Att2");
        }
        yield return new WaitUntil(() => weapon.released);

        StartCoroutine(weapon.startRelease());     


        StartCoroutine(weapon.AWSMFinished(rigidbody2d, LookDirection.normalized));
        yield return new WaitWhile(() => weapon.blocksMovement);
        attacking = false;
        //Final weapon specific movement
        StartCoroutine(weapon.startCooldown());
        yield return new WaitForSeconds(weapon.cooldowntimer);
        canAttack = true;

    }


    public void PickupWeapon(GameObject weaponToPickup)
    {
        var oldWpn = GetComponentInChildren<Weapon>();
        if (oldWpn)
        {
            oldWpn.transform.parent = null;
            Destroy(oldWpn.gameObject);
        }

        weaponToPickup.GetComponentInChildren<Weapon>().transform.parent = transform;
        Weapon weaponb = GetComponentInChildren<Weapon>();
        weapon = weaponb;
        weapon.sliderCanvas = transform.Find("SlidersCanvas").GetComponent<Canvas>(); 
        weapon.Equipped(stats);
        Destroy(weaponToPickup);
        //weaponToPickup.SetActive(false);
    }

    public void PickupSkill(GameObject skillToPickup)
    {
        //var oldskill = getcomponentinchildren<skill>();
        //if (oldskill)
        //{
        //    oldskill.transform.parent = null;
        //    destroy(oldskill.gameobject);
        //}
        Skill skill = skillToPickup.GetComponentInChildren<Skill>();
        EquipSkill(skill);
        Destroy(skillToPickup);
        //weaponToPickup.SetActive(false);
    }

    public void RecieveDamage(float damage)
    {
        if (changeRoutine != null)
            StopCoroutine(changeRoutine);
        changeRoutine = uiController.changeHp(currenthp, currenthp - damage, healthpoints);
        StartCoroutine(changeRoutine);
        currenthp = currenthp - damage;
        if (currenthp < 0)
        {
            stunned = true;
            StartCoroutine(deathCoroutine());
        }
    }

    IEnumerator deathCoroutine()
    {
        yield return new WaitForSeconds(2f);
        SceneManager.LoadScene("endGameScene");
    }

    public void EquipSkill(Skill skill, bool isPrimary= true)
    {
        if (isPrimary)
        {
            skillPrimary = (Skill)gameObject.AddComponent(skill.GetType());
            skillPrimary.Equipped();
            skillPrimary.skillKeyCode = KeyCode.V;
            skillPrimary.skillBar = uiController.PrimarySkillBar;
            uiController.SetPrimarySkillName(skillPrimary.skillName);
            uiController.SetPrimarySkillBarBackground(skillPrimary.skillIcon);
        }
        else
        {
            skillClass = (Skill)gameObject.AddComponent(skill.GetType());
            skillClass.Equipped();
            skillClass.skillKeyCode = KeyCode.C;
            skillClass.skillBar = uiController.ClassSkillBar;
            uiController.SetClassSkillName(skillClass.skillName);
            uiController.SetClassSkillBarBackground(skillClass.skillIcon);
        }
    }

    public void RiseXp(int newXp)
    {
        if (xpChangeRoutine != null)
            StopCoroutine(xpChangeRoutine);
        xpChangeRoutine = uiController.changeXp(xp, xp + newXp, ReqXp);
        StartCoroutine(xpChangeRoutine);
        xp = xp + newXp;
        if (xp > ReqXp)
        {
            xp = xp - ReqXp;
            levelUp();
        }
    }

    public void levelUp()
    {
        ReqXp = (int)Math.Pow(level,1.5) * 2;
        level += 1;
        currenthp = healthpoints;
        if(changeRoutine != null)
            StopCoroutine(changeRoutine);
        uiController.HpBarImage.fillAmount = currenthp / healthpoints;
        uiController.SetLevelText(level);
        if (xpChangeRoutine != null)
            StopCoroutine(xpChangeRoutine);
        StartCoroutine(uiController.changeXp(0, xp, ReqXp));
        freePointsStats += 2;
    }
}
