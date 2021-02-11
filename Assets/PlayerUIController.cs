using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUIController : MonoBehaviour
{
    // Start is called before the first frame update
    public IEnumerator changeRoutine;
    public Image HpBarImage;
    public Image ExperienceBarImage;
    public Image ClassSkillBar;
    public Image PrimarySkillBar;

    public Image ClassSkillBarBackground;
    public Image PrimarySkillBarBackground;

    public TMP_Text LevelText;
    public TMP_Text ClassSkillName;
    public TMP_Text PrimarySkillName;

    public void SetLevelText(int level)
    {
        LevelText.text = string.Format("Lvl:{0}", level);
    }
    public void SetClassSkillName(string skillName)
    {
        ClassSkillName.text = skillName;
    }
    public void SetPrimarySkillName(string skillName)
    {
        PrimarySkillName.text = skillName;
    }
    public void SetClassSkillBarBackground(Sprite background)
    {
        ClassSkillBarBackground.sprite = background;
    }
    public void SetPrimarySkillBarBackground(Sprite background)
    {
        PrimarySkillBarBackground.sprite = background;
    }
    public IEnumerator changeXp(float xpBefore, float xp, float maxXp)
    {
        float iterator = (xp - xpBefore) / 16;
        for (float i = xpBefore; i < xp + 0.1; i += iterator)
        {
            ExperienceBarImage.fillAmount = i / maxXp;
            iterator = (maxXp - i) / 16;
            yield return new WaitForSeconds(0.02f);
        }
        ExperienceBarImage.fillAmount = xp / maxXp;
        yield return null;

    }
    public IEnumerator changeHp(float healthbefore, float health, float maxHealth)
    {
        float iterator = (healthbefore - health) / 16;
        for (float i = healthbefore; i > health - 0.1; i -= iterator)
        {
            HpBarImage.fillAmount = i / maxHealth;
            iterator = (i - health) / 16;
            yield return new WaitForSeconds(0.02f);
        }
        HpBarImage.fillAmount = health / maxHealth;
        yield return null;

    }
}
