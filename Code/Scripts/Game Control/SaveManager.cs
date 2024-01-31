using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SaveManager : MonoBehaviour
{
    [SerializeField] bool loadSaveOnStart;
    [SerializeField] float saveDelay = 30;
    SaveObject[] saveObjects;
    AmbianceControl ambiance;

    void Start()
    {
        saveObjects = FindObjectsOfType<SaveObject>();
        ambiance = FindObjectOfType<AmbianceControl>();

        if (loadSaveOnStart)
        {
            Load();
        }
        StartCoroutine(SaveLoop());
    }

    void Update()
    {
        
    }

    private void Load()
    {
        GameManager.instance.LoadLatestPlayerData();

        if (GameManager.instance.currentPlayerData != null)
        {
            if (SpaceEnvironment.instance != null)
                SpaceEnvironment.instance.timer = GameManager.instance.currentPlayerData.time;
            if (PlanetEnvironment.instance != null)
                PlanetEnvironment.instance.timer = GameManager.instance.currentPlayerData.time;

            foreach (SaveObject saveObject in saveObjects)
            {
                if(GameManager.instance.saveObjectInfoDict.TryGetValue(saveObject.saveID, out var saveObjectInfo))
                {
                    saveObject.LoadSave(saveObjectInfo);
                }
                else
                {
                    Debug.LogWarning("No info in dictionary for saveObject with id of " + saveObject.saveID);
                }
            }

            if (ambiance != null)
            {
                if (GameManager.instance.currentPlayerData.inCave)
                {
                    ambiance.EnterCave();
                }
                else if (GameManager.instance.currentPlayerData.inShelter)
                {
                    ambiance.EnterShelter();
                }
            }
        }
    }

    private void Save()
    {
        Scene activeScene = SceneManager.GetActiveScene();
        if (activeScene.buildIndex != 0)
        {
            GameManager.instance.UpdatePlayerData(saveObjects, activeScene.buildIndex);
            GameManager.instance.SavePlayerData();
        }
    }

    IEnumerator SaveLoop()
    {
        yield return new WaitForSecondsRealtime(0.1f);
        Save();
        yield return new WaitForSeconds(saveDelay);
        StartCoroutine(SaveLoop());
    }

    private void OnApplicationQuit()
    {
        Save();
    }
}
