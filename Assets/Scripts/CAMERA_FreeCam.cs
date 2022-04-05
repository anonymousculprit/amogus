using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CAMERA_FreeCam : MonoBehaviour
{
    public bool legacyControls;                     // explained more in CAMERA_Trigger.
    public bool invertX, invertY;                   // inverts camera controls.
    public float rotationClamp = 25f;
    public float inputSensitivity = 3.0f;
    public float minimumDistanceFromPlayer = 0.1f;  // sets the minimum distance camera will be from player. default to 0.1f;
    
    float smoothTime = 0.05f;
    float rotateX, rotateY;
    Vector3 currentRotation, smoothVelo;
    GameObject player;

    public static bool STATIC_legacyControls;

    void Awake() => STATIC_legacyControls = legacyControls;
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        player = GameObject.FindGameObjectWithTag("Player");
    }
    void Update()
    {
        HandleCameraRotation();
        PointCameraTowardsPlayer();
    }

    void HandleCameraRotation()
    {
        float rotateHorizontal = Input.GetAxis("Mouse X") * inputSensitivity;   // get inputs.
        float rotateVertical = Input.GetAxis("Mouse Y") * inputSensitivity;

        rotateHorizontal = InvertControl(invertX, rotateHorizontal);            // apply control inversion where applicable.
        rotateVertical = InvertControl(invertY, rotateVertical);

        rotateY += rotateHorizontal;                                            // tracks rotation values
        rotateX += rotateVertical;

        rotateX = Mathf.Clamp(rotateX, -rotationClamp, rotationClamp);          // stops player from being able to move cam up/down so much.
        Vector3 nextRotation = new Vector3(rotateX, rotateY, 0);                // compile rotation values. this is the rotation we want to change towards.
        currentRotation = Vector3.SmoothDamp(currentRotation, nextRotation, ref smoothVelo, smoothTime);    // smooth rotation with SmoothDamp.
        transform.localEulerAngles = currentRotation;                           // set currentRotation to SmoothDamp'd rotation.
    }

    // point direction of camera towards player. set position of camera to be distance from player where applicable.
    void PointCameraTowardsPlayer() => transform.position = player.transform.position - transform.forward * minimumDistanceFromPlayer;
    float InvertControl(bool invert, float value) => invert ? -value : value;   // inverts controls depending on bool checked.
}

