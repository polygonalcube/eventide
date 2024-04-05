using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraLogic : MonoBehaviour
{
    // The player camera.
    
    // More than centering the player on screen, the camera with pan to smooth out screen movement & give the player a better view.
    
    [SerializeField] Transform target; // The position, rotation, & scale of the object the camera will follow.
    [SerializeField] bool willPan = true; // If true, the camera will pan according to the settings.
    [SerializeField] float panMultiplier = .1f; // A modifier for how far the camera will pan in Unity units, relative to the player's velocity.
    
    Vector3 refVelocity = Vector3.zero;
    [SerializeField] float smoothTime = .3f; // The time it will take for the camera to settle at it's final position, in seconds.
    [SerializeField] float maxSpeed = Mathf.Infinity; // The maximum movement speed of the camera.

    [SerializeField] Message lockCam;
    [SerializeField] Message unlockCam;
    bool isLocked = false;

    [SerializeField] bool inDebugMode = false;

    void FixedUpdate()
    {
        if (target.tag == "Player") // If the target is the player, checked for by GameObject tag.
        {
            if (isLocked)
            {
                transform.position = Vector3.SmoothDamp(transform.position, new Vector3((float)Convert.ToDouble(lockCam.arguments[0]), 
                (float)Convert.ToDouble(lockCam.arguments[1]), transform.position.z), ref refVelocity, smoothTime, maxSpeed);
                if (inDebugMode) Debug.Log("lockCam.arguments[0]:");
                if (inDebugMode) Debug.Log((float)Convert.ToDouble(lockCam.arguments[0]));
                if (inDebugMode) Debug.Log("lockCam.arguments[1]:");
                if (inDebugMode) Debug.Log((float)Convert.ToDouble(lockCam.arguments[1]));
            }
            else
            {
                if (willPan) transform.position = Vector3.SmoothDamp(transform.position, target.position + 
                ((Vector3)target.GetComponent<Rigidbody2D>().velocity * panMultiplier) + (Vector3.forward * -10f), ref refVelocity, smoothTime, maxSpeed);
                else transform.position = target.position + (Vector3.forward * -10f); // Places the player in the center of the screen.
            }
        }
        if (lockCam != null && lockCam.isBroadcasting) isLocked = true;
        if (unlockCam != null && unlockCam.isBroadcasting) isLocked = false;
    }
}
