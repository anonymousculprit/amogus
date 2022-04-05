using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PLAYER : MonoBehaviour
{
    static GameObject mainCamera;
    public float jumpForce = 100.0f;
    public float playerSpeed = 5.0f;

    float inputHorizontal, inputVertical;
    float rotateHorizontal, rotateVertical;
    Rigidbody rb;
    CapsuleCollider col;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        col = GetComponent<CapsuleCollider>();
        mainCamera = Camera.main.gameObject;
    }
    void Update() => HandleRotation();

    private void FixedUpdate()
    {
        HandleMove();
        HandleJump();
    }

    void HandleRotation()
    {
        GetWASDInputSmooth();

        //grab movement direction.
        Vector3 movement = Quaternion.Euler(0, mainCamera.transform.eulerAngles.y, 0) * new Vector3(rotateHorizontal, 0, rotateVertical);
        Vector3 dir = movement.normalized;

        if (dir != Vector3.zero) 
        {
            Quaternion lookRotation = Quaternion.LookRotation(dir, Vector3.up);                             // turn character to face movement direction.
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, 10f * Time.deltaTime);  // smooth rotation process.
        }
    }

    // used in CAMERA_Trigger.
    // since movement is relative to main camera, when switching cameras in non-legacy mode,
    // change controls as well by updating main camera.
    public static void UpdateMainCamera(Camera cam) => mainCamera = cam.gameObject;

    void HandleMove()
    {
        // taken from gerald afaik.

        GetWASDInputRaw();

        Vector3 forward =   CleanVector(mainCamera.transform.forward);  // This is the vector we will move towards
        Vector3 right =     CleanVector(mainCamera.transform.right);    // Do the same for the right vector, for moving sideways.            

        // Add forward + right to get the final movement on the xz-plane.
        Vector3 moveDirection = inputHorizontal * right + inputVertical * forward;
        moveDirection = moveDirection.normalized;

        moveDirection *= playerSpeed;       // Multiply movement direction with speed to move the amount we want
        moveDirection.y = rb.velocity.y;  // Set y-velocity to current y-velocity
        rb.velocity = moveDirection;      // Apply xz-plane movement


    }
    void HandleJump()
    {
        // taken from gerald afaik.

        // Now we deal with jump
        // Convenient/Hacky way is to 'raycast' to the ground to see if we hit anything below us
        // Set "QueryTriggerInteraction.Ignore" to ignore trigger boxes (by default, it will not ignore them)
        // "-1" layer mask is to collide with all layer masks.
        bool isOnGround = Physics.Raycast(transform.position, Vector3.down, col.height, -1, QueryTriggerInteraction.Ignore);
        if (isOnGround && Input.GetButton("Jump")) rb.AddForce(transform.up * jumpForce);
    }
    void GetWASDInputRaw()
    {
        // We use GetAxisRaw to prevent the 'falloff' smooth filter.
        inputHorizontal = Input.GetAxisRaw("Horizontal");
        inputVertical = Input.GetAxisRaw("Vertical");
    }
    void GetWASDInputSmooth()
    {
        // want smoothing for rotation input specifically.
        rotateHorizontal = Input.GetAxis("Horizontal");
        rotateVertical = Input.GetAxis("Vertical");
    }
    Vector3 CleanVector(Vector3 v)
    {
        // Remember to kill 'y' axis, and then re-normalize the vector.
        // This is so that we don't 'fly'
        v.y = 0;
        v = v.normalized;
        return v;
    }
}
