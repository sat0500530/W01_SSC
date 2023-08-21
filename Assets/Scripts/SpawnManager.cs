using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    public GameManager gameManager;

    public GameObject playerPrefabs;
    public Transform spawnPoint;

    private void Start()
    {
        gameManager = GameObject.Find("GameManger").GetComponent<GameManager>();
    }
    public void SpawnPlayer()
    {
        gameManager.life++;
        GameObject playerPrefab = playerPrefabs;
        Instantiate(playerPrefab, spawnPoint.position, spawnPoint.rotation);

    }
}

