using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class LevelManager : MonoBehaviour {
    static public LevelManager I;
    public GameObject[] levelPrefabs;
    public Transform activeParent;
    public Transform deactiveParent;
    public int initialPoolSize = 3;
    private Transform lastSpawnedPlatform;
    

    void Start() {
        I = this;
        
        for (int i = 0; i < levelPrefabs.Length; i++) {
            Instantiate(levelPrefabs[i], Vector3.zero, Quaternion.identity, deactiveParent);
        }

        for (int i = 0; i < initialPoolSize; i++) {
            SpawnPlatform();
        }
    }
    private Transform GetPlatformFromPool() {
        for (int i = 0; i < deactiveParent.childCount; i++) {
            return deactiveParent.GetChild(0);
        }
        GameObject newPlatform = Instantiate(levelPrefabs[Random.Range(0, levelPrefabs.Length)], Vector3.zero, Quaternion.identity, deactiveParent);
        return newPlatform.transform;
    }
    public void SpawnPlatform()
    {
        Transform platform = GetPlatformFromPool();
        platform.SetParent(activeParent);
        float angle = 0;
        if (platform.tag == "ForwardGround") angle = 0;
        else if (platform.tag == "RightGround") angle = 90;
        else if (platform.tag == "LeftGround") angle = -90;
        Vector3 spawnPosition = activeParent.GetChild(activeParent.childCount - 2).GetChild(1).position;
        float determineRotation = activeParent.GetChild(activeParent.childCount - 2).rotation.eulerAngles.y;
        Vector3 rot = new Vector3(0, determineRotation + angle, 0); 
        
        platform.transform.position = spawnPosition;
        platform.transform.rotation = Quaternion.Euler(rot);
        lastSpawnedPlatform = platform;
    }
    public void RemoveOldPlatform()
    {
        Transform oldPlatform = activeParent.GetChild(1);
        oldPlatform.SetParent(deactiveParent);
        int randomIndex = Random.Range(0, deactiveParent.childCount);
        oldPlatform.SetSiblingIndex(randomIndex);
    }

    public Transform currentPlatform(int index) {
        return activeParent.GetChild(index+1);
    }
}
