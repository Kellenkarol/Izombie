using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class SpawnManager : MonoBehaviour
{

    [SerializeField] private BoxCollider2D mapArea;

    public float checkToSpawnDelay = 1f;

    [SerializeField] private int maxCountOfEachHuman = 10;

    private List<Transform> spawnPoint = new List<Transform>();

    private List<GameObject> humansPrefab;

    private List<GameObject> humansToSpawn              = new List<GameObject>();
    private List<List<GameObject>> humansSpawned        = new List<List<GameObject>>();
    private List<List<GameObject>> humansSpawnedDead    = new List<List<GameObject>>();

    Transform playerTrans;
    float aux;
    int humansDead;

    // Start is called before the first frame update
    void Start()
    {
        humansPrefab = GameManager.GetAllHumansFromResources().Values.ToList(); 
        print("humansPrefab.Count:  "+humansPrefab.Count);

        playerTrans = GameObject.FindWithTag("Player").transform;

        for(int c=0; c<humansPrefab.Count; c++)
        {
            humansSpawned.Add(new List<GameObject>());
            humansSpawnedDead.Add(new List<GameObject>());
        }

        SetSpawnsPoint();
        StartCoroutine("KeepUpdatingHumansToSpawn");
        StartCoroutine("KeepTryingToSpawn");
    }


    // Update is called once per frame
    void Update()
    {
        aux += Time.deltaTime/2;
        humansDead = (int) aux;
    }


    // fica verificando se é a hora de criar um novo humano
    IEnumerator KeepTryingToSpawn()
    {
        while(true)
        {
            Vector2 position = GetRandomSpawnPosition();
            
            if(CheckIfSpawnIsInsideOfMap(position))
            {   
                // GameObject human;
                // human.transform.position = SimpleAI.FindNearestPositionOnNavMesh(position);
                GameObject human = GetHumanToSpawn();

                if(human != null)
                {
                    // print("ABC");
                    human.transform.position = SimpleAI.FindNearestPositionOnNavMesh(position);
                    // human.transform.rotation = Quaternion.identity;
                    // human.GetComponent<SimpleAI>().agent.enabled = true;
                }

                yield return new WaitForSeconds(checkToSpawnDelay);
            }

            yield return null;

        }
    }


    // fica de olho na quatidade de pessoas mortas, e vai atualizando quais humanos podem ser criados
    IEnumerator KeepUpdatingHumansToSpawn()
    {
        while(true)
        {
            // int humansDead = 10; // usando dados fixos por enquanto

            foreach(GameObject gob in humansPrefab)
            {
                if(!humansToSpawn.Contains(gob))
                {
                    if(humansDead >= gob.GetComponent<SimpleAI>().scriptableOfMyData.humansNeededDie)
                    {
                        humansToSpawn.Add(gob);
                    }
                }
            }

            yield return new WaitForSeconds(0.2f);
        }
    }


    private void SetSpawnsPoint()
    {   
        foreach(Transform trans in transform)
        {
            spawnPoint.Add(trans);
        }
        
    }


    private GameObject GetHumanToSpawn()
    {
        List<int> indexs = new List<int>();

        // verificar quais humanos (seus indexs) ainda não ultrapassaram a quantidade máxima
        for(int c=0; c<humansToSpawn.Count; c++)
        {
            if(humansSpawned[c].Count < maxCountOfEachHuman)
            {
                indexs.Add(c);
            }
        }

        if(humansSpawned.Count == 0)
        {
            indexs.Add(0);
        }
        else if(indexs.Count == 0)
        {
            return null;
        }

        GameObject newHuman;
        int humanPrefabIndex = indexs[Random.Range(0, indexs.Count)];

        if(humansSpawnedDead[humanPrefabIndex].Count == 0)
        {
            newHuman = Instantiate(humansToSpawn[humanPrefabIndex]) as GameObject;
            // print("[1] newHuman.name:  "+newHuman.name);
        }
        else
        {
            newHuman = humansSpawnedDead[humanPrefabIndex][0];  
            // print("[2] newHuman.name:  "+newHuman.name);
        }

        TurnOnHuman(newHuman, humanPrefabIndex);

        // newHuman.transform.rotation = Quaternion.identity;
        newHuman.transform.Rotate(Vector3.zero);
        // print("newHuman.transform.rotation:  "+newHuman.transform.rotation);

        return newHuman;
    }


    private void TurnOnHuman(GameObject human, int index)
    {
        human.SetActive(true);
        humansSpawned[index].Add(human);
        try
        {
            humansSpawnedDead[index].Remove(human);
        }
        catch
        {
            print($"Não há nenhum humano em 'humansSpawnedDead[{index}]'");
        }
    }


    public void TurnOffHuman(GameObject human, int index)
    {
        human.SetActive(false);
        humansSpawnedDead[index].Add(human);
        try
        {
            humansSpawned[index].Remove(human);
        }
        catch
        {
            print($"Não há nenhum humano em 'humansSpawned[{index}]'");
        }
    }


    public void TurnOffHuman(GameObject human)
    {
        human.SetActive(false);
        for(int c1=0; c1<humansSpawned.Count; c1++)
        {
            for(int c2=0; c2<humansSpawned[c1].Count; c2++)
            {
                if(humansSpawned[c1][c2].transform == human.transform)
                {
                    TurnOffHuman(human, c1);
                    return;
                }
            }
        }
    }




    private bool CheckIfSpawnIsInsideOfMap(Vector2 spawnPosition)
    {
        return mapArea.bounds.Contains(spawnPosition);
    }


    private Vector2 GetRandomSpawnPosition()
    {
        return spawnPoint[Random.Range(0, spawnPoint.Count)].position;
    }


}
