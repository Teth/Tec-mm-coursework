using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Healthbar : MonoBehaviour
{
    // Start is called before the first frame update

    public Slider Slider;
    public Vector3 Offset;
    public IEnumerator hproutine;
    void Start()
    {
        
    }

    public void setHp(float health, float maxHealth)
    {
        Slider.maxValue = maxHealth;
        Slider.value = health;
    }

    public void ReduceHp(float health, float maxHealth)
    {
        Slider.gameObject.SetActive(health < maxHealth);
        var hpBefore = Slider.value;
        StartCoroutine(changeHp(hpBefore, health, maxHealth));
    }

    public IEnumerator changeHp(float healthbefore, float health, float maxHealth)
    {
        Slider.value = healthbefore;
        float iterator = (healthbefore - health)/16;
        for (float i = healthbefore; i > health - 0.1; i -= iterator)
        {
            Slider.value = i;
            iterator = (i - health) / 16;
            yield return new WaitForSeconds(0.02f);
        }
        Slider.value = health;
        yield return null;

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Slider.transform.position = Camera.main.WorldToScreenPoint(transform.parent.position + Offset);   
    }
}
