using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public GameObject player;
    private float inputSensitivity = 3.0f;
    private float rotationX;
    private float rotationY;

    public float distanceFromPlayer = 0.0f;
    private Vector3 currentRotation;
    private Vector3 smoothness = Vector3.zero;
    private float smoothTime = 0.05f;


    public CameraCollisionController cameraCollider;
    public bool lockedCamera = false;

    bool transitioning = false;

    ////For collisions
    //public float minDistance = 1.0f;
    //public float maxDistance = 3.0f;
    //public float smooth = 10.0f;
    //float distance, distance_ray;

    //private Vector3 dollyDirection;
    //private float dollyDirectionAdj;

    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update()
    {
        FreeCam();
    }

    void FreeCam()
    {
        //Get the mouse x and y axis 
        float rotateHorizontal = -Input.GetAxis("Mouse X") * inputSensitivity;
        float rotateVertical = -Input.GetAxis("Mouse Y") * inputSensitivity;

        //Everytime you move the mouse, increment the value
        rotationY -= rotateHorizontal;
        rotationX += rotateVertical;

        //To prevent the up and down angles going all the way, clamp the value down
        rotationX = Mathf.Clamp(rotationX, -40, 40);

        //Let the camera rotate, with the help of smoothdamp to make the rotation smooth
        Vector3 nextRotation = new Vector3(rotationX, rotationY, 0);
        currentRotation = Vector3.SmoothDamp(currentRotation, nextRotation, ref smoothness, smoothTime);
        transform.localEulerAngles = currentRotation;

        //Set the camera at the player's transform so that it will orbit around the player instead
        //Multiply by the distance so that the camera isn't stuck on the player
        transform.position = player.transform.position - transform.forward * distanceFromPlayer;
    }
}

