using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    public GameObject playerPrefabs;
    public Transform spawnPoint;


    public void SpawnPlayer()
    {
        GameObject playerPrefab = playerPrefabs;
        Instantiate(playerPrefab, spawnPoint.position, spawnPoint.rotation);

    }
}

