using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public GameObject player;
    private float inputSensitivity = 3.0f;
    private float rotationX;
    private float rotationY;

    private float distanceFromPlayer = 3.0f;
    private Vector3 currentRotation;
    private Vector3 smoothness = Vector3.zero;
    private float smoothTime = 0.05f;

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
        //To keep the cursor in the centre
        Cursor.lockState = CursorLockMode.Locked;
        //Get the direction that the camera is facing?
        //dollyDirection = transform.localPosition.normalized;
        //distance = transform.localPosition.magnitude;
        //distance_ray = maxDistance - minDistance;
    }

    // Update is called once per frame
    void Update()
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
        //collisionCheck();
    }

    //void collisionCheck()
    //{
    //    //dollyDirection = player.transform.position - transform.parent.forward;

    //    //Set the cam's position according to the max distance
    //    Vector3 newCamPosition = transform.parent.TransformPoint(dollyDirection * maxDistance);
    //    RaycastHit hit;

    //    //cast a ray from the og camera position and wherever the max distance that it is set at is at.
    //    if (Physics.Linecast(transform.parent.position, newCamPosition, out hit))
    //    {
    //        //If there is a wall, close the distance from the collider
    //        distance = Mathf.Clamp((hit.distance * distance_ray), minDistance, maxDistance);
    //    }
    //    else
    //    {
    //        //Else remain at the max distance
    //        distance = maxDistance;
    //    }

    //    //transform.localPosition = Vector3.Lerp(transform.localPosition, dollyDirection * distance, Time.deltaTime * smooth);
    //}
}
