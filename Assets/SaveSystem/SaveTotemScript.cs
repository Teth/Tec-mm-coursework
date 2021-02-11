using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SaveTotemScript : MonoBehaviour
{
    public Sprite enabledSprite;
    bool canPuickup = false;
    SaveDataScript saver;
    SpriteRenderer spRenderer;
    Player player;
    float pickupRange = 2f;
    public TextMeshPro tmp;
    // Start is called before the first frame update
    void Start()
    {
        spRenderer = GetComponentInChildren<SpriteRenderer>();
        saver = GameObject.FindGameObjectsWithTag("SaveObject")[0].GetComponent<SaveDataScript>();
        tmp = GetComponentInChildren<TextMeshPro>();
        tmp.alpha = 0;
        player = GameObject.FindGameObjectsWithTag("Player")[0].GetComponent<Player>();
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && canPuickup)
        {
            SavePlayer();
        }

    }
    public void SavePlayer()
    {
        SaveFile s = new SaveFile
        {
            stats = player.stats,
            playerClass = player.playerClass,
            xp = player.xp,
            lvl = player.level,
            currentSkill = player.skillPrimary ? player.skillPrimary.GetType() : null,
            currentWeapon = WeaponClass.getWeaponClassFromWeapon(player.weapon),
            x_WaypointLocation = transform.position.x,
            y_WaypointLocation = transform.position.y,
            levelName = SceneManager.GetActiveScene().name
        };
        saver.SaveData(s);
        spRenderer.sprite = enabledSprite;
    }

    private void OnMouseOver()
    {
        if ((player.transform.position - transform.position).magnitude < pickupRange)
        {
            if (tmp.alpha == 0)
                StartCoroutine(changeAlpha(0.25f));
            canPuickup = true;
        }
    }

    public void OnMouseEnter()
    {
        if ((player.transform.position - transform.position).magnitude < pickupRange)
        {
            StartCoroutine(changeAlpha(0.25f));
            canPuickup = true;
        }
    }

    public void OnMouseExit()
    {
        tmp.alpha = 0;
        StopAllCoroutines();
        canPuickup = false;

    }

    IEnumerator changeAlpha(float transitionDutarion)
    {
        for (float d = 0; d <= transitionDutarion + 0.01f; d = d + 0.05f)
        {
            tmp.alpha = d / transitionDutarion;
            yield return new WaitForSeconds(0.05f);
        }
    }
}
