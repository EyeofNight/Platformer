using UnityEngine;
using System.Collections;

public class PlayerMovement : MonoBehaviour {
    float jumpStrength = 8f;
    float moveSpeed = 5f;
    float airSpeed = 5f;
    float sprintMulti = 2f;
    float gravity = 9.81f;
    bool doubleJump = false;
    CharacterController controller;
    Vector3 moveDirection = Vector3.zero;

    void Start()
    {
        controller = GetComponent<CharacterController>();
    }

	void Update ()
    {
        if (controller.isGrounded)
        {
            doubleJump = false;
            moveDirection = new Vector3(Input.GetAxis("Horizontal"), 0, 0);

            if (Input.GetButton("Sprint"))
            {
                moveDirection *= (moveSpeed * sprintMulti);
            }
            else
                moveDirection *= moveSpeed;
            if (Input.GetButtonDown("Jump"))
                moveDirection.y = jumpStrength;
        }
        else
        {
            if (Input.GetButton("Horizontal"))
            {
                moveDirection += new Vector3(Input.GetAxis("Horizontal")*airSpeed, 0, 0)*Time.deltaTime;

            }
            if (Input.GetButtonDown("Jump") && !doubleJump)
            {
                moveDirection.y = jumpStrength;
                doubleJump = true;
            }

        }

        moveDirection.y -= gravity * Time.deltaTime;
        controller.Move(moveDirection * Time.deltaTime);
        
    }

    void OnControllerColliderHit(ControllerColliderHit col)
    {
        if (col.gameObject.tag == "Hazard")
            transform.position = new Vector3(0, 1, 0);
        if (col.gameObject.tag == "WinCollider")
            transform.position = new Vector3(0, 1, 0); //Need to change scenes later. Might attach a scenemanager to this or the finish line.

    }

}
