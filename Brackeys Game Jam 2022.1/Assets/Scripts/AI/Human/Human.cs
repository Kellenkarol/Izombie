using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// using UnityEngine.AI;

public class Human : SimpleAI
{   
    [SerializeField] private int health;
    [SerializeField] private float speed;

    protected bool transformedInZombie;


    public int   Health { get { return health; } set{ health = value; } }
    public float Speed  { get { return speed; }  set{ speed  = value; } }


    bool runningOut;
 
    void Start()
    {
        gameObject.tag       = tagOfHuman;
        gameObject.layer     = (int)(Mathf.Round(Mathf.Log(LayerMask.GetMask(tagOfHuman), 2)));
        SetStartVariables();
        SetAgentSpeed(Speed);
        StopAllCoroutines();
    }

    // Update is called once per frame
    void Update()
    {

        if(PlayerIsOnRange() && !HasObstacleBetweenThePlayerAndMe())
        {
            Vector2 opposite = GetOppositeDiretionOfPlayer();
            target = GetNearstPositionOnNavMesh(opposite* 10 + (Vector2) transform.position);
            agent.SetDestination(target);
        }
        else if(IsOnTheEnd() || stuck)
        {
            stuck = false;
            target = GetNewRandomTargetPosition(true);
            agent.SetDestination(target);
        }

        Debug.DrawLine(transform.position, target, Color.red);
        LookAt(agent.steeringTarget - transform.position);
    }



    public override void TakeDamage(int damage)
    {        
        print("Chamou 'TakeDamage' do Human");
        
        if(Health-damage < 0)
            return;
            
        Health = Health-damage;   

        if(Health == 0)
            TransformInZombie();

    }


    public override void TransformInZombie()
    {
        print("Chamou 'TransformInZombie' do Human");
        if(transformedInZombie)
            return;

        transformedInZombie = true;

        //TODO

        GetComponent<Zombie>().enabled = true;
        this.enabled = false;


    }

}
