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

    float previousVelocityY;
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

        Debug.Log("���� �Ÿ��� = " + fallingDistance);
        Debug.Log("���� ���� ������ = " + fallingStart.y);
        Debug.Log("���� ������ ? = " + isOnGround);

        vCam.GetComponent<CinemachineVirtualCamera>().Follow = this.transform;
        cameraManager.SetTarget(transform.position);

        if (!isOnGround && !isDie)
        {
            fallingDistance = fallingStart.y - this.transform.position.y;

            //���ϰŸ� ��� �� ��ȯ
            float percentage = Mathf.Clamp01(fallingDistance / maxFallingDistance);
            Color newColor = Color.Lerp(initialColor, targetColor, percentage);
            objectRenderer.material.color = newColor;

            if (fallingDistance > maxFallingDistance)
            {
                Die();
            }
        }






        if (gameManager.isGameActive && !isDie)
        {
            Debug.Log("�ӵ��� =" + rb.velocity);

            if (!isOnGround && Mathf.Abs(rb.velocity.y) < 0.0001f && previousVelocityY < 0)
            {
                isOnGround = true;
            }

            float moveX = Input.GetAxis("Horizontal");
            Vector2 movement = new Vector2(moveX * moveSpeed, rb.velocity.y);
            rb.velocity = movement;

            rb.AddTorque(-moveX / 20, ForceMode2D.Force);

            if (rb.velocity.y < maxVelocity)
            {
                rb.velocity = new Vector2(rb.velocity.x, maxVelocity);
            }

            //����
            if (Input.GetKeyDown(KeyCode.Space) && isOnGround)
            {
                rb.AddForce(new Vector2(0f, jumpForce), ForceMode2D.Impulse);
                isOnGround = false;
            }

            if (Input.GetKeyDown(KeyCode.LeftShift) || Input.GetKeyDown(KeyCode.X))
            {
                ChangeToPlatform();
                Destroy(this);
            }

            previousVelocityY = rb.velocity.y;
        }




    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Ground")
        {
            isOnGround = true;
            fallingStart.y = 0f;
            fallingDistance = 0f;
        }
        if (collision.gameObject.tag == "Wall")
        {
            Die();
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
        objectRenderer.material.color = Color.red;
        Destroy(rb);
        isDie = true;
    }

    private void DieRespawn()
    {
        spawnManager.SpawnPlayer();
        Destroy(gameObject);
    }

    private void ChangeToPlatform()
    {
        Destroy(rb);
        objectRenderer.material.color = Color.white;
        spawnManager.SpawnPlayer();
    }
}