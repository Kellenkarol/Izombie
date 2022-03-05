using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Player : MonoBehaviour
{

    public string tagOfHuman  = "Human";
    private int attackDamage  = 1;

    [Header("Horder -----------------------------------------------------------")]
    public bool attack;

    public int  removeAt;
    public bool remove;

    public int maxZombiesInHorde;

    public int hordeSize=9;
    public float distanceAux=1;
    public GameObject zombiePrefab;
    
    public Vector2 distanceFromEachOneInTheHorde = Vector2.one;

    public List<GameObject> myHordeInList = new List<GameObject>();
    private List<Vector2>    myHordePositionsInList = new List<Vector2>();
    private List<SimpleAI>    myHordeScriptInList = new List<SimpleAI>();

    private Vector2[]    myHordePositions;
    private GameObject[] myHorde         ;

    float currentAngle = 0;

    // Start is called before the first frame update
    void Start()
    {
        hordeSize = (int) PegarRaizMaisProxima(hordeSize);
        hordeSize *= hordeSize;
        if(hordeSize > 1)
                hordeSize--;
        // myHordePositionsInList.Add(Vector2.zero);
        myHordePositions = new Vector2[hordeSize];
        myHorde          = new GameObject[hordeSize];
        StartCoroutine("KeepAddingZombie");
        // StartCoroutine("KeepMovingHorde");
        // StartCoroutine("KeepRotatingZombies");
        // UpdateHordePosition();
        // CreateHorde();
    }

    // Update is called once per frame
    void Update()
    {
        // print(UpdateRadiusCorrectDistance(new Vector2(-2, 2)));
        // print(UpdateRadiusCorrectDistance(new Vector2(-2, 1)));
        // print(GetAngleOfDirection(new Vector2(-2, 2)));
        // UpdateHordePosition();
        if(remove)
        {
            remove = false;
            RemoveZombieFromHorde(myHordeInList[removeAt]);
        }

        if(!attack)
            MoveHorde();
        
        // int quant = 26;
        // print(((PegarRaizMaisProxima(quant)-1)*4));
        return;
    }

    IEnumerator KeepMovingHorde()
    {
        while(true)
        {      
            // yield return new WaitForSeconds(0.5f);
            if(!attack)
            {
                for(int c=0; c<myHordeInList.Count; c++)
                {
                    try
                    {
                        if(myHordeInList[c]!=null)
                        {
                            Vector2 centerOffset = Vector2.zero;
                            Vector2 dir = myHordePositionsInList[c]*distanceFromEachOneInTheHorde;

                            // float oldAngle = GetAngleOfDirection(dir) + currentAngle / Mathf.Ceil(((PegarRaizMaisProxima(c+2)-1)*4)/8);
                            // float newAngle = (360+Mathf.Round(oldAngle))%360;
                            // dir = GetDirectionOfAngle(newAngle, 1)*Mathf.Ceil(((PegarRaizMaisProxima(c+2)-1)*4)/8)*distanceFromEachOneInTheHorde;
                            myHordeScriptInList[c].SetAgentDestination(centerOffset+(Vector2)transform.position+dir);
                            // myHordeInList[c].GetComponent<SimpleAI>().SetAgentDestination(centerOffset+(Vector2)transform.position+dir);
                        }
                    }
                    catch
                    {

                    }
                    // yield return null;
                }

            }
            yield return null;
        }
    }

    void MoveHorde()
    {
        for(int c=0; c<myHordeInList.Count; c++)
        {
            try
            {
                if(myHordeInList[c]!=null)
                {
                    Vector2 centerOffset = Vector2.zero;
                    Vector2 dir = myHordePositionsInList[c];

                    float oldAngle = GetAngleOfDirection(dir) + currentAngle / Mathf.Ceil(((PegarRaizMaisProxima(c+2)-1)*4)/8);
                    float newAngle = (360+Mathf.Round(oldAngle))%360;
                    dir = GetDirectionOfAngle(newAngle, 1)*Mathf.Ceil(((PegarRaizMaisProxima(c+2)-1)*4)/8)*distanceFromEachOneInTheHorde;
                    myHordeScriptInList[c].SetAgentDestination(centerOffset+(Vector2)transform.position+dir);
                    // myHordeInList[c].GetComponent<SimpleAI>().SetAgentDestination(centerOffset+(Vector2)transform.position+dir);
                }
            }
            catch
            {

            }
        }

        return;

        for(int c=0; c<myHorde.Length; c++)
        {
            if(myHorde[c]!=null)
            {
                // Vector2 centerOffset = new Vector2(distanceFromEachOneInTheHorde.x/2, distanceFromEachOneInTheHorde.y/2);
                Vector2 centerOffset = Vector2.zero;
                // if(hordeSize%2 == 0)
                //     centerOffset = distanceFromEachOneInTheHorde/2;
                myHorde[c].GetComponent<SimpleAI>().SetAgentDestination(centerOffset+(Vector2)transform.position+myHordePositions[c+1]);
            }
        }
    }


    IEnumerator KeepAddingZombie()
    {
        for(int c=0; c<hordeSize; c++)
        {
            AddNewZombieOnHorde(Instantiate(zombiePrefab));
            // yield return new WaitForSeconds(0.05f);
            yield return null;
        }
    }


    IEnumerator KeepRotatingZombies()
    {
        while(true)
        {
            // for(int c=0; c<myHordePositionsInList.Count; c++)
            // {
            //     float oldAngle = GetAngleOfDirection(myHordePositionsInList[c])-1;
            //     float newAngle = (360+Mathf.Round(oldAngle))%360;
            //     myHordePositionsInList[c] = GetDirectionOfAngle(oldAngle, myHordePositionsInList[c].magnitude);

            //     yield return new WaitForSeconds(0.1f);
            //     // yield return null;
            // }
            currentAngle+=10;
            yield return new WaitForSeconds(0.1f);
        }
    }




    void OnTriggerEnter2D(Collider2D col)
    {
        if(col.tag == tagOfHuman)
        {
            col.GetComponent<SimpleAI>().TakeDamage(attackDamage);
        }
    }


    public void AddNewZombieOnHorde(GameObject zombie)
    {
        myHordeInList.Add(zombie);
        myHordeScriptInList.Add(zombie.GetComponent<SimpleAI>());
        // Vector2 direction = GetDirectionOfAngle(myAngle, borderSize);
        myHordePositionsInList.Add(Vector2.up*distanceFromEachOneInTheHorde * 5);

        CalculateTheCorrectPositionOfBoard(); //atualiza a posição dinamicamente

        return;
        int idx = GetIndexOfNearestEmptyPosition();
        myHorde[idx] = zombie;
    }
  

    public void RemoveZombieFromHorde(GameObject zombie)
    {
        int idx = myHordeInList.IndexOf(zombie);
        DestroyImmediate(myHordeInList[idx]);
        myHordeInList.RemoveAt(idx);
        myHordePositionsInList.RemoveAt(idx);

        // CalculateTheCorrectPositionOfBoard();
        UpdateAllHordePosition();


        return;

        for(int c=0; c<myHorde.Length; c++)
        {
            if(myHorde[c] != null)
            {
                if(myHorde[c] == zombie)
                {
                    myHorde[c] = null;
                    return;
                }
            }
        }
    }


    void UpdateAllHordePosition(int startIndex=1)
    {
        int rings              = NumberOfRings();
        int ringOfCurrentIndex = NumberOfRings(startIndex);

        // UpdateSingleRing(1);

        // return;

        for(int c=ringOfCurrentIndex; c<=rings; c++)
        {
            UpdateSingleRing(c);
        }

    }

    int ZombiesUntilThisRing(int ring)
    {
        return ((ring*2)+1)*((ring*2)+1)-1;
    }


    void UpdateSingleRing(int ring)
    {
        string txt="";

        ring                            = NumberOfRings() > ring ? ring : NumberOfRings();
        int zombiesUntilThisRing        = ZombiesUntilThisRing(ring);
        txt += $"ring {ring} \n";
        txt += $"zombiesUntilThisRing {zombiesUntilThisRing} \n";
        int zombiesCount                = myHordeInList.Count;
        txt += $"zombiesCount {zombiesCount} \n";
        int zombiesCountInCurrentRing   = SizeOfRing(ring);
        txt += $"old zombiesCountInCurrentRing {zombiesCountInCurrentRing} \n";

        if(zombiesCount < zombiesUntilThisRing)
            zombiesCountInCurrentRing   = zombiesCount - ZombiesUntilThisRing(ring-1);

        txt += $"new zombiesCountInCurrentRing {zombiesCountInCurrentRing} \n";
        txt += $"ZombiesUntilThisRing(ring-1) {ZombiesUntilThisRing(ring-1)} \n";

        float currentAngleOfOne = 360/(zombiesCountInCurrentRing+0.00000001f); 
        print(txt);
// print($"NumberOfRings() {NumberOfRings()} | zombiesUntilThisRing() {zombiesUntilThisRing()} | zombiesUntilThisRing() {zombiesUntilThisRing()}")
        for(int c=0; c<zombiesCountInCurrentRing; c++)
        {
            float myAngle                 = c*currentAngleOfOne;
            Vector2 direction             = GetDirectionOfAngle(myAngle, SizeOfRing(ring)).normalized*ring;
            int idx                       = (ZombiesUntilThisRing(ring-1))+c;
            if(myHordePositionsInList.ElementAtOrDefault(idx) != null)
            {
                print(idx);
                myHordePositionsInList[idx]   = direction;
            }
            else
            {
                myHordePositionsInList.Add(direction);
            }
        }

    }


    int NumberOfRings(int index=-1)
    {
        int zombiesCount = myHordeInList.Count;
        
        if(index >= 0)
            zombiesCount = index;

        return (int) ((PegarRaizMaisProxima(zombiesCount) - 1) / 2);

    }

    int SizeOfRing(int ring)
    {
        return (int)(ring*8);
    }


    void CalculateTheCorrectPositionOfBoard()
    {
        int zombiesCount                = myHordeInList.Count;
        int raizUp                      = (int) PegarRaizMaisProxima(zombiesCount+1);
        int raizDown                    = (int) PegarRaizMaisProxima(zombiesCount, true);
        int borderSize                  = (raizUp-1)*4;    

        int zombiesCountInCurrentRadius = (zombiesCount+1) - raizDown * raizDown; 
        // print($"zombiesCount  {zombiesCount} \n raizUp \\ {raizUp} \n raizDown  {raizDown} \n borderSize  {borderSize} \n zombiesCountInCurrentRadius  {zombiesCountInCurrentRadius}");

        float currentAngleOfOne = 360/(zombiesCountInCurrentRadius+0.00000001f); 

        for(int c=0; c<zombiesCountInCurrentRadius; c++)
        {
            float myAngle                 = c*currentAngleOfOne;
            Vector2 direction             = GetDirectionOfAngle(myAngle, borderSize).normalized*((raizUp-1)/2);
            int idx                       = (zombiesCount-zombiesCountInCurrentRadius)+c;
            if(myHordePositionsInList.ElementAtOrDefault(idx) != null)
            {

                myHordePositionsInList[idx]   = direction;
            }
            else
            {
                myHordePositionsInList.Add(direction);
            }
        }
    }




    int GetIndexOfNearestEmptyPosition()
    {
        int idx = 0;
        float minDistance = Mathf.Infinity;

        for(int c=0; c<myHordePositions.Length; c++)
        {
            double distance = Mathf.Max(Mathf.Abs(myHordePositions[c].x), Mathf.Abs(myHordePositions[c].y));
            if(myHorde[c] == null && distance < minDistance && distance > 0)
            {
                minDistance = Mathf.Max(Mathf.Abs(myHordePositions[c].x), Mathf.Abs(myHordePositions[c].y));
                idx         = c;
            }
        }

        return idx;
    }


    void UpdateHordePosition()
    {
        int size = (int) PegarRaizMaisProxima(hordeSize);
        // print(size);
        if(size == 0)
            return;

        for(int c=0; c<(int)(size*size); c++)
        {
            float row = c%size;
            float col = c/size;

            float rowOffset = (float)(row - size / 2);
            float colOffset = (float)(col - size / 2);

            // if(size % 2 == 0)
            // {
            //     rowOffset += 0.5f;
            //     colOffset += 0.5f;
            // }
            
            // rowOffset = rowOffset > 0 ? Mathf.Ceil(rowOffset) : Mathf.Floor(rowOffset);
            // if(row >= size/2)
            // {
            //     rowOffset += 1;
            // }
            // if(col >= size/2)
            // {
            //     colOffset += 1;
            // }

            Vector2 posInHorde = new Vector2(rowOffset, colOffset);
            // if(Mathf.Abs(rowOffset) > Mathf.Abs(colOffset))
            // {
            //     rowOffset += (Mathf.Abs(rowOffset) - Mathf.Abs(colOffset))*(rowOffset/Mathf.Abs(rowOffset));
            //     // colOffset /= (colOffset/Mathf.Abs(colOffset))*-2f;
            // }
            // else if(Mathf.Abs(rowOffset) < Mathf.Abs(colOffset))
            // {
            //     colOffset += (Mathf.Abs(colOffset) - Mathf.Abs(rowOffset))*(colOffset/Mathf.Abs(colOffset));
            //     // rowOffset /= (rowOffset/Mathf.Abs(rowOffset))*-2f;
            // }
            // else
            // {

            // }

            // posInHorde        *= distanceFromEachOneInTheHorde;
            // float distance     = posInHorde.magnitude;
            // if(rowOffset == 2 && colOffset == 1)
                posInHorde = UpdateRadiusCorrectDistance(posInHorde);
            // posInHorde = Vector2.ClampMagnitude(posInHorde, Mathf.Max(Mathf.Abs(rowOffset), Mathf.Abs(colOffset)));
            // if(Mathf.Min(Mathf.Abs(rowOffset), Mathf.Abs(colOffset)) != 0 && Mathf.Abs(rowOffset) != Mathf.Abs(colOffset))
            // {

            // }


            myHordePositions[c] = posInHorde * distanceFromEachOneInTheHorde;
            // print(posInHorde);
        }

        // if(size * size < myHordePositions.Length)
        // {
        //     for(int c=size*size; c < myHordePositions.Length; c++)
        //     {

        //     }    
        // }
    }


    void CreateHorde()
    {
        for(int c=0; c<myHorde.Length; c++)
        {
            if(myHordePositions[c].magnitude > 0.001f)
            {
                AddNewZombieOnHorde(Instantiate(zombiePrefab));
                // myHorde[c] = Instantiate(zombiePrefab);
            } 
            // else if(myHordePositions[c].magnitude > 0.001f)
            // {
            //     myHorde[c] = Instantiate(zombiePrefab);
            // }
        }
    }



    float PegarRaizMaisProxima(int num, bool floor=false)
    {
        while(true)
        {
            if(( (Mathf.Sqrt(num)*Mathf.Sqrt(num) == num) && (num % 2 != 0) ) || num == 0)
                return Mathf.Sqrt(num);

            num += floor ? -1 : 1;
        }
        // for(int c=num; c>=0; c--)
        // {
        //     if(Mathf.Sqrt(c)*Mathf.Sqrt(c)==c)
        //     {
        //         return Mathf.Sqrt(c);
        //     }
        // }
    }


    void SortZombieToNearestPosition()
    {

    }


    float GetAngleOfDirection(Vector2 direction)
    {
        float angle = Mathf.Atan2(direction.y, direction.x);
        float degrees = 180*angle/Mathf.PI;
        // return degrees;
        return (360+Mathf.Round(degrees))%360;
    }


    Vector2 GetDirectionOfAngle(float angle, float radius)
    {
        // var radius = 60;
        // var angle  = 140;
        float x = radius * Mathf.Sin(Mathf.PI * 2 * angle / 360);
        float y = radius * Mathf.Cos(Mathf.PI * 2 * angle / 360);
        return new Vector2(x, y);
    }


    Vector2 UpdateRadiusCorrectDistance(Vector2 vector)
    {
        Vector2 retorno = vector;
        float maxAxis            = Mathf.Max(Mathf.Abs(retorno.x), Mathf.Abs(retorno.y));
        int borderSize           = (int)((maxAxis*2)*4);

        borderSize = borderSize == 0 ? 1 : borderSize; 

        float currentAngleOfOne  = 360/(borderSize+0.00000001f); 
        float myAngle            = GetAngleOfDirection(retorno);
        float myAngleUpdated     = myAngle - myAngle % currentAngleOfOne;

        // if(currentAngleOfOne != 45)

        if(myAngle - myAngleUpdated > currentAngleOfOne/2)
            myAngleUpdated += currentAngleOfOne;
        

        Vector2 correctDirection = GetDirectionOfAngle(myAngleUpdated, maxAxis*2);

        print($"correctDirection {correctDirection} | vector {vector} | currentAngleOfOne {currentAngleOfOne} | myAngle {myAngle} | myAngleUpdated {myAngleUpdated}");
        retorno = correctDirection;

        return retorno;
    }

    // encontrar angulo
    //     this.getAngle = function(){
    //     var angle = Math.atan2(this.y, this.x);   //radians
    //     // you need to devide by PI, and MULTIPLY by 180:
    //     var degrees = 180*angle/Math.PI;  //degrees
    //     return (360+Math.round(degrees))%360; //round number, avoid decimal fragments
    // }


    /* encontrar direção do angulo
    var radius = 60;
    var angle  = 140;
    var x = radius * Math.sin(Math.PI * 2 * angle / 360);
    var y = radius * Math.cos(Math.PI * 2 * angle / 360);
    console.log('Points coors are  x='+ 
       Math.round(x * 100) / 100 +', y=' +
       Math.round(y * 100) / 100)

    */


}
