using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using System.Runtime.Serialization.Formatters.Binary;

public static class SaveSystem
{
    private static readonly string SAVE_FOLDER = Application.dataPath + "/Saves/";

    public static readonly PlayerData defaultPlayerData = new PlayerData
    {
        pos_x = 0,
        pos_y = 0,
        pos_z = 0,
        rot_x = 0,
        rot_y = 0,
        rot_z = 0,
        rot_w = 0,
        time = 0,
        sceneIndex = -1,
    };

    public static void Init()
    {
        if (!Directory.Exists(SAVE_FOLDER))
        {
            Directory.CreateDirectory(SAVE_FOLDER);
        }
    }

    public static void Save(string fileName, PlayerData fromData)
    {
        BinaryFormatter formatter = new BinaryFormatter();
        FileStream stream = new FileStream(SAVE_FOLDER + fileName + ".save", FileMode.Create);

        formatter.Serialize(stream, fromData);
        stream.Close();
    }

    public static PlayerData Load(string fileName)
    {
        string filePath = SAVE_FOLDER + fileName + ".save";
        if (File.Exists(filePath))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(filePath, FileMode.Open);

            PlayerData loadedPlayerData = (PlayerData)formatter.Deserialize(stream);
            stream.Close();
            return loadedPlayerData;
        }
        else
        {
            Debug.LogWarning("Could not find file \"" + filePath + "\", saving and returning default player data.");

            Save(fileName, defaultPlayerData);

            return defaultPlayerData;
        }
    }

    public static PlayerData LoadLatest()
    {
        FileInfo latestFile = new DirectoryInfo(SAVE_FOLDER).GetFiles("*.save", SearchOption.AllDirectories).OrderByDescending(f => f.LastWriteTime).FirstOrDefault();
        if(latestFile != null)
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = latestFile.Open(FileMode.Open);

            try
            {
                PlayerData loadedPlayerData = (PlayerData)formatter.Deserialize(stream);
                stream.Close();
                return loadedPlayerData;
            }
            catch(System.Exception e)
            {
                Debug.LogWarning("Failed to deserialize \"" + latestFile.Name + "\": " + e.Message);
                return null;
            }
        }
        return null;
    }

    public static void DeleteLatest()
    {
        FileInfo latestFile = new DirectoryInfo(SAVE_FOLDER).GetFiles("*.save", SearchOption.AllDirectories).OrderByDescending(f => f.LastWriteTime).FirstOrDefault();
        if (latestFile != null)
        {
            latestFile.Delete();
        }
    }

    public static bool HasSave()
    {
        return Directory.EnumerateFiles(SAVE_FOLDER, "*.save", SearchOption.AllDirectories).Any();
    }
}
