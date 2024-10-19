using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class LevelManager : MonoBehaviour {
    static public LevelManager I;
    public Transform player;
    public GameObject[] levelPrefabs;
    public Transform activeParent;
    public Transform deactiveParent;
    public int initialPoolSize = 3;
    private Transform lastSpawnedPlatform;
    public bool bFirstSpawn = true;
    

    void Start() {
        I = this;
        activeParent.GetChild(0).position = player.transform.position;
        //Loading all levels.
        for (int i = 0; i < levelPrefabs.Length; i++) {
            Instantiate(levelPrefabs[i], Vector3.zero, Quaternion.identity, deactiveParent);
        }
        //Activating a specific number of platforms in the scene.
        for (int i = 0; i < initialPoolSize; i++) {
            SpawnPlatform();
        }

        bFirstSpawn = false;
    }
    //Finding the deactive platform.
    private Transform GetPlatformFromPool() {
        for (int i = 0; i < deactiveParent.childCount; i++) {
            return deactiveParent.GetChild(0);
        }
        GameObject newPlatform = Instantiate(levelPrefabs[Random.Range(0, levelPrefabs.Length)], Vector3.zero, Quaternion.identity, deactiveParent);
        return newPlatform.transform;
    }
    public void SpawnPlatform() {
        Transform platform = GetPlatformFromPool();
        platform.SetParent(activeParent);
        float angle = 0;
        //Rotation change towards the platform direction.
        if (platform.tag == "ForwardGround") angle = 0;
        else if (platform.tag == "RightGround") angle = 90;
        else if (platform.tag == "LeftGround") angle = -90;
        //Spawning based on the pivot point.
        Vector3 spawnPosition = activeParent.GetChild(activeParent.childCount - 2).GetChild(1).position;
        //Determining the platform's rotation based on its direction.
        float determineRotation = activeParent.GetChild(activeParent.childCount - 2).rotation.eulerAngles.y;
        Vector3 rot = new Vector3(0, determineRotation + angle, 0);

        for (int i = 0; i < platform.GetChild(3).childCount; i++) {
            GameObject gold = platform.GetChild(3).GetChild(i).gameObject;
            gold.SetActive(true);
        }
        
        //To ensure that there are no currencies in the initial spawn.
        if (bFirstSpawn) 
            platform.GetChild(3).gameObject.SetActive(false);
        
        else 
            platform.GetChild(3).gameObject.SetActive(true);
        
        platform.transform.position = spawnPosition;
        platform.transform.rotation = Quaternion.Euler(rot);
        lastSpawnedPlatform = platform;
    }
    //Deactivating the active platform
    public void RemoveOldPlatform() {
        Transform oldPlatform = activeParent.GetChild(1);
        oldPlatform.SetParent(deactiveParent);
        int randomIndex = Random.Range(0, deactiveParent.childCount);
        oldPlatform.SetSiblingIndex(randomIndex);
    }

    public Transform currentPlatform(int index) {
        return activeParent.GetChild(index+1);
    }
}
