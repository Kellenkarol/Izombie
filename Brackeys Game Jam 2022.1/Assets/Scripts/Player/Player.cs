using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{

    public string tagOfHuman  = "Human";
    private int attackDamage  = 1;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    void OnTriggerEnter2D(Collider2D col)
    {
        if(col.tag == tagOfHuman)
        {
            col.GetComponent<SimpleAI>().TakeDamage(attackDamage);
        }
    }
}
