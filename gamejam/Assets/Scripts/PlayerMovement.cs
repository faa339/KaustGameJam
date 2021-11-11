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

    [SerializeField]
    Transform groundCheck;

    [SerializeField]
    LayerMask ground;

    Rigidbody player;

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

    }

    bool IsGrounded()
    {
        return Physics.CheckSphere(groundCheck.position, .1f, ground);
    }
}

