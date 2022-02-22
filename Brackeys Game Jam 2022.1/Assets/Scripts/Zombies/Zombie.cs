using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Zombie : SimpleAI
{

    // public string tagOfEnemy  = "Enemy";

    // [SerializeField] private int health;
    // [SerializeField] private float speed;

    // public int Health { get { return health; } set{ health = value; } }
    // public float Speed  { get { return speed; }  set{ speed  = value; } }


    bool followingEnemy;
    Vector2 humanNearest=Vector2.zero;


    void Start()
    {
        StopAllCoroutines();
        
        gameObject.tag       = tagOfZombie;
        
        int newLayer         = (int)(Mathf.Round(Mathf.Log(LayerMask.GetMask(tagOfZombie), 2)));
        GameManager.SetLayerOfGameObjectAndChilds(gameObject, newLayer);

        SetStartVariables();
        SetAgentSpeed(Speed);
        // KeepFollowingThePlayer(3); 
        StartCoroutine("KeepLookingToNearestHuman");
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
        // humanNearest = GetNearestLayerPositionInRadius();
        followingEnemy = true;

        // print((target - humanNearest).magnitude);

        if((target - humanNearest).magnitude > 0.5f && humanNearest.magnitude > 0.0001f)
        {
            followingEnemy  = true;
            target = humanNearest;
            agent.SetDestination(target);
        }
        Debug.DrawLine((Vector2)transform.position, target+(Vector2)transform.right*0.1f, Color.white);

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





    IEnumerator KeepLookingToNearestHuman()
    {
        while(true)
        {
            humanNearest = GetNearestLayerPositionInRadius(transform.position, layerOfHuman);
            yield return null;
            // yield return new WaitForSeconds(0.01f);
        }
    }


    void OnTriggerStay2D(Collider2D col)
    {
        if(gameObject.tag == tagOfHuman)
            return;

        if(col.tag == tagOfHuman)
        {
            col.GetComponent<SimpleAI>().TakeDamage(1);
        }
    }

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
