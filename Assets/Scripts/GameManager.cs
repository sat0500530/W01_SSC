using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    //public TextMeshPro lifeText;
    public TextMeshProUGUI lifeText;
    public GameObject endingCreditUI;

    public int life = 0;


    public bool isGameActive;
    public SpawnManager spawnManager;


    void Start()
    {
        spawnManager = GameObject.Find("SpawnManager").GetComponent<SpawnManager>();
        endingCreditUI.SetActive(false);

        isGameActive = true;
        spawnManager.SpawnPlayer();

    }


    void Update()
    {
        lifeText.text = "x" + life;
    }


}
