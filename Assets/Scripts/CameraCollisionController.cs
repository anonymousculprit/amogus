using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraCollisionController : MonoBehaviour
{
    public GameObject player;
    public float minDistance = 1.0f;
    public float maxDistance = 3.0f;
    public float smoothness = 10.0f;
    private Vector3 dollyDirection;
    public float distance;
    public float distance_ray;

    // Start is called before the first frame update
    void Start()
    {
        //Get the direction that the camera is facing?
        dollyDirection = transform.localPosition.normalized;
        distance = transform.localPosition.magnitude;
    }

    // Update is called once per frame
    void Update()
    {
        //Set the cam's position according to the max distance
        Vector3 newCamPosition = transform.parent.TransformPoint(dollyDirection * maxDistance);
        RaycastHit hit;

        //cast a ray from the og camera position and wherever the max distance that it is set at is at.

        Debug.DrawLine(player.transform.position, newCamPosition);

        if (Physics.Linecast(player.transform.position, newCamPosition, out hit))
        {
            //If there is a wall, close the distance from the collider
            distance = Mathf.Clamp((hit.distance * distance_ray), minDistance, maxDistance);
            Debug.Log(distance);
        }
        else
        {
            //Else remain at the max distance
            distance = maxDistance;
            Debug.Log(distance);
        }

        transform.localPosition = Vector3.Lerp(transform.localPosition, dollyDirection * distance, Time.deltaTime * smoothness);
        Debug.Log(dollyDirection * distance);
    }
}
