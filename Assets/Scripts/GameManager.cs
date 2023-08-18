using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{

    public bool isGameActive;
    public SpawnManager spawnManager;


    void Start()
    {
        spawnManager = GameObject.Find("SpawnManager").GetComponent<SpawnManager>();

        isGameActive = true;
        spawnManager.SpawnPlayer();

    }


    void Update()
    {

    }



}
