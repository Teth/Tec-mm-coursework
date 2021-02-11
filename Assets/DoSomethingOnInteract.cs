using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using TMPro;
using UnityEngine;

public class DoSomethingOnInteract : MonoBehaviour
{
    Canvas c;
    public TextMeshPro tmp;
    bool canPuickup = false;
    GameObject player;
    float pickupRange = 1f;
    void Start()
    {
        tmp = GetComponentInChildren<TextMeshPro>();
        tmp.alpha = 0;
        c = GetComponentInChildren<Canvas>();
        c.enabled = false;
        player = GameObject.FindGameObjectsWithTag("Player")[0];
    }

    // Update is called once per frame
    void Update()
    {
        if ((Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.E)) && c.enabled)
        {
            c.enabled = false;
        }
        if (Input.GetKeyDown(KeyCode.E) && canPuickup)
        {
            c.enabled = !c.enabled;
        }
        
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
