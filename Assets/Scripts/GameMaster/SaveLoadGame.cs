using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

public static class SaveLoadGame
{

    public static List<GameData> savedGames = new List<GameData>();

    //it's static so we can call it from anywhere
    public static void Save()
    {
        SaveLoadGame.savedGames.Add(GameData.current);
        BinaryFormatter bf = new BinaryFormatter();
        //Application.persistentDataPath is a string, so if you wanted you can put that into debug.log if you want to know where save games are located
        FileStream file = File.Create(Application.persistentDataPath + "/savedGames.gd"); //you can call it anything you want
        bf.Serialize(file, SaveLoadGame.savedGames);

        // Debug.Log(SaveLoadGame.savedGames.GetType().FullName);
        // object datasObj = SaveLoadGame.savedGames;
        // Debug.Log(datasObj.GetType().FullName);
        // List<GameData> test = datasObj as List<GameData>;
        // Debug.Log(test);

        file.Close();
    }

    public static void Load()
    {
        if (File.Exists(Application.persistentDataPath + "/savedGames.gd"))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + "/savedGames.gd", FileMode.Open);
            // Debug.Log(file);
            SaveLoadGame.savedGames = (List<GameData>)bf.Deserialize(file);
            // object datasObj = bf.Deserialize(file);
            // Debug.Log(datasObj.GetType().FullName);
            // List<GameData> test = datasObj as List<GameData>;
            // Debug.Log(test);

            // GameData test2 = (List<GameData>)datasObj;
            // Debug.Log(test2);
            // Debug.Log(SaveLoadGame.savedGames);
            file.Close();
        }
    }
}
