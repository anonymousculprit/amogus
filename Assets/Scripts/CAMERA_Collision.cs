using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CAMERA_Collision : MonoBehaviour
{
    public float minDistance = 1.0f;
    public float maxDistance = 3.0f;
    
    float smoothness = 10.0f;
    float distance, distance_ray;
    Vector3 dollyDirection;
    GameObject player;
    
    void Start()
    {
        dollyDirection = transform.localPosition.normalized;    // get direction that camera is facing.
        distance = transform.localPosition.magnitude;           // grab current distance.
        player = GameObject.FindGameObjectWithTag("Player");
    }
    void Update()
    {
        CheckForCameraCollisions();
        ApplyCollisionZoom();
        
        void CheckForCameraCollisions()
        {
            Vector3 newCamPosition = transform.parent.TransformPoint(dollyDirection * maxDistance); // grab camera position as where it wants to be.

            RaycastHit hit;
            if (Physics.Linecast(player.transform.position, newCamPosition, out hit))               // draw line from player to where camera wants to be.
                distance = Mathf.Clamp((hit.distance * distance_ray), minDistance, maxDistance);    // if wall in between player and camera, zoom into player.
            else
                distance = maxDistance;                                                             // otherwise remain @ max distance.
        }
        void ApplyCollisionZoom()
        {
            transform.localPosition = Vector3.Lerp(transform.localPosition, dollyDirection * distance, Time.deltaTime * smoothness);
        }
    }


}
