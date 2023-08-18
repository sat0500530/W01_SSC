using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;


public class PlayerController : MonoBehaviour
{

    public float moveSpeed = 5f;
    public float jumpForce = 5f;
    public float maxVelocity = -30f;
    private float maxFallingDistance = 30f;
    private float fallingDistance = 0f;


    public CameraManager cameraManager;
    private GameManager gameManager;
    private Rigidbody2D rb;
    private SpawnManager spawnManager;
    private GameObject vCam;


    private Vector2 fallingStart;

    //float previousVelocityY;
    private bool isOnGround = false;
    private bool isDie = false;

    private Renderer objectRenderer;
    private Color initialColor;
    private Color targetColor = Color.red;

    private void Start()
    {
        cameraManager = GameObject.Find("Main Camera").GetComponent<CameraManager>();
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        spawnManager = GameObject.Find("SpawnManager").GetComponent<SpawnManager>();
        rb = GetComponent<Rigidbody2D>();

        objectRenderer = GetComponent<Renderer>();
        initialColor = objectRenderer.material.color;

        vCam = GameObject.Find("CM vcam1");

}

    private void Update()
    {
        Debug.Log("속도는 =" + rb.velocity);
        Debug.Log("낙하 거리는 = " + fallingDistance);
        Debug.Log("낙하 시작 지점은 = " + fallingStart.y);
        Debug.Log("현재 땅인지 ? = " + isOnGround);


        if (!isOnGround)
        {
            fallingDistance = fallingStart.y - this.transform.position.y;

            if (fallingDistance > maxFallingDistance)
            {
                Die();
            }
        }

        vCam.GetComponent<CinemachineVirtualCamera>().Follow = this.transform;
        cameraManager.SetTarget(transform.position);

        //점프 제거
        //if (!isOnGround && Mathf.Abs(rb.velocity.y) < 0.0001f && previousVelocityY < 0)
        //{
        //    isOnGround = true;
        //}

        if (gameManager.isGameActive && !isDie)
        {
            float moveX = Input.GetAxis("Horizontal");
            Vector2 movement = new Vector2(moveX * moveSpeed, rb.velocity.y);
            rb.velocity = movement;

            rb.AddTorque(-moveX, ForceMode2D.Force);

            Debug.Log(moveX);

            if (rb.velocity.y < maxVelocity)
            {
                rb.velocity = new Vector2(rb.velocity.x, maxVelocity);
            }

            //점프 제거
            //if (Input.GetKeyDown(KeyCode.Space) && isOnGround)
            //{
            //    rb.AddForce(new Vector2(0f, jumpForce), ForceMode2D.Impulse);
            //    isOnGround = false;
            //}

            if (Input.GetKeyDown(KeyCode.LeftShift) || Input.GetKeyDown(KeyCode.X))
            {
                ChangeToPlatform();
                Destroy(this);
            }

            if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                transform.Rotate(Vector3.forward * 90);
            }


            if (Input.GetKeyDown(KeyCode.DownArrow))
            {
                transform.Rotate(Vector3.forward * -90);
            }
        }


        // Calculate the percentage of falling distance relative to maxFallingDistance
        float percentage = Mathf.Clamp01(fallingDistance / maxFallingDistance);

        // Interpolate between initialColor and targetColor based on the percentage
        Color newColor = Color.Lerp(initialColor, targetColor, percentage);

        // Update the object's Material color property
        objectRenderer.material.color = newColor;

        //previousVelocityY = rb.velocity.y;

    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Ground")
        {
            isOnGround = true;
            fallingStart.y = 0f;
            fallingDistance = 0f;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        this.GetComponent<Animator>().SetBool("IsFalling", true);
        isOnGround = false;
        fallingStart = this.transform.position;
    }

    private void Die()
    {
        this.GetComponent<Animator>().SetBool("IsDie", true);
        rb.velocity = Vector2.zero;
        isDie = true;
    }

    private void DieRespawn()
    {
        spawnManager.SpawnPlayer();
        Destroy(gameObject);
    }

    private void ChangeToPlatform()
    {
        // player 자리에 platform prefab 소환 후, player 오브젝트 제거, 그리고 player 소환
        Destroy(rb);
        spawnManager.SpawnPlayer();
    }
}
