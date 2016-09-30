using UnityEngine;
using System.Collections;

public class VelocityBased : MonoBehaviour {

    private float walkSpeed = 5f; //Default movement speed.
    private float runSpeed = 10f; //Speed while running.
    private float crchSpeed = 2.5f; //Movement speed while crouched.
    private float speed = 0f; //Current desired movement speed
    private float jumpStr = 10f; //Velocity of a jump.
    private float accel = 15f; //Higher means less drift, lower means more drift.
    private float airAccel = 8f; //Same as accel, but for air movement.
    private float angleTol = 45f; //Maximum angle to allow'isgrounded' to be true.
    private float at; //Stores calculated angle tolerance sin function to reduce cpu use.
    //private float speedMultiplier = 1f; //Placeholder for future multiplier.
    private bool isGrounded = false;
    private bool doubleJump = false;
    private bool inAir = false;
    private Rigidbody rb;
	
	void Start () {
        rb = GetComponent<Rigidbody>();
        at = Mathf.Sin(angleTol);
	}
	
	
	void Update () {
        Movement();
	}

    void Crouch()
    {

    }

    void InAir()
    {
        rb.drag = 0.1f;
        if (Input.GetAxis("Horizontal") > 0 && rb.velocity.x <= walkSpeed)
        {
            rb.velocity += new Vector3(airAccel, 0,0)*Time.deltaTime;
        }
        if (Input.GetAxis("Horizontal") < 0 && rb.velocity.x >= -walkSpeed)
        {
            rb.velocity += new Vector3(-airAccel, 0,0)*Time.deltaTime;
        }
        if (Input.GetButtonDown("Jump") && !doubleJump)
        {
            rb.velocity = new Vector3(rb.velocity.x, jumpStr, rb.velocity.z);
            doubleJump = true;
        }
    }

    void WallJump(int dir)
    {
        if (Input.GetButtonDown("Jump"))
        {
            rb.velocity = new Vector3(.5f * jumpStr * dir, .7f * jumpStr, rb.velocity.z);
        }
    }

    void Movement()
    {
        if (Input.GetButton("Sprint"))
            speed = runSpeed;
        else if (Input.GetAxis("Vertical") < 0)
            speed = crchSpeed;
        else if (Input.GetButton("Horizontal"))
            speed = walkSpeed;
        else
            speed = 0;

        if (isGrounded)
        {
            doubleJump = false;
            if (Input.GetAxis("Horizontal") > 0 && rb.velocity.x <= speed)
            {
                rb.velocity += new Vector3(accel, 0, 0)*Time.deltaTime;
            }
            if (Input.GetAxis("Horizontal") < 0 && rb.velocity.x >= -speed)
            {
                rb.velocity += new Vector3(-accel, 0, 0)*Time.deltaTime;
            }
            if (Input.GetButtonDown("Jump"))
                rb.velocity = new Vector3(rb.velocity.x, jumpStr, rb.velocity.z);
            if (!Input.GetButton("Horizontal"))
                rb.drag = 10f;
            else
                rb.drag = 0.1f;
            Crouch();

        }else
            InAir();
    }

    void OnCollisionEnter(Collision col)
    {
        foreach (ContactPoint contact in col.contacts)
        {
            if (contact.normal.y >= at)
                isGrounded = true;
        }
    }

    void OnCollisionStay(Collision col)
    {
        foreach (ContactPoint contact in col.contacts)
        {
            if (contact.normal.y >= at)
                isGrounded = true;
            if (contact.normal.x > 0 && isGrounded == false)
                WallJump(1);
            if (contact.normal.x < 0 && isGrounded == false)
                WallJump(-1);
        }
    }

    void OnCollisionExit()
    {
        isGrounded = false;
    }
}
