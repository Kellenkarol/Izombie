using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    public float speed=10;
    public Animator anim; 

    float x,y;

    // Start is called before the first frame update
    void Start()
    {
        // anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 direction = Vector3.ClampMagnitude(new Vector3(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"), 0), 1.0f);
        transform.position += direction * speed * Time.deltaTime;
        ChangeAnimation(direction);
    }


    Vector3 GetUnitary(Vector3 vector)
    {
        Vector3 newVector = vector;
        newVector.x = newVector.x > 0 ? Mathf.Ceil(newVector.x) : Mathf.Floor(newVector.x);
        newVector.y = newVector.y > 0 ? Mathf.Ceil(newVector.y) : Mathf.Floor(newVector.y);
        newVector.z = newVector.z > 0 ? Mathf.Ceil(newVector.z) : Mathf.Floor(newVector.z);

        return newVector;
    }


    void ChangeAnimation(Vector3 direction)
    {
        Vector3 unitaryDirection = GetUnitary(direction);

        if(unitaryDirection.x == 1)
        {
            anim.Play("WalkingToRight");
        }
        else if(unitaryDirection.x == -1)
        {
            anim.Play("WalkingToLeft");
        }
        else if(unitaryDirection.y == 1)
        {
            anim.Play("WalkingToUp");
        }
        else if(unitaryDirection.y == -1)
        {
            anim.Play("WalkingToDown");
        }
        else
        {
            anim.Play("Stoped");
        }


        

    }



}
