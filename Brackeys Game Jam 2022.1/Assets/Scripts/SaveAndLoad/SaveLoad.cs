using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public enum FileType{Weapon, Item, GameData};

public class SaveLoad
{
    private static string saveGamePath = Application.persistentDataPath+"/SaveGame";

    public static void SaveGame<T>(T data, string name)
    {
        string path = $"{saveGamePath}/{name}";

        if(!Directory.Exists(path + "/" + name))
        {
            Directory.CreateDirectory(path);
            Debug.LogWarning("O diretório '" + path +"' não existia, mas acabou de ser criado" );
        }
        
        path += "/"+typeof(T).ToString()+".data";
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = new FileStream(path, FileMode.Create);
        bf.Serialize(file, data);
        file.Close();        
        Debug.Log("Salvamento feito em: " + path);
    }


    public static T LoadGame<T>(string name)
    {
        string path = $"{saveGamePath}/{name}/{typeof(T)}.data";
     
        if (!File.Exists(path))
        {
            Debug.LogWarning("Salvamento não encontrando em: " + path);
            return default(T);
        }

        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = new FileStream(path, FileMode.Open);

        T data = (T) bf.Deserialize(file);

        file.Close();

        return data;
        
    }



}
