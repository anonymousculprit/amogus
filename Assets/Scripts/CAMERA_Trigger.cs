using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CAMERA_Trigger : MonoBehaviour
{
    public Camera lockedCam, freeRoamCam;

    // apparently???? there is a difference in the maneuvering of controls????
    // so here is my solution: legacyControls/tankControls 
    // if this is active:
    //  - check will be OnTriggerStay.
    //  - player controls will not update when switching cameras
    // if this is not active:
    //  - check will be on trigger enter/exit
    //  - player controls will update when switching cameras.
    public static bool legacyControls = false;
    private void Start() => legacyControls = CAMERA_FreeCam.STATIC_legacyControls;

    // assuming red arrow of trigger is facing outwards
    // if player is moving towards red arrow/into trigger,
    // switch to fixed (moving into fixed space).
    // otherwise, switch to free roam (moving out of fixed space).
    // fixed check is on trigger enter (because players expect the switch immediately),
    // free roam check is on trigger exit (because players should switch upon leaving the area 
    //                                     rather than pre-emptively)
    private void OnTriggerEnter(Collider other) => NONLEGACY_SwitchToFixed(other);
    private void OnTriggerStay(Collider other) => LEGACY_SwitchCameras(other);
    private void OnTriggerExit(Collider other) => NONLEGACY_SwitchToFreeRoam(other);

    void NONLEGACY_SwitchToFixed(Collider other)
    {
        if (other.tag != "Player") return;
        if (legacyControls) return;

        if (!PlayerMovingIntoTrigger(other)) return;
        lockedCam.gameObject.SetActive(true);           // turn relevant cameras on/off to ensure it is set as main camera.
        freeRoamCam.gameObject.SetActive(false);
        PLAYER.UpdateMainCamera(lockedCam);   // update main camera to switch controls relative to camera
    }
    void NONLEGACY_SwitchToFreeRoam(Collider other)
    {
        if (other.tag != "Player") return;
        if (legacyControls) return;

        if (PlayerMovingIntoTrigger(other)) return;
        lockedCam.gameObject.SetActive(false);          // turn relevant cameras on/off to ensure it is set as main camera.
        freeRoamCam.gameObject.SetActive(true);
        PLAYER.UpdateMainCamera(freeRoamCam); // update main camera to switch controls relative to camera
    }
    void LEGACY_SwitchCameras(Collider other)
    {
        if (other.tag != "Player") return;
        if (!legacyControls) return;

        if (PlayerMovingIntoTrigger(other))   // assuming red arrow of trigger is facing outwards
            SwitchToFixed();                                    // if player is moving towards red arrow/into trigger,
        else                                                    // switch to fixed (moving into fixed space).
            SwitchToFreeRoam();                                 // otherwise, switch to free roam (moving out of fixed space).

        // turn relevant cameras on/off to ensure it is set as main camera.
        void SwitchToFixed()
        {
            lockedCam.gameObject.SetActive(true);
            freeRoamCam.gameObject.SetActive(false);
        }
        void SwitchToFreeRoam()
        {
            lockedCam.gameObject.SetActive(false);
            freeRoamCam.gameObject.SetActive(true);
        }
    }

    // oh wee vector math
    // if angle > 90, both vectors are facing same direction.
    // if angle < 90, vectors are facing different direction.
    // assuming player is moving towards transform.right/red arrow, player.forward & red angle
    // in different direction, hence angle < 90 = player moving into trigger = true.
    bool PlayerMovingIntoTrigger(Collider other) => Vector3.Angle(transform.right, other.transform.forward) > 90;
}
