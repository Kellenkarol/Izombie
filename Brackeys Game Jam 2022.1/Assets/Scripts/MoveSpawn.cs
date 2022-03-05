using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveSpawn : MonoBehaviour
{

    /*
        O QUE FAZ:
            Movimenta GameObject dentro do espaço da camera, na vertical ou horizontal, 
            não importando a posição da camera ou o seu tamanho.


        COMO USAR:
            - Definir 'Projection' da camera para 'Orthographic'
            - Criar um canvas contendo os spawns, e defina o seu 'Render Mode' para 'Screen Space - Camera' 
            - Adicione esse script em cada spawn
    */


    public bool vertical;
    public float movementTime;
    public int startDirection;

    float[] maxDistanceX = new float[2];
    float[] maxDistanceY = new float[2];
    float distance;

    // Start is called before the first frame update
    void Start()
    {
        startDirection = startDirection > 0 ? 1 : -1;
        StartCoroutine("KeepUpdatingMaxDistances");
    }


    // Update is called once per frame
    void Update()
    {

        transform.position += (vertical?transform.up:transform.right)*startDirection*distance*Time.deltaTime/movementTime;

        startDirection = (vertical ? 
                                (startDirection == -1 ? 
                                    transform.position.y < maxDistanceY[1] 
                                    :
                                    transform.position.y > maxDistanceY[0]) 
                                : 
                                (startDirection == -1 ? 
                                    transform.position.x < maxDistanceX[0] 
                                    : 
                                    transform.position.x > maxDistanceX[1])) 
                                ? (int)(startDirection*-1) : startDirection ;
    }


    IEnumerator KeepUpdatingMaxDistances()
    {
        while(true)
        {
            maxDistanceX[0] = Camera.main.ScreenToWorldPoint(Vector2.zero).x;
            maxDistanceX[1] = Camera.main.ScreenToWorldPoint(new Vector2(Screen.width, Screen.height)).x;
            maxDistanceY[0] = Camera.main.ScreenToWorldPoint(new Vector2(Screen.width, Screen.height)).y;
            maxDistanceY[1] = Camera.main.ScreenToWorldPoint(Vector2.zero).y;

            distance = vertical ? maxDistanceY[0]-maxDistanceY[1] : maxDistanceX[1]-maxDistanceX[0];

            yield return new WaitForSeconds(0.1f);
        }
    }

}
