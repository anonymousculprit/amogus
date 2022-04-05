using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    static GameObject mainCamera;
    public float jumpForce = 100.0f;
    private Rigidbody rb3d;
    private CapsuleCollider collider3D;
    private float playerSpeed = 5.0f;

    void Start()
    {
        rb3d = GetComponent<Rigidbody>();
        collider3D = GetComponent<CapsuleCollider>();
        mainCamera = Camera.main.gameObject;
    }
    void Update()
    {
        PlayerRotation();
    }

    private void FixedUpdate()
    {
        PlayerMove();
    }

    void PlayerRotation()
    {
        //Get the movements input
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");

        //grab the vector3 of the input
        Vector3 movementInput = Quaternion.Euler(0, mainCamera.transform.eulerAngles.y, 0) * new Vector3(moveHorizontal, 0, moveVertical);
        Vector3 movementDirection = movementInput.normalized;
        if (movementDirection != Vector3.zero) 
        {
            Quaternion lookRotation = Quaternion.LookRotation(movementDirection, Vector3.up);
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, 10f * Time.deltaTime);
        }
    }

    void PlayerMove()
    {
        // This is the vector we will move towards
        // Remember to kill 'y' axis, and then re-normalize the vector.
        // This is so that we don't 'fly'
        Vector3 forward = mainCamera.transform.forward;
        forward.y = 0.0f;
        forward = forward.normalized;

        // Do the same for the right vector, for moving sideways.
        Vector3 right = mainCamera.transform.right;
        right.y = 0.0f;
        right = right.normalized;

        // We use GetAxisRaw to prevent the 'falloff' smooth filter.
        float horizontalInput = Input.GetAxisRaw("Horizontal");
        float verticalInput = Input.GetAxisRaw("Vertical");

        // Add forward + right to get the final movement on the xz-plane.
        Vector3 moveDirection = horizontalInput * right + verticalInput * forward;
        moveDirection = moveDirection.normalized;

        // Multiply movement direction with speed to move the amount we want
        moveDirection *= playerSpeed;

        // Set y-velocity to current y-velocity
        moveDirection.y = rb3d.velocity.y;

        // Apply xz-plane movement
        // We probably should move this to FixedUpdate
        //rb.AddForce(moveDir);
        rb3d.velocity = moveDirection;

        // Now we deal with jump
        // Convenient/Hacky way is to 'raycast' to the ground to see if we hit anything below us
        // Set "QueryTriggerInteraction.Ignore" to ignore trigger boxes (by default, it will not ignore them)
        // "-1" layer mask is to collide with all layer masks.
        bool isOnGround = Physics.Raycast(transform.position, Vector3.down, collider3D.height, -1, QueryTriggerInteraction.Ignore);
        Debug.Log(isOnGround);

        if (isOnGround && Input.GetButton("Jump"))
        {
            rb3d.AddForce(this.transform.up * jumpForce);
        }
    }

    public static void UpdateMainCamera(Camera cam) => mainCamera = cam.gameObject;
}
