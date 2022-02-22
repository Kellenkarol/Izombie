using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Human
{
    [SerializeField] private float attackRange;
    [SerializeField] private float attackSpeed;

    public float AttackRange { get { return attackRange; } set{ attackRange = value; } }
    public float AttackSpeed { get { return attackSpeed; } set{ attackSpeed = value; } }


 
    void Start()
    {
        gameObject.tag       = tagOfHuman;
        gameObject.layer     = layerOfHuman;
        SetStartVariables();
        SetAgentSpeed(Speed);
    }

    // Update is called once per frame
    void Update()
    {  
        DrawLine(transform.position, target);
        CheckIfPlayerIsNearAndFollowHim();            
        CheckIsHasNeedChangeDirectionAndDoIt();
        LookAt(agent.steeringTarget - transform.position);
    }



    public override void TakeDamage(int damage)
    {        
        print("Chamou 'TakeDamage' do Enemy");
        
        if(Health-damage < 0)
            return;
            
        Health = Health-damage;   

        if(Health == 0)
            TransformInZombie();

    }


    public override void TransformInZombie()
    {
        print("Chamou 'TransformInZombie' do Enemy");
        if(transformedInZombie)
            return;

        transformedInZombie = true;

        //TODO
    }


}
