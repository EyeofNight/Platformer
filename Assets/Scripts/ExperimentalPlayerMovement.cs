using UnityEngine;
using System.Collections;

public class ExperimentalPlayerMovement : MonoBehaviour {

    private float walkSpeed = 5f; //Default movement speed.
    private float runSpeed = 10f; //Speed while running.
    private float crchSpeed = 2.5f; //Movement speed while crouched.
    private float speed = 0f; //Current desired movement speed
    private float jumpStr = 25000f; //How much force is applied in a jump.
    private float accel = 100000f; //Higher means less drift, lower means more drift.
    private float airAccel = 50000f; //Same as accel, but for air movement.
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
        if (Input.GetAxis("Horizontal") > 0 && rb.velocity.x <= speed)
        {
            rb.AddForce((airAccel * Time.deltaTime), 0, 0);
        }
        if (Input.GetAxis("Horizontal") < 0 && rb.velocity.x >= -speed)
        {
            rb.AddForce((-airAccel * Time.deltaTime), 0, 0);
        }
        if (Input.GetButtonDown("Jump") && !doubleJump)
        {
            rb.AddForce(0, jumpStr, 0);
            doubleJump = true;
        }
    }

    void Movement()
    {
        if (Input.GetButton("Sprint"))
            speed = runSpeed;
        else if (Input.GetAxis("Vertical") < 0)
            speed = crchSpeed;
        else
            speed = walkSpeed;

        if (isGrounded)
        {
            doubleJump = false;
            if (Input.GetAxis("Horizontal") > 0 && rb.velocity.x <= speed)
            {
                rb.AddForce((accel*Time.deltaTime), 0, 0);
            }
            if (Input.GetAxis("Horizontal") < 0 && rb.velocity.x >= -speed)
            {
                rb.AddForce((-accel * Time.deltaTime), 0, 0);
            }
            if (Input.GetButtonDown("Jump"))
                rb.AddForce(0, jumpStr, 0);
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

        }
    }

    void OnCollisionExit()
    {
        isGrounded = false;
    }
}
