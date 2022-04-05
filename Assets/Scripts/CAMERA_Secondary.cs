using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CAMERA_Secondary : MonoBehaviour
{
    public Camera secondaryCam;

    void Update()
    {
        
    }

    void ReadInputs()
    {
        //Get the mouse x and y axis 
        float rotateHorizontal = -Input.GetAxis("Mouse X") * inputSensitivity;
        float rotateVertical = -Input.GetAxis("Mouse Y") * inputSensitivity;
    }
}
