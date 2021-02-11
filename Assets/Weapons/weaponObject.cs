using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class weaponObject : MonoBehaviour
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
        if(Input.GetKeyDown(KeyCode.E) && canPuickup)
        {
            player.GetComponent<Player>().PickupWeapon(gameObject);
        }
    }

    public void OnMouseEnter()
    {
        //show ui 
        //check for player pos
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
        for (float d = 0; d <= transitionDutarion + 0.01f ; d = d + 0.05f)
        {
            tmp.alpha = d/transitionDutarion;
            yield return new WaitForSeconds(0.05f);
        }
    }
}
