using NUnit.Framework;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

public class StorageManager : MonoBehaviour
{
    // Create a field for the save file.
    string saveFile;



    void Awake()
    {
        // Update the path once the persistent path exists.
        saveFile = Application.persistentDataPath + "/gamedata.json";

        // Make sure file exists
        if (!File.Exists(saveFile))
        {
            File.Create(saveFile);

        }
    }

    public PlayerData ReadData(int playerId)
    {
        // Does the file exist?
        //if (File.Exists(saveFile))
        //{
        // Read the entire file and save its contents.
        string fileContents = File.ReadAllText(saveFile);
        var gameData = JsonUtility.FromJson<PlayerDataList>(fileContents);
        if (gameData is null)
        {
            return new PlayerData();
        }
        PlayerData playerData = gameData.Players.Find(p => p.PlayerId == playerId);
        if (playerData is null)
        {
            return new PlayerData();
        }
        return playerData;
        //}
        //return new PlayerData();
    }

    public void WritePlayerData(int playerId, PlayerData data)
    {
        // Work with JSON
        data.PlayerId = playerId;
        var gameData = JsonUtility.FromJson<PlayerDataList>(File.ReadAllText(saveFile));
        if (gameData is not null)
        {


            PlayerData toUpdate = gameData.Players.Find(p => p.PlayerId == playerId);
            if (toUpdate is not null)
            {
                gameData.Players.Remove(toUpdate);
            }
            gameData.Players.Add(data);
        }
        else
        {
            gameData = new PlayerDataList();
            gameData.Players = new List<PlayerData>() { data};
        }


        string jsonString = JsonUtility.ToJson(gameData);

        // Write JSON to file.
        File.WriteAllText(saveFile, jsonString);
    }
}

