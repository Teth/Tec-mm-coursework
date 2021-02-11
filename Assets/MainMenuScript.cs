using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuScript : MonoBehaviour
{
    SaveDataScript saver;
    // Start is called before the first frame update
    public GameObject CharaterCreatingObj;
    public GameObject MainMenu;
    public GameObject ClassSelection;
    public GameObject StatSelectionSelection;

    public GameObject ClassImage;

    public GameObject ClassDescription;
    public GameObject SkillDescription;


    public GameObject InsCount;
    public GameObject ProwCount;
    public GameObject StrCount;
    public GameObject FreeStatsCount;


    public Button ClassSelected;
    public Button StatsSelectedBegin;


    public Button AddInsBtn;
    public Button RemInsBtn;
    public Button AddStrBtn;
    public Button RemStrBtn; 
    public Button AddProwBtn;
    public Button RemProwBtn;

    StatsStruct stats;

    int freeStats = 3;
    int maxFreeStats;
    int str;
    int prow;
    int ins;
    int strBuff = 0;
    int prowBuff = 0;
    int insBuff = 0;

    public PlayerClass.PlayerClassEnum plClass;

    private void Start()
    {
        saver = GameObject.FindGameObjectsWithTag("SaveObject")[0].GetComponent<SaveDataScript>();
    }
    public void Update()
    {
        if (freeStats == 0)
        {
            AddInsBtn.interactable = false;
            AddStrBtn.interactable = false;
            AddProwBtn.interactable = false;
            StatsSelectedBegin.gameObject.SetActive(true);
        }
        else
        {
            AddInsBtn.interactable = true;
            AddStrBtn.interactable = true;
            AddProwBtn.interactable = true;
            StatsSelectedBegin.gameObject.SetActive(false);
        }
        if (strBuff == 0) { RemStrBtn.interactable = false; } else { RemStrBtn.interactable = true; }
        if (prowBuff == 0) { RemProwBtn.interactable = false; } else { RemProwBtn.interactable = true; }
        if (insBuff == 0) { RemInsBtn.interactable = false; } else { RemInsBtn.interactable = true; }

        InsCount.GetComponent<TMP_Text>().text = ins.ToString();
        StrCount.GetComponent<TMP_Text>().text = str.ToString();
        ProwCount.GetComponent<TMP_Text>().text = prow.ToString();
        FreeStatsCount.GetComponent<TMP_Text>().text = freeStats.ToString();
    }
    public void NewGame()
    {
        MainMenu.SetActive(false);
        StatSelectionSelection.SetActive(false);
        ClassSelection.SetActive(true);
        CharaterCreatingObj.SetActive(true);
        ClassSelected.gameObject.SetActive(false);
    }

    public void LoadGame()
    {
        SceneManager.LoadScene(saver.LoadData().levelName);
    }

    public void ExitGame()
    {
        Application.Quit();
    }

    public void Options()
    {

    }

    public void RogueSelected() {
        plClass = PlayerClass.PlayerClassEnum.Rogue;
        ClassSelected.gameObject.SetActive(true);
    }
    public void WarriorSelected() {
        plClass = PlayerClass.PlayerClassEnum.Warrior;
        ClassSelected.gameObject.SetActive(true);
    }
    public void MageSelected() { 
        plClass = PlayerClass.PlayerClassEnum.Mage;
        ClassSelected.gameObject.SetActive(true);
    }

    public void ClassSelectedNext()
    {
        stats = StatsStruct.getStatsFromClass(plClass);
        ins = stats.Insight;
        str = stats.Strength;
        prow = stats.Prowess;
        ClassDescription.GetComponent<TMP_Text>().text = PlayerClass.getClassDescription(plClass);
        SkillDescription.GetComponent<TMP_Text>().text = PlayerClass.getClassSkillDescription(plClass);
        ClassImage.GetComponent<Image>().sprite = PlayerClass.getClassTexture(plClass);
        StatSelectionSelection.SetActive(true);
        ClassSelection.SetActive(false);
        StatsSelectedBegin.gameObject.SetActive(false);
    }

    public void BackToClassSelection()
    {
        StatSelectionSelection.SetActive(false);
        ClassSelected.gameObject.SetActive(false);
        ClassSelection.SetActive(true);
        strBuff = 0;
        prowBuff = 0;
        insBuff = 0;
        freeStats = 3;

    }
    public void AddProw() {
        prowBuff += 1;
        prow += 1;
        freeStats -= 1;
    }
    public void RemoveProw() {
        prowBuff -= 1;
        prow -= 1;
        freeStats += 1;
    }
    public void AddIns() {
        insBuff += 1;
        ins += 1;
        freeStats -= 1;
    }
    public void RemoveIns() {
        insBuff -= 1;
        ins -= 1;
        freeStats += 1;
    }
    public void AddStr() {
        strBuff += 1;
        str += 1;
        freeStats -= 1;
    }
    public void RemoveStr() {
        strBuff -= 1;
        str -= 1;
        freeStats += 1;
    }

    public void StartGame()
    {
        SaveFile s = new SaveFile
        {
            stats = new StatsStruct(ins, prow, str),
            playerClass = plClass,
            xp = 0,
            lvl = 1,
            currentSkill = null,
            currentWeapon = WeaponClass.WeaponClassEnum.None,
            //Spawn location
            x_WaypointLocation = -8,
            y_WaypointLocation = 1,
            levelName = "Tutorial"
        };
        saver.SaveData(s);
        SceneManager.LoadScene("preLoadLevel");
    }
}
