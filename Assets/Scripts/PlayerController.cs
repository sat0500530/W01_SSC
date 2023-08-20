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

    public float cutBoxX = 1.5f;
    public float cutBoxY = 1.3f;

    //float previousVelocityY;
    private bool isOnGround = false;
    private bool isOnStone = false;
    private bool isDie = false;

    private Renderer objectRenderer;
    private Color initialColor;
    private Color targetColor = Color.red;

    public new ParticleSystem particleSystem;


    private void Start()
    {
        cameraManager = GameObject.Find("Main Camera").GetComponent<CameraManager>();
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        spawnManager = GameObject.Find("SpawnManager").GetComponent<SpawnManager>();
        rb = GetComponent<Rigidbody2D>();

        objectRenderer = GetComponent<Renderer>();
        initialColor = objectRenderer.material.color;

        vCam = GameObject.Find("CM vcam1");

        particleSystem = GetComponent<ParticleSystem>();

}

    private void Update()
    {

        //Debug.Log("���� �Ÿ��� = " + fallingDistance);
        //Debug.Log("���� ���� ������ = " + fallingStart.y);
        //Debug.Log("���� ������ ? = " + isOnGround);

        //ī�޶� �����ǰ� �ѱ��
        vCam.GetComponent<CinemachineVirtualCamera>().Follow = this.transform;
        cameraManager.SetTarget(transform.position);

        //���� ��
        if (!isOnGround && !isDie)
        {
            Falling();

            //Debug.Log("���� �Ÿ� = " + fallingDistance);
            //Debug.Log("���� ���� y�� = " + fallingStart.y);

            fallingDistance = fallingStart.y - this.transform.position.y;

            //���ϰŸ� ��� �� ��ȯ
            ColorChange();

            if (fallingDistance > maxFallingDistance)
            {
                Die();
            }
        }

        if (isOnGround && !isDie)
        {
            OnGround();
        }


        //ĳ���� �̵�
        if (gameManager.isGameActive && !isDie)
        {
            //Debug.Log("�ӵ��� =" + rb.velocity);

            if (!isOnGround && Mathf.Abs(rb.velocity.y) < 0.0001f && previousVelocityY < 0)
            {
                isOnGround = true;
            }

            float moveX = Input.GetAxis("Horizontal");
            Vector2 movement = new Vector2(moveX * moveSpeed, rb.velocity.y);
            rb.velocity = movement;

            //ȸ��
            rb.AddTorque(-moveX / 20, ForceMode2D.Force);

            //�ӵ� ����
            if (rb.velocity.y < maxVelocity)
            {
                rb.velocity = new Vector2(rb.velocity.x, maxVelocity);
            }

            //����
            if (Input.GetKey(KeyCode.Space) && isOnGround)
            {
                rb.AddForce(new Vector2(0f, jumpForce), ForceMode2D.Impulse);
                isOnGround = false;
            }
            //����
            if (Input.GetKeyDown(KeyCode.X) && !isOnStone)
            {
                ChangeToPlatform();
                Destroy(this);
            }

            //���� ���� ����
            if (Input.GetKeyDown(KeyCode.C))
            {
                Debug.Log("1");
                Vector2 boxSize = new (cutBoxX, cutBoxY);
                Collider2D[] colliders = Physics2D.OverlapBoxAll(transform.position, boxSize, 0f);

                foreach (Collider2D collider in colliders)
                {
                    Debug.Log("2");
                    if (collider.CompareTag("Player") && collider.gameObject != gameObject)
                    {
                        Debug.Log("3");
                        Destroy(collider.gameObject);
                    }
                }

            }

            previousVelocityY = rb.velocity.y;
        }

    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Ground" || collision.gameObject.tag == "Player")
        {
            OnGround();
        }
        if (collision.gameObject.tag == "Wall")
        {
            Die();
        }

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {

        if (collision.gameObject.tag == "Obstacle")
        {
            Die();
        }

        if (collision.gameObject.tag == "Stone")
        {
            isOnStone = true;
        }


    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Ground" || collision.gameObject.tag == "Player")
        {
            Falling();
            fallingStart = this.transform.position;

        }

    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Stone")
        {
            isOnStone = false;
        }

    }

    private void Die()
    {
        particleSystem.Play();
        this.GetComponent<SpriteRenderer>().enabled = false;
        this.GetComponent<Animator>().SetBool("IsDie", true);
        rb.velocity = Vector2.zero;
        //objectRenderer.material.color = Color.red;
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
        Destroy(this);
    }

    private void OnGround()
    {
        isOnGround = true;
        objectRenderer.material.color = Color.white;
        this.GetComponent<Animator>().SetTrigger("IsIdle");
        fallingStart.y = 0f;
        fallingDistance = 0f;

    }

    private void Falling()
    {
        this.GetComponent<Animator>().SetTrigger("IsFalling");
        isOnGround = false;
    }

    private void ColorChange()
    {
        float percentage = Mathf.Clamp01(fallingDistance / maxFallingDistance);
        Color newColor = Color.Lerp(initialColor, targetColor, percentage);
        objectRenderer.material.color = newColor;
    }
}
