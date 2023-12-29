using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    PlayerData currentPlayerData;

    void Start()
    {
        if(instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
            SaveSystem.Init();
            currentPlayerData = SaveSystem.defaultPlayerData.Copy();
        }
    }

    public PlayerData LoadLatestPlayerData()
    {
        currentPlayerData = SaveSystem.LoadLatest();
        return currentPlayerData;
    }

    public void UpdatePlayerData(Vector3 pos, Quaternion rot, int sceneIndex)
    {
        currentPlayerData.pos_x = pos.x;
        currentPlayerData.pos_y = pos.y;
        currentPlayerData.pos_z = pos.z;

        currentPlayerData.rot_x = rot.x;
        currentPlayerData.rot_y = rot.y;
        currentPlayerData.rot_z = rot.z;
        currentPlayerData.rot_w = rot.w;

        if(PlanetEnvironment.instance != null)
            currentPlayerData.time = PlanetEnvironment.instance.timer;
        else if(SpaceEnvironment.instance != null)
            currentPlayerData.time = SpaceEnvironment.instance.timer;

        currentPlayerData.sceneIndex = sceneIndex;
    }

    public void SavePlayerData()
    {
        Debug.Log("Saved player data");
        SaveSystem.Save("PlayerData", currentPlayerData);
    }

    public Vector3 GetLoadedPosition()
    {
        return new Vector3(currentPlayerData.pos_x, currentPlayerData.pos_y, currentPlayerData.pos_z);
    }

    public Quaternion GetLoadedRotation()
    {
        return new Quaternion(currentPlayerData.rot_x, currentPlayerData.rot_y, currentPlayerData.rot_z, currentPlayerData.rot_w);
    }

    public float GetLoadedTime()
    {
        return currentPlayerData.time;
    }

    public int GetLoadedSceneIndex()
    {
        return currentPlayerData.sceneIndex;
    }
}
