using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoToDestination : MonoBehaviour
{
    public Transform destination;
    public float speed;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        LookAt(destination.position - transform.position);
        MoveToDestination(destination.position);
    }



    void LookAt(Vector2 direction)
    {
        Quaternion toRotation = Quaternion.LookRotation(Vector3.forward, direction);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, toRotation, 1000*Time.deltaTime);
    }


    void MoveToDestination(Vector2 _destination)
    {
        transform.position = Vector2.MoveTowards(transform.position, _destination, speed*Time.deltaTime);

    }
}
