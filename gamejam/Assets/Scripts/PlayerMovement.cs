using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    //Player speed to move forward
    [SerializeField]
    private float playerSpeed = 10f;

    //Player jump force
    [SerializeField]
    private float jumpForce = 8f;

    //Jump setup
    [SerializeField]
    Transform groundCheck;

    [SerializeField]
    LayerMask ground;

    //Player body
    Rigidbody player;


    //Dash setup
    [SerializeField]
    private float dashSpeed = 100f;

    [SerializeField]
    private float dashCooldown = 0f;

    //can stop Time??
    [SerializeField]
    public bool canSlow = false;
    public float powerTimer = 6f;

    //number of Items carried
    [SerializeField]
    public int backPack = 0;

    //Timer to death
    [SerializeField]
    public float healthTimer = 300f;



    GameObject Item;

    private AudioClip timeTick;

    private float fixedDeltaTime;

    float smooth = 5.0f;
    float tiltAngle = 45.0f;

    //Start Location
    private Vector3 start;



    private void Awake()
    {
        this.fixedDeltaTime = Time.fixedDeltaTime;
    }

    //Start of game
    void Start()
    {
        player = GetComponent<Rigidbody>();
        start = new Vector3(0, 3, 0);
    }

    // Update is called once per frame
    void Update()
    {
        //player Movement variables
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        float tiltLeft = verticalInput * tiltAngle;
        float tiltRight = horizontalInput * tiltAngle;

        float powerT = powerTimer;

        player.velocity = new Vector3(horizontalInput * playerSpeed, player.velocity.y, verticalInput * playerSpeed);

        //Jump
        if (Input.GetButtonDown("Jump") && IsGrounded())
        {
            player.AddForce(new Vector3(player.velocity.x, jumpForce, player.velocity.z), ForceMode.Impulse);
        }

        //Rotate Right
        if (Input.GetKey(KeyCode.E) && player.transform.rotation.y <= 45)
        {
            Quaternion tiltL = Quaternion.Euler(0, tiltLeft, 0);
            transform.rotation = Quaternion.Slerp(transform.rotation, tiltL, Time.deltaTime * smooth);
        }

        //Rotate Left
        if (Input.GetKey(KeyCode.Q) && player.transform.rotation.y >= -45)
        {
            Quaternion tiltR = Quaternion.Euler(0, tiltRight, 0);
            transform.rotation = Quaternion.Slerp(transform.rotation, tiltR, Time.deltaTime * smooth);
        }


        //Respawn
        if (Input.GetKeyDown(KeyCode.T))
        {
            player.transform.position = start;
        }

        //Dash Setup
        if (Input.GetKeyDown(KeyCode.LeftShift) && dashCooldown <= 0f)
        {
            player.velocity = new Vector3(horizontalInput * dashSpeed, player.velocity.y, verticalInput * dashSpeed);
            dashCooldown = 3f;
        }
        if (dashCooldown > 0)
        {
            dashCooldown -= Time.deltaTime;
            Debug.Log(dashCooldown);
        }

        //Timer to Death
        if (healthTimer > 0)
        {
            healthTimer -= Time.deltaTime;
        }

        //Use Slow Time by clicking left Control
        if (Input.GetKeyDown(KeyCode.LeftControl) && powerT > 0)
        {
            slowTime();
        }
        if (Time.timeScale < 1)
        {
            powerTimer -= Time.deltaTime;
        }
        if (powerTimer <= 0)
        {
            Time.timeScale = 1f;
        }

    }


    //Setup life and power bar
    private void OnGUI()
    {
        GUI.Box(new Rect(20, 20, 250, 120), "Player Stats");
        GUI.TextArea(new Rect(25, 50, 220, 20), "Time left: " + healthTimer);
        GUI.TextArea(new Rect(25, 80, 220, 20), "Slow Time Timer: " + powerTimer);
    }

    //Grounded Checking
    bool IsGrounded()
    {
        return Physics.CheckSphere(groundCheck.position, .1f, ground);
    }

    //Ability
    void slowTime()
    {
        Time.timeScale = 0.5f;
    }



    private void OnCollisionEnter(Collision collision)
    {
        //Damage done by Projectile
        if (collision.gameObject.tag == "Projectile")
        {
            healthTimer -= 2.0f;
        }
        //Damage done by Enemy
        else if (collision.gameObject.tag == "Enemy")
        {
            healthTimer -= 4.0f;
        }
        //picking up Item
        else if (collision.gameObject.tag == "Item")
        {
            Item.SetActive(false);
            powerTimer += 6.0f;
        }
    }
}

