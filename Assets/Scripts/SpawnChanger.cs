using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnChanger : MonoBehaviour
{
    private GameObject spawnPoint;
    private Vector2 changePoint;
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            spawnPoint = GameObject.Find("SpawnPoint");
            spawnPoint.transform.position = this.transform.position;
            Destroy(gameObject);
        }
    
    }
}
