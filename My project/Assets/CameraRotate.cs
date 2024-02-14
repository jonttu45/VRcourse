using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraRotate : MonoBehaviour
{
    public Transform playerHead;
    public Transform cameraTransform;

    // Update is called once per frame
    void Update()
    {
        // Calculate the direction from the player's head to the camera
        Vector3 lookDirection = cameraTransform.position - playerHead.position;

        // Calculate the rotation to look towards the camera position
        Quaternion targetRotation = Quaternion.LookRotation(lookDirection, playerHead.up);

        // Apply the rotation to the camera transform
        cameraTransform.rotation = targetRotation;
    }
}
