using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class PlayerController : MonoBehaviour
{
    [SerializeField] float EnemyKillJump;
    [SerializeField] float jumpForce;
    [SerializeField] float runSpeed;
    private int playerHealth;
    public int SceneEnemies_no;
    public int DeadEnemies;
    public float checkRadius;
    public int JumpsValue;
    private int Jumps;

    private ScreenShake shake;
    [SerializeField] Rigidbody2D rb;
    public AudioClip PlayerDamage;
    public AudioClip DeathSound;

    public Transform GroundCheck;
    public GameObject gibEffect;
    public GameObject Health_1;
    public GameObject Health_2;
    public GameObject Health_3;
    public GameObject _button;
    public GameObject Enemy;

    public GameObject Platforms;
    public GameObject PlayAgain;
    public GameObject Enemies;
    public GameObject Buttons;

    public Button Right;
    public Button Space;
    public Button Left;

    public LayerMask whatIsGround;
    private bool isGrounded;
    private Vector3 enemPos;

    void Start()
    {
        rb = this.GetComponent<Rigidbody2D>();
        enemPos = Enemy.transform.position;
        shake = GameObject.FindGameObjectWithTag("Screenshake").GetComponent<ScreenShake>();
        Jumps = JumpsValue;
        playerHealth = 6;
    }
    private void FixedUpdate()
    {
        isGrounded = Physics2D.OverlapCircle(GroundCheck.position, checkRadius, whatIsGround);
    }

    private void Update()
    {
        Damage();
        Movement();
        DoubleJump();
        if (isGrounded == true)
        {
            Jumps = JumpsValue;
        }
        if (Input.GetKey(KeyCode.Escape))
        {
            Application.Quit();
        } 
        
    }
    public void DoubleJump()
    {
        if (Input.GetKeyDown(KeyCode.Space) && Jumps > 0)
        {
            rb.velocity = Vector2.up * jumpForce;
            Jumps--;
        }
        else if (Input.GetKeyDown(KeyCode.Space) && Jumps == 0 && isGrounded == true)
        {
            rb.velocity = Vector2.up * jumpForce;
        }
    }

    public void Damage()
    {
        if (playerHealth == 4)
        {
            Destroy(Health_1);
        }
        if (playerHealth == 2)
        {
            Destroy(Health_2);
        }
        if (playerHealth == 0)
        {
            Platforms.SetActive(false);
            Enemies.SetActive(false);
            Buttons.SetActive(false);
            PlayAgain.SetActive(true);
            Destroy(Health_3);
        }
        if (DeadEnemies == SceneEnemies_no)
        {
            PlayAgain.SetActive(true);
        }
    }

    public void Movement()
    {
        if (Input.GetKey(KeyCode.A))
        {
            transform.position += Vector3.left * runSpeed * Time.deltaTime;
            Left.Select();
            Left.OnSelect(null);
        }
        
        if (Input.GetKey(KeyCode.D))
        {
            transform.position += Vector3.right * runSpeed * Time.deltaTime;
            Right.Select();
            Right.OnSelect(null);
        }

        if (Input.GetKey(KeyCode.Space))
        {
            Space.Select();
            Space.OnSelect(null);
        }
    }

    void EnemyDeathEffect()
    {
        AudioSource.PlayClipAtPoint(DeathSound, transform.position);
        Instantiate(gibEffect, transform.position, transform.rotation);
    }

    public void Play_Again()
    {
        Platforms.SetActive(true);
        Enemies.SetActive(true);
        Buttons.SetActive(true);
        PlayAgain.SetActive(false);
        SceneManager.LoadScene(0);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))    //if player collides with enemy on their head(1.0f contact point) then kill enemy, otherwise hurt playerand remove one heart
        {                                                //when player runs out of herats start the No_Heart coroutine
            foreach (ContactPoint2D point in collision.contacts)
            {
                Debug.Log(point.normal);
                if (point.normal.y >= 0.9f)
                {
                    rb.velocity = Vector2.up * EnemyKillJump;
                    DeadEnemies = DeadEnemies + 1;
                    print(DeadEnemies);
                    EnemyDeathEffect();
                    Destroy(collision.gameObject);
                }
                if(point.normal.x >= 0.8f)
                {
                    playerHealth = playerHealth - 1;
                    shake.CamShake();
                    AudioSource.PlayClipAtPoint(PlayerDamage, transform.position);
                    print(playerHealth);
                }
                else if(point.normal.x <= -0.8f)
                {
                    playerHealth = playerHealth - 1;
                    shake.CamShake();
                    AudioSource.PlayClipAtPoint(PlayerDamage, transform.position);
                    print(playerHealth);
                }
            }
        }
    }
    
}
