using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;

class SkillObject : MonoBehaviour
{
    bool canPuickup = false;
    public TextMeshPro tmp;
    private float pickupRange = 5f;
    private GameObject player;

    // Start is called before the first frame update
    void Start()
    {
        tmp = GetComponentInChildren<TextMeshPro>();
        tmp.alpha = 0;
        player = GameObject.FindGameObjectsWithTag("Player")[0];
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && canPuickup)
        {
            player.GetComponent<Player>().PickupSkill(gameObject);
        }
    }

    public void OnMouseEnter()
    {
        //show ui 
        //check for player pos
        StartCoroutine(changeAlpha(1f));
        if ((player.transform.position - transform.position).magnitude < pickupRange)
            canPuickup = true;
        Debug.Log("Showing ui");
    }

    public void OnMouseExit()
    {
        tmp.alpha = 0;
        StopAllCoroutines();
        canPuickup = false;
        Debug.Log("Hiding ui");
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
