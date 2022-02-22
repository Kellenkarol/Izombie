using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Zombie : SimpleAI
{

    // public string tagOfEnemy  = "Enemy";

    [SerializeField] private int health;
    [SerializeField] private float speed;

    public int Health { get { return health; } set{ health = value; } }
    public float Speed  { get { return speed; }  set{ speed  = value; } }


    bool followingEnemy;

    void Start()
    {
        gameObject.tag       = tagOfZombie;
        gameObject.layer     = (int)(Mathf.Round(Mathf.Log(LayerMask.GetMask(tagOfZombie), 2)));
        // print(LayerMask.GetMask(tagOfZombie));
        SetStartVariables();
        SetAgentSpeed(Speed);
        KeepFollowingThePlayer(3); 
        // LookAt(agent.steeringTarget - transform.position);
    }

    // Start is called before the first frame update
    void OnEnable()
    {
        GetComponent<SpriteRenderer>().color = Color.blue;       
    }

    // Update is called once per frame
    void Update()
    {
        LookAt(agent.steeringTarget - transform.position);
        Vector2 enemyNearest = GetNearestEnemyInRadius();
        followingEnemy = false;



        if(!((enemyNearest-Vector2.zero).magnitude < 0.0001f) && (player.position-transform.position).magnitude < 25)
        {
            followingEnemy = true;
            target = enemyNearest;
            agent.SetDestination(target);
        }

        // Debug.DrawLine((Vector2)transform.position, target, Color.white);

        if(followingEnemy)
            return;

        if((player.position-transform.position).magnitude > 5)
        {
            KeepFollowingThePlayer(3);
        }
    }

    public void Attack()
    {

    }

    public void Movement()
    {

    }

    public void TransformInHuman()
    {

    }


    private Vector2 GetNearestEnemyInRadius(float radius=25)
    {
        List<RaycastHit2D> hits = new List<RaycastHit2D>(Physics2D.CircleCastAll(transform.position, radius, Vector2.zero, radius, layerOfHuman));

        if(hits.Count <= 1)
            return Vector2.zero;

        hits = hits.OrderBy( x => Vector2.Distance(this.transform.position, x.transform.position)).ToList();
        Vector2 correctPoint = Physics2D.LinecastAll(transform.position, hits[1].point, layerOfHuman)[1].point;    
        Debug.DrawLine((Vector2) transform.position, correctPoint, Color.blue);

        return correctPoint;

        // print("hits.Length:  "+hits.Length);

        // float minDistance = Mathf.Infinity;
        // Vector2 nearestEnemyPosition = hits[1].point;
        // // Debug.DrawLine((Vector2) transform.position, nearestEnemyPosition, Color.blue);


        // foreach(RaycastHit2D hit in hits)
        // {
        //     if(hit.point != (Vector2) transform.position)
        //     {
        //         Debug.DrawLine((Vector2) transform.position, (Vector2) hit.point, Color.blue);
        //         if(((Vector2) hit.point-(Vector2) transform.position).magnitude <= minDistance)
        //         {
        //             minDistance = ((Vector2) hit.point-(Vector2) transform.position).magnitude;
        //             nearestEnemyPosition = hit.point;
        //         }
        //     }
        // }

        // return nearestEnemyPosition;

    }


    // void OnTriggerStay2D(Collider2D col)
    // {
    //     if(col.tag == tagOfEnemy && (player.position-transform.position).magnitude < 25)
    //     {
    //         followingEnemy = col.transform;
    //         target = col.transform.position;
    //         agent.SetDestination(target);
    //     }
    //     else
    //     {
    //         followingEnemy = null;
    //     }
    // }

    // void OnTriggerExit2D(Collider2D col)
    // {
    //     if(followingEnemy == null)
    //         return;

    //     if(col.tag == tagOfEnemy && col.transform == followingEnemy)
    //     {   
    //         followingEnemy = null;
    //     }
    // }

}
