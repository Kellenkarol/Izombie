using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class a : MonoBehaviour
{
    public ScriptableOfCharacter[] scriptables;
    public ScriptableOfCharacter scriptableEnemy;

    // Start is called before the first frame update
    void Start()
    {
        foreach(ScriptableOfCharacter scriptable in scriptables)
        {
            print(scriptable.Name());
        }

        print(scriptableEnemy.groupType);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
