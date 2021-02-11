using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProperSpriteRenderer : MonoBehaviour
{

    SpriteRenderer renderer;
    int orderMult = 100; 
    // Start is called before the first frame update
    void Start()
    {
        renderer = GetComponentInChildren<SpriteRenderer>();

        renderer.sortingOrder = -(int)(transform.position.y * orderMult);
    }
}
