using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthOfEnemy : MonoBehaviour
{
    [SerializeField] private float health;


    public float Health 
    { 
        get 
        { 
            return health; 
        } 
        set 
        { 
            health = value; 
        } 
    }

}
