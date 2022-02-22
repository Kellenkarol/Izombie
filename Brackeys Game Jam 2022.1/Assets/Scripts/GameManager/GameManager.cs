using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;


public class GameManager : MonoBehaviour
{
    private static GameManager _instance;
    public static GameManager Instance
    {
        get
        {
            if (_instance == null)
                _instance = new GameObject().AddComponent<GameManager>();
            return _instance;
        }
    }
    
    private static Dictionary<string, GameObject> characters = new Dictionary<string, GameObject>();


    // Start is called before the first frame update
    void Start()
    {
        if(_instance)
            DestroyImmediate(this.gameObject);

        _instance = this;
        Setcharacters();
        DontDestroyOnLoad(this.gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    
    // todos o GameObjects em gameObj vão ter sua layer alterada
    public static void SetLayerOfGameObjectAndChilds(GameObject gameObj, int layer)
    {
        gameObj.layer = layer;

        foreach(Transform child in gameObj.transform)
        {
            SetLayerOfGameObjectAndChilds(child.gameObject, layer);
        }
    }


    private void Setcharacters()
    {
        var loadedObjects = Resources.LoadAll("Prefabs", typeof(GameObject)).Cast<GameObject>();

        foreach(var go in loadedObjects)
        {
            characters[go.name] = go.gameObject;
                print(go.name);
        }
    }


    public static GameObject GetCharacterGameObject(string nameOfCharacter)
    {
        return characters[nameOfCharacter];
    }


    public static SimpleAI GetSimpleAIOfCharacter(string nameOfCharacter)
    {
        return characters[nameOfCharacter].GetComponent<SimpleAI>();
    }


    public static Dictionary<string, GameObject> GetAllCharacters()
    {
        return characters != null ? characters : new Dictionary<string, GameObject>();
    }
}
