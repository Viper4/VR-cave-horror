using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public Dictionary<uint, PlayerData.SaveObjectInfo> saveObjectInfoDict;
    public PlayerData currentPlayerData;

    void Start()
    {
        if(instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
            SaveSystem.Init();
        }
    }

    public void LoadLatestPlayerData()
    {
        currentPlayerData = SaveSystem.LoadLatest();
        saveObjectInfoDict = new Dictionary<uint, PlayerData.SaveObjectInfo>();
        if (currentPlayerData != null)
        {
            foreach(PlayerData.SaveObjectInfo saveObjectInfo in currentPlayerData.saveObjectInfos)
            {
                saveObjectInfoDict.Add(saveObjectInfo.id, saveObjectInfo);
            }
            /*for (int i = 0; i < currentPlayerData.keys.Length; i++)
            {
                saveObjectInfoDict.Add(currentPlayerData.keys[i], currentPlayerData.values[i]);
            }*/
        }
    }

    public void UpdatePlayerData(SaveObject[] saveObjects, int sceneIndex)
    {
        if (currentPlayerData == null)
            currentPlayerData = SaveSystem.defaultPlayerData.Copy();

        currentPlayerData.saveObjectInfos = new PlayerData.SaveObjectInfo[saveObjects.Length];
        if(saveObjectInfoDict != null && saveObjectInfoDict.Count > 0)
        {
            for (int i = 0; i < saveObjects.Length; i++)
            {
                PlayerData.SaveObjectInfo saveObjectInfo = saveObjects[i].GetInfo();
                if (saveObjectInfoDict.ContainsKey(saveObjectInfo.id))
                    saveObjectInfoDict[saveObjectInfo.id] = saveObjectInfo;
                else
                    saveObjectInfoDict.Add(saveObjectInfo.id, saveObjectInfo);
                currentPlayerData.saveObjectInfos[i] = saveObjectInfo;
            }
        }
        else
        {
            saveObjectInfoDict = new Dictionary<uint, PlayerData.SaveObjectInfo>();
            for (int i = 0; i < saveObjects.Length; i++)
            {
                PlayerData.SaveObjectInfo saveObjectInfo = saveObjects[i].GetInfo();
                saveObjectInfoDict.Add(saveObjectInfo.id, saveObjectInfo);
                currentPlayerData.saveObjectInfos[i] = saveObjectInfo;
            }
        }

        if (PlanetEnvironment.instance != null)
            currentPlayerData.time = PlanetEnvironment.instance.timer;
        else if(SpaceEnvironment.instance != null)
            currentPlayerData.time = SpaceEnvironment.instance.timer;

        AmbianceControl ambiance = FindObjectOfType<AmbianceControl>();
        if (ambiance != null)
        {
            currentPlayerData.inShelter = ambiance.inShelter;
            currentPlayerData.inCave = ambiance.inCave;
        }

        currentPlayerData.sceneIndex = sceneIndex;
    }

    public void SavePlayerData()
    {
        if(currentPlayerData != null)
            SaveSystem.Save("PlayerData", currentPlayerData);
    }
}
