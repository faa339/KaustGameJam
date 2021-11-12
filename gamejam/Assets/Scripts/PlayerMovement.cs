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
    public float powerTimer = 0f;

    private AudioClip timeTick;

    // Start is called before the first frame update
    void Start()
    {
        player = GetComponent<Rigidbody>();

        //    cam = Camera.main;
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
    }

    //private void OnGUI()
    //{
    //    Vector3 point = new Vector3();
    //    Event currentEvent = Event.current;
    //    Vector2 mousePos = new Vector2();

    //    mousePos.x = currentEvent.mousePosition.x;
    //    mousePos.y = currentEvent.mousePosition.y;

    //    point = cam.ScreenToViewportPoint(new Vector3(mousePos.x, mousePos.y, cam.nearClipPlane));

    //    GUILayout.BeginArea(new Rect(20, 20, 250, 120));
    //    GUILayout.Label("Screen pixels: " + cam.pixelWidth + ":" + cam.pixelHeight);
    //    GUILayout.Label("Mouse position: " + mousePos);
    //    GUILayout.Label("World position: " + point.ToString("F3"));
    //    GUILayout.EndArea();
    //}

    bool IsGrounded()
    {
        return Physics.CheckSphere(groundCheck.position, .1f, ground);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Bullet")
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

