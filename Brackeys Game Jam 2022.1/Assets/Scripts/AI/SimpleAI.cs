using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using System.Reflection;
using System.Linq;


[System.Serializable]
public class CharacterData
{
    [SerializeField] Dictionary<string, List<float>> properties;

    public Dictionary<string, List<float>> Properties
    {
        get
        {
            return properties;
        }
        set
        {
            properties = value;
        }
    }

    public CharacterData(Dictionary<string, List<float>> _properties)
    {
        Properties = _properties;
    }
}




public class SimpleAI : MonoBehaviour
{
    public string tagOfHuman  = "Human";
    public string tagOfZombie = "Zombie";

    public ScriptableOfCharacter scriptableOfMyData;

    protected Dictionary<string, List<float>> myProperties;

    [SerializeField] private int health;
    [SerializeField] private float speed;

    public int   Health { get { return health; } set{ health = value; } }
    public float Speed  { get { return speed; }  set{ speed  = value; } }


    protected Transform player;
    public LayerMask layerOfObstacles, layerOfHuman, layerOfZombie;
    public float visualDistance, rangeOfRandomPosition=15;

    protected Vector2 target;
    protected NavMeshAgent agent;
    protected NavMeshObstacle myObstacle;

    Vector3[] cornersOfPath;

    HealthOfEnemy myHealth;

    protected bool isOnEnd, pathFound, stuck, followingPlayer, lastTargetWasThePlayer, lostThePlayer, tryingFindPlayer;
    protected int currentIndexOfPathCorner;
    protected float _speed;


    // deve ser chamado no start dos filhos
    protected void SetStartVariables()
    {
        StopAllCoroutines();

        agent                   = GetComponent<NavMeshAgent>();
        agent.updateRotation    = false;
        agent.updateUpAxis      = false;

        target = GetNewRandomTargetPosition(true);
        agent.SetDestination(target);
        SetAgentSpeed(myProperties["speed"][0]);


        player = GameObject.FindWithTag("Player").transform;

        StartCoroutine("ChangeDestinationWhenStuck");
    }


    // Save e Load ------------------------------------------------------------------------
        public void SaveData()
        {
            try
            {
                CharacterData data = new CharacterData(myProperties);
                // data.Properties["speed"][0] = 20;
                SaveLoad.Save<CharacterData>(data, scriptableOfMyData.Name());
                Debug.Log($"Salvamendo de {scriptableOfMyData.Name()} feito com sucesso!");
            }
            catch (System.Exception e)
            {
                Debug.Log($"[!] Erro no salvamendo de {scriptableOfMyData.Name()}!");
                Debug.LogException(e, this);
            }
        }


        public void LoadData()
        {
            try
            {
                // CharacterData newData = new CharacterData(scriptableOfMyData.GetFloatProperties());
                CharacterData data = SaveLoad.Load<CharacterData>(scriptableOfMyData.Name());

                if(data == null)
                {
                    data = new CharacterData(scriptableOfMyData.GetFloatProperties()); // default
                    // data = newData; // default
                    Debug.LogWarning($"Não foi encontrado um save para {scriptableOfMyData.Name()}. Usando valores padrões");
                }
                else
                {
                    data = new CharacterData(MergeProperties(scriptableOfMyData.GetFloatProperties(), data.Properties));
                    print("Save carregado com sucesso");
                }

                foreach(KeyValuePair<string, List<float>> a in data.Properties)
                {
                    print($"{a.Key}  {a.Value.Count}");
                }
                myProperties = data.Properties;
                
            }
            catch (System.Exception e)
            {
                Debug.Log($"[!] Erro no carregamento dos dados de {scriptableOfMyData.Name()}!");
                Debug.LogException(e, this);
            }

        }


        // essa função é importante para os casos onde se tem adicionado uma nova propriedade no scriptable.
        // ela mantem o as propriedades sempre atualizadas
        private Dictionary<string, List<float>> MergeProperties(Dictionary<string, List<float>> propertiesOfScriptable, Dictionary<string, List<float>> propertiesOfSave)
        {
            // OBS: propertiesOfSave é mais importante, então ele irá sobrescrever propertiesOfScriptable no caso de chaves iguals

            Dictionary<string, List<float>> mergedProperties = propertiesOfScriptable;

            foreach(KeyValuePair<string, List<float>> prop in propertiesOfSave)
            {
                for(int c=0; c<prop.Value.Count; c++)
                {
                    mergedProperties[prop.Key][c] = prop.Value[c];
                }
            }

            return mergedProperties;
        }
    // ---------------------------------------------------------------------------------------------------------


    protected void CheckIfPlayerIsNearAndFollowHim()
    {
        followingPlayer = !HasObstacleBetweenThePlayerAndMe() && PlayerIsOnRange();
        if(followingPlayer)
        {
            StopCoroutine("TryFindThePlayer");
            print("Achei muahahahahaha");
            lastTargetWasThePlayer  = true;
            lostThePlayer           = false;
            target                  = player.position;
            agent.SetDestination(target);
        }
        else if(lastTargetWasThePlayer && !lostThePlayer)
        {
            print("Perdi o safado");
            lostThePlayer           = true;
            StopCoroutine("TryFindThePlayer");
            List<Vector2> closePositions = GetListOfPositionsNearOfThePlayer(6, true, 6, 25, 3); //GetListOfPositionsNearOfThePlayer(int count = 2, bool StartWithPlayerPosition = true, float minRange = 5, float maxRange = 15, float maxCountOfExellentPositions = 1)
            closePositions = SortByClosest(closePositions);
            StartCoroutine(TryFindThePlayer(closePositions));
        }
    }


    protected void KeepFollowingThePlayer(float randomRangePosition)
    {
        target = player.position;
        // target = player.position + Random.Range(-randomRangePosition,randomRangePosition);
        agent.SetDestination(target);
    }


    // IEnumerator KeepFollowingThePlayerAsync()
    // {

    // }


    protected void CheckIsHasNeedChangeDirectionAndDoIt()
    {
        if(((IsOnTheEnd() && !tryingFindPlayer) || stuck))
        {
            stuck           = false;
            target          = GetNewRandomTargetPosition(true);
            agent.SetDestination(target);
        }
    }



    protected IEnumerator TryFindThePlayer(List<Vector2> positions)
    {   

        print("Mas eu vou achar ele, nem que seja a última coisa que eu faça");
        tryingFindPlayer = true;

        foreach(Vector2 position in positions)
        {
            target = position;
            Debug.DrawLine(transform.position, target, Color.red);
            agent.SetDestination(target);

            print("Waiting");
            yield return new WaitUntil(IsOnTheEnd);
        }
        print("Desisto");

        tryingFindPlayer = false;
    }



    public virtual void TakeDamage(int damage)
    {
        print("Chamou 'TakeDamage' do SimpleEnemyAI");


    }


    public virtual void TransformInZombie()
    {
        
    }


    protected bool IsOnTheEnd()
    {
        return (target-(Vector2)transform.position).magnitude <= 0.01f;
    }


    protected int GetRandomPriority()
    {
        return Random.Range(0, 100);
    }


    protected bool GetIfHasObstacle(Vector2 origin, Vector2 end)
    {
        return Physics2D.Linecast(origin, end, layerOfObstacles);
    }

    protected bool GetIfHasOnRange(Vector2 origin, Vector2 end, LayerMask layer, float radius)
    {
        RaycastHit2D hit = Physics2D.Linecast(origin, end, layer);
        return hit.distance > 0 && hit.distance <= radius;
    }

    protected void DrawLine(Vector2 start, Vector2 end)
    {
        Debug.DrawLine(start, end, Color.blue);
    }


    protected IEnumerator ChangeDestinationWhenStuck()
    {
        while(true)
        {
            Vector2 myOldPosition = transform.position;
            yield return new WaitForSeconds(0.5f);
            Vector2 myNewPosition = transform.position;
            // print((myOldPosition-myNewPosition).magnitude);
            if((myOldPosition-myNewPosition).magnitude <= 0.5f)
            {
                print("Held, i'm stuck");
                stuck = true;
            }

        }
    }




    protected void SetAgentNewDestination(Vector2 target)
    {
        // print("Definindo nova posição");
        pathFound           = false;
        myObstacle.enabled  = false;
        agent.enabled       = true;

        if(agent.hasPath) 
        {
            // print("Resetando caminho antigo");
            agent.ResetPath();
        }

        agent.SetDestination(target);
        StopAllCoroutines();
        StartCoroutine(WaitUntilFindPath());
    }

    protected void SetAgentSpeed(float speed)
    {
        agent.speed = speed;
    }


    protected IEnumerator WaitUntilFindPath()
    {
        while(!agent.hasPath)
        {
            // print("Esperando encontrar caminho");
            yield return null;
        }

        SetCorners(); // atualiza as posições do caminho
        
        myObstacle.enabled  = true;
        agent.enabled       = false;
        pathFound           = true;
        // print("caminho encontrado");
    }


    protected void SetAgentDestinationAndCorners(Vector2 target)
    {
        // myObstacle.enabled          = false;
        agent.enabled               = true;
        agent.SetDestination(target);
        SetCorners();
        agent.enabled               = false;
        // myObstacle.enabled          = true;

    }


    protected void SetCorners()
    {
        cornersOfPath = agent.path.corners;
    }


    protected void MoveThroughPath()
    {
        // print(cornersOfPath.Length+"          "+currentIndexOfPathCorner);

        transform.position = Vector2.MoveTowards(transform.position, cornersOfPath[currentIndexOfPathCorner], _speed*Time.deltaTime);

        bool moveToNextCorner = ((Vector2)(cornersOfPath[currentIndexOfPathCorner] - transform.position)).magnitude <= 0.001f;
        
        if(moveToNextCorner)
        {
            // print("[!] moveToNextCorner");
            if(currentIndexOfPathCorner < cornersOfPath.Length-1)
            {
                // print("[!] currentIndexOfPathCorner++");
                currentIndexOfPathCorner++;
            }
            // else
            // {
            //     print("[!] ResetPath");
            //     agent.ResetPath();
            // }
        }
    }

    
    protected List<Vector2> GetListOfPositionsNearOfThePlayer(int count = 2, bool StartWithPlayerPosition = true, float minRange = 5, float maxRange = 15, float maxCountOfExellentPositions = 1)
    {
        List<Vector2> positions = new List<Vector2>();

        if(StartWithPlayerPosition)
        {
            count--;
            positions.Add(GetNearstPositionOnNavMesh(player.position));
        }

        maxCountOfExellentPositions = maxCountOfExellentPositions > count ? count : maxCountOfExellentPositions;

        for(int c=0; c<maxCountOfExellentPositions; c++)
        {
            Vector2 p = GetNewRandomPositionNearOfThePlayer(minRange, maxRange, true);
            Debug.DrawLine(player.position, p, Color.magenta, 10);
            positions.Add(p);
        }
        
        for(int c=0; c<count-maxCountOfExellentPositions; c++)
        {
            Vector2 p = GetNewRandomPositionNearOfThePlayer(minRange, maxRange, false);
            Debug.DrawLine(player.position, p, Color.black, 10);
            positions.Add(p);
        }

        print("positions.Count: "+positions.Count);

        return positions;
    }



    // usar quando o inimigo está seguindo o player mas o perde de vista
    // basicamente o que a função faz é restornar uma posição proxima ao player
    // para que assim o inimigo tenha uma pista da sua possível localização
    protected Vector2 GetNewRandomPositionNearOfThePlayer(float minRange = 5, float maxRange = 15, bool useLineCast=false)
    {
        Vector2 playerPos       = player.position;
        Vector2 playerPosRight  = player.transform.right;
        Vector2 playerPosUp     = player.transform.up;
        Vector2 randomDirection;
        Vector2 randomDirectionWithRange;


            if(useLineCast)
            {
            // old method ---------------------
                for(int c=0; c < 50; c++)
                {
                    randomDirection = playerPosRight * Random.Range(-maxRange, maxRange);
                    randomDirection += playerPosUp* Random.Range(0, maxRange);
                    // randomDirection += (Vector2) playerPosUp/4;
                    // randomDirection = randomDirection.normalized;
                    randomDirection /= Mathf.Sqrt(randomDirection.x*randomDirection.x + randomDirection.y*randomDirection.y);
                    RaycastHit2D _hit = Physics2D.Linecast(playerPos, (Vector2) playerPos+randomDirection*maxRange, layerOfObstacles);

                    NavMeshHit hit;
                    Vector2 positionNearOfThePlayer = (Vector2) playerPos + randomDirection * _hit.distance;
                    NavMesh.SamplePosition(positionNearOfThePlayer, out hit, 1000, NavMesh.AllAreas);
                    randomDirection = ((Vector2)hit.position-playerPos);
                    float hitDistance = randomDirection.magnitude;
                    float randomDistance = Random.Range(0, hitDistance);

                    if(randomDistance > minRange)
                    {
                        // Debug.DrawLine(playerPos, (Vector2) playerPos + randomDirection.normalized * randomDistance, Color.white, 10);
                        return (Vector2) playerPos + randomDirection.normalized * (randomDistance);
                    }
                }

                print("Adicionei a posição do player 2");
                return GetNearstPositionOnNavMesh(playerPos);

            }



        // new method ---------------------


            for(int c=0; c < 50; c++)
            {
                randomDirection = playerPosRight * Random.Range(-maxRange, maxRange);
                randomDirection += playerPosUp* Random.Range(0, maxRange);

                // randomDirection += (Vector2) playerPosUp/4;

                // randomDirection             = new Vector2(Random.Range(-1.0f, 1.0f), Random.Range(-1.0f, 1.0f));
                randomDirection            /= Mathf.Sqrt(randomDirection.x*randomDirection.x + randomDirection.y*randomDirection.y);
                randomDirectionWithRange    = randomDirection * Random.Range(0, maxRange);

                NavMeshHit hit;

                if (NavMesh.SamplePosition((Vector2) playerPos + randomDirectionWithRange, out hit, 1000, NavMesh.AllAreas))
                {

                    // Debug.DrawLine(playerPos, hit.position, Color.magenta, 10);
                    return hit.position;
                }

            }

            print("Adicionei a posição do player 1");
          // return player.position;
            return GetNearstPositionOnNavMesh(player.position);

            
    }


    protected List<Vector2> SortByClosest(List<Vector2> positions)
    {
        // return positions;

        List<Vector2> positionsSorted = new List<Vector2>();
        int count = positions.Count;
        Vector2 lastPosition = positions[0];
        Vector2 nearstPosition = positions[0];
        // positionsSorted.Add(lastPosition);
        // positions.Remove(lastPosition);


        while(positionsSorted.Count != count)
        {
            float minDistance = Mathf.Infinity;

            foreach(Vector2 oldPos in positions)
            {
                if((lastPosition-oldPos).magnitude < minDistance)
                {
                    minDistance     = (lastPosition-oldPos).magnitude;
                    nearstPosition  = oldPos;
                }
            }
            lastPosition = nearstPosition;
            positionsSorted.Add(lastPosition);
            positions.Remove(lastPosition);
        }

        // print("positionsSorted.Count: "+positionsSorted.Count);

        // for(int c=0; c<positionsSorted.Count; c++)
        // {
        //     print(positions[c]+"     ->    "+positionsSorted[c]);
        // }

        return positionsSorted;
    }


    protected Vector2 GetNearstPositionOnNavMesh(Vector2 position)
    {
        NavMeshHit hit;
        NavMesh.SamplePosition(position, out hit, 1000, NavMesh.AllAreas);
        return hit.position;
    }


    protected Vector3 GetNewRandomTargetPosition(bool toFace=false)
    {

        while(true)
        {
            Vector3 randomDirection = Random.insideUnitSphere * Random.Range(0, rangeOfRandomPosition);

            if(toFace)
            {
                randomDirection = transform.right * Random.Range(-rangeOfRandomPosition, rangeOfRandomPosition);
                randomDirection += transform.up * Random.Range(0, rangeOfRandomPosition);
            }

            randomDirection += transform.position;

            NavMeshHit hit;

            if (NavMesh.SamplePosition(randomDirection, out hit, 1000, NavMesh.AllAreas))
                return hit.position;
        }
    }

    protected void LookAt(Vector2 direction)
    {
        Quaternion toRotation = Quaternion.LookRotation(Vector3.forward, direction);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, toRotation, 1000*Time.deltaTime);
    }


    protected bool PlayerIsOnRange()
    {
        return ((Vector2)(player.position-transform.position)).magnitude <= visualDistance;
    }

    protected bool HasObstacleBetweenThePlayerAndMe()
    {
        return Physics2D.Linecast(transform.position, player.position, layerOfObstacles).distance != 0;
    }

    public void ClearLog()
    {
        var assembly = Assembly.GetAssembly(typeof(UnityEditor.Editor));
        var type = assembly.GetType("UnityEditor.LogEntries");
        var method = type.GetMethod("Clear");
        method.Invoke(new object(), null);
    }


    public Vector2 GetNearestLayerPositionInRadius(Vector2 origin, LayerMask layer, float radius=25)
    {
        List<RaycastHit2D> hits = new List<RaycastHit2D>(Physics2D.CircleCastAll(origin, radius, Vector2.zero, Mathf.Infinity, layer));
        
        if(hits.Count == 0)
            return Vector2.zero;

        hits = hits.OrderBy( x => Vector2.Distance(origin, x.transform.position)).ToList();
        // print(hits[0].point);
        Vector2 correctPoint = Physics2D.Linecast(origin, hits[0].point, layer).transform.position;    
        // Debug.DrawLine((Vector2) origin, correctPoint, Color.blue);

        return correctPoint;
    }


    public Vector2 GetOppositeDiretionOfPlayer()
    {
        Vector2 oppositeDirection = (transform.position-player.position).normalized; 
        return oppositeDirection;
    }


    public Vector2 GetOppositeDiretion(Vector2 position)
    {
        Vector2 oppositeDirection = ((Vector2)transform.position-position).normalized; 
        return oppositeDirection;
    }


    // void OnTriggerStay2D(Collider2D col)
    // {
    //     if(col.tag != player.tag)
    //         return;

    //     agent.SetDestination(col.transform.position);
    // }

    // void OnTriggerExit2D(Collider2D col)
    // {
    //     if(col.tag != player.tag)
    //         return;
            
    // }



}
