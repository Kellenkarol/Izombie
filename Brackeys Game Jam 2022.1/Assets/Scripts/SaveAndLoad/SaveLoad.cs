using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public enum FileType{Weapon, Item, GameData};

public class SaveLoad
{
    private static string saveGamePath = Application.persistentDataPath+"/SaveGame";

    public static void Save<T>(T data, string name)
    {

        if(!Directory.Exists(saveGamePath))
        {
            Directory.CreateDirectory(saveGamePath);
            Debug.LogWarning("O diretório '" + saveGamePath +"' não existia, mas acabou de ser criado" );
        }
        
        string path = $"{saveGamePath}/{name}.data";
        // path += $"/{typeof(T)}.data";
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = new FileStream(path, FileMode.Create);
        bf.Serialize(file, data);
        file.Close();        
        Debug.Log("Salvamento feito em: " + path);
    }


    public static T Load<T>(string name)
    {
        string path = $"{saveGamePath}/{name}.data";
     
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
