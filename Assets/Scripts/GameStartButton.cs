using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameStartButton : MonoBehaviour
{
    public bool goToTitle = false;
    private void Awake()
    {
        Screen.SetResolution(1920, 1080, true);
    }
    void Start()
    {
        GetComponent<Button>().onClick.AddListener(GameStart);
    }

    void GameStart()
    {
        SceneManager.LoadScene(goToTitle ? "Title" : "InGame");
    }
}
