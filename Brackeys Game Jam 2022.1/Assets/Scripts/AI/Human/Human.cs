using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// using UnityEngine.AI;

public class Human : SimpleAI
{   

    protected bool transformedInZombie;


    bool runningOut;
    Vector2 zombieNearest;
 
    void Start()
    {
        StopAllCoroutines();
        LoadData(); // quem deve carregar os dados é o GameManager
        gameObject.tag       = tagOfHuman;
        gameObject.layer     = (int)(Mathf.Round(Mathf.Log(LayerMask.GetMask(tagOfHuman), 2)));
        SetStartVariables();

        // target = player.transform.position; // iniciar seguindo o player caso seja um humano diferente do tipo civil
        // agent.SetDestination(target);

        StartCoroutine("KeepTryingToRunAwayFromZombie");
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 opposite=Vector2.zero;
        bool foundZombie=false;

        if(PlayerIsOnRange() && !HasObstacleBetweenThePlayerAndMe())
        {
            opposite = GetOppositeDiretionOfPlayer();
            foundZombie = true;
            // print("[1]");
        }
        else if(GetIfHasOnRange(transform.position, zombieNearest, layerOfZombie, 25) && 
                 !GetIfHasObstacle(transform.position, zombieNearest))
        {
            opposite = GetOppositeDiretion(zombieNearest);
            foundZombie=true;
            // print("[2]     "+!GetIfHasObstacle(transform.position, zombieNearest));
        }
        else if(IsOnTheEnd() || stuck)
        {
            stuck = false;
            target = GetNewRandomTargetPosition();
            agent.SetDestination(target);
        }

        if(foundZombie)
        {
            target = GetNearstPositionOnNavMesh(opposite* 10 + (Vector2) transform.position);
            agent.SetDestination(target);
        }
        
        ChangeAnimation();

        Debug.DrawLine(transform.position, target, Color.red);
        if(foundZombie)
            Debug.DrawLine(transform.position, zombieNearest, Color.blue);
        // LookAt(agent.steeringTarget - transform.position);
    }


    // public override void TakeDamage(int damage)
    // {        
    //     print("Chamou 'TakeDamage' do Human");
        
    //     if(Health-damage < 0)
    //         return;
            
    //     Health = Health-damage;   

    //     if(Health == 0)
    //         TransformInZombie();

    // }


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


    IEnumerator KeepTryingToRunAwayFromZombie()
    {
        while(true)
        {
            zombieNearest = GetNearestLayerPositionInRadius(transform.position, layerOfZombie);
            // Debug.DrawLine(transform.position, zombieNearest, Color.blue);
            yield return null;
        }
    }

}