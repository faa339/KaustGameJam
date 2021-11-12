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

    private Camera cam;

    [SerializeField]
    Transform groundCheck;

    [SerializeField]
    LayerMask ground;

    Rigidbody player;

    [SerializeField]
    private float dashSpeed = 100f;

    [SerializeField]
    private float dashCooldown = 0f;

    [SerializeField]
    public bool canSlow = false;

    [SerializeField]
    public int backPack = 0;

    [SerializeField]
    public float healthTimer = 300f;

    [SerializeField]
    public float powerTimer = 6f;

    private AudioClip timeTick;

    private float fixedDeltaTime;

    private void Awake()
    {
        this.fixedDeltaTime = Time.fixedDeltaTime;
    }
    // Start is called before the first frame update
    void Start()
    {
        player = GetComponent<Rigidbody>();

    }

    // Update is called once per frame
    void Update()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");


        player.velocity = new Vector3(horizontalInput * playerSpeed, player.velocity.y, verticalInput * playerSpeed);

        if (Input.GetButtonDown("Jump") && IsGrounded())
        {
            player.AddForce(new Vector3(player.velocity.x, jumpForce, player.velocity.z), ForceMode.Impulse);
        }

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

        if (healthTimer > 0)
        {
            healthTimer -= Time.deltaTime;
        }

        if (Input.GetKeyDown(KeyCode.LeftControl) && powerTimer>0)
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

    private void OnGUI()
    {
        GUI.Box(new Rect(20, 20, 250, 120), "Player Stats");
        GUI.TextArea(new Rect(25, 50, 220, 20), "Time left: " + healthTimer);
        GUI.TextArea(new Rect(25, 80, 220, 20), "Slow Time Timer: " + powerTimer);
    }

    bool IsGrounded()
    {
        return Physics.CheckSphere(groundCheck.position, .1f, ground);
    }

    void slowTime()
    {
        Time.timeScale = 0.5f;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Projectile")
        {
            healthTimer -= 2.0f;
        }
        else if (collision.gameObject.tag == "Enemy")
        {
            healthTimer -= 4.0f;
        }
        else if (collision.gameObject.tag == "Item")
        {
            powerTimer += 6.0f;
        }
    }
}



