using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpdateSortingOrder : MonoBehaviour
{
    public Renderer myRenderer;

    // Start is called before the first frame update
    void Start()
    {
        if(myRenderer == null)
            myRenderer = GetComponent<Renderer>();    
    }

    // Update is called once per frame
    void Update()
    {
        UpdateRendererSorter();
    }

    protected void UpdateRendererSorter()
    {
        int sortingOrderBase = 5000;
        myRenderer.sortingOrder = (int)(sortingOrderBase - transform.position.y);
        transform.position = new Vector3(transform.position.x,transform.position.y, (sortingOrderBase + transform.position.y)/ 1000);
    }
}
