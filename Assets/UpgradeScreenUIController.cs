using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeScreenUIController : MonoBehaviour
{
    public GameObject Player;
    public Player player;
    // Start is called before the first frame update

    public GameObject InsCount;
    public GameObject ProwCount;
    public GameObject StrCount;
    public GameObject FreeStatsCount;

    public GameObject ClassDescription;
    public GameObject SkillDescription;

    public GameObject ClassImage;

    public Button ClassSelected;
    public Button StatsSelectedBegin;


    public Button AddInsBtn;
    public Button RemInsBtn;
    public Button AddStrBtn;
    public Button RemStrBtn;
    public Button AddProwBtn;
    public Button RemProwBtn;

    StatsStruct stats;

    int maxFreeStats;

    int strBuff = 0;
    int prowBuff = 0;
    int insBuff = 0;

    Canvas c;
    void Start()
    {
        player = Player.GetComponent<Player>();
        c = GetComponent<Canvas>();
        c.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        
        if (Input.GetKeyDown(KeyCode.U))
        {
            c.enabled = !c.enabled;
            if (c.enabled == true)
            {
                Time.timeScale = 0;
            }
            else
            {
                player.UpdateStats(); 
                strBuff = 0;
                prowBuff = 0;
                insBuff = 0;
                Time.timeScale = 1;
            }
        }
        if (c.enabled == true)
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                c.enabled = false;
            }
            if (player.freePointsStats == 0)
            {
                AddInsBtn.interactable = false;
                AddStrBtn.interactable = false;
                AddProwBtn.interactable = false;
            }
            else
            {
                AddInsBtn.interactable = true;
                AddStrBtn.interactable = true;
                AddProwBtn.interactable = true;
            }
            if (strBuff == 0) {
                RemStrBtn.interactable = false; 
            } else { 
                RemStrBtn.interactable = true;
            }
            if (prowBuff == 0) {
                RemProwBtn.interactable = false;
            } else { 
                RemProwBtn.interactable = true;
            }
            if (insBuff == 0) {
                RemInsBtn.interactable = false;
            } else { 
                RemInsBtn.interactable = true; 
            }

            var plClass = player.playerClass;
            InsCount.GetComponent<TMP_Text>().text = player.stats.Insight.ToString();
            StrCount.GetComponent<TMP_Text>().text = player.stats.Strength.ToString();
            ProwCount.GetComponent<TMP_Text>().text = player.stats.Prowess.ToString();
            FreeStatsCount.GetComponent<TMP_Text>().text = player.freePointsStats.ToString();
            ClassDescription.GetComponent<TMP_Text>().text = PlayerClass.getClassDescription(plClass);
            SkillDescription.GetComponent<TMP_Text>().text = PlayerClass.getClassSkillDescription(plClass);
            ClassImage.GetComponent<Image>().sprite = PlayerClass.getClassTexture(plClass);
        }
    }

    public void AddProw()
    {
        prowBuff += 1;
        player.stats.Prowess += 1;
        player.freePointsStats -= 1;
    }
    public void RemoveProw()
    {
        prowBuff -= 1;
        player.stats.Prowess -= 1;
        player.freePointsStats += 1;
    }
    public void AddIns()
    {
        insBuff += 1;
        player.stats.Insight += 1;
        player.freePointsStats -= 1;
    }
    public void RemoveIns()
    {
        insBuff -= 1;
        player.stats.Insight -= 1;
        player.freePointsStats += 1;
    }
    public void AddStr()
    {
        strBuff += 1;
        player.stats.Strength += 1;
        player.freePointsStats -= 1;
    }
    public void RemoveStr()
    {
        strBuff -= 1;
        player.stats.Strength -= 1;
        player.freePointsStats += 1;
    }
}
