using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class SpawnManager : MonoBehaviour
{

    public float checkToSpawnDelay = 1f;

    private List<Vector2> spawnPoint = new List<Vector2>();

    Transform playerTrans;


    // Start is called before the first frame update
    void Start()
    {
        playerTrans = GameObject.FindWithTag("Player").transform;
        SetSpawnsPoint();

    }


    // Update is called once per frame
    void Update()
    {
        
    }


    IEnumerator KeepTryingToSpawn()
    {
        while(true)
        {
            yield return new WaitForSeconds(checkToSpawnDelay);

        }
    }


    private void SetSpawnsPoint()
    {   
        foreach(Transform trans in transform)
        {
            spawnPoint.Add((Vector2) trans.position);
        }
        
    }


    private List<Vector2> GetNearestSpawn(Vector2 origin, int quant)
    {
        if(quant > spawnPoint.Count) quant   = spawnPoint.Count;

        List<Vector2> nearestSpawn           = new List<Vector2>();
        List<Vector2> spawnPointShorted      = spawnPoint.OrderBy( x => Vector2.Distance(origin, x)).ToList();

        for(int c=0; c<quant; c++)           nearestSpawn.Add(spawnPointShorted[c]);

        return nearestSpawn;
    }

}
