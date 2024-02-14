using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CustomGrab : MonoBehaviour
{
    CustomGrab otherHand = null;
    public Transform grabbedObject = null;
    public InputActionReference grabAction;
    public InputActionReference doubleRotationAction; // Reference to the action for doubling rotation
    bool grabbing = false;
    Vector3 initialGrabOffset; // Offset between the grabbed object's position and the controller's position when grabbed
    Quaternion initialGrabRotation; // Rotation of the grabbed object when grabbed
    Vector3 previousPosition;
    Quaternion previousRotation;
    float rotationFactor = 2f; // Factor to increase rotation angle

    // List to store nearby grabbable objects
    List<Transform> nearObjects = new List<Transform>();

    private void Start()
    {
        grabAction.action.Enable();
        doubleRotationAction.action.Enable();

        // Find the other hand
        foreach (CustomGrab c in transform.parent.GetComponentsInChildren<CustomGrab>())
        {
            if (c != this)
                otherHand = c;
        }
    }

    void Update()
    {
        grabbing = grabAction.action.IsPressed();
        if (grabbing)
        {
            // Grab nearby object or the object in the other hand
            if (!grabbedObject)
            {
                grabbedObject = GetNearestGrabbableObject();
                if (grabbedObject)
                {
                    // Store initial offset and rotation of grabbed object relative to controller
                    initialGrabOffset = grabbedObject.position - transform.position;
                    initialGrabRotation = Quaternion.Inverse(transform.rotation) * grabbedObject.rotation;
                }
            }

            if (grabbedObject)
            {
                // Calculate delta position and rotation
                Vector3 deltaPosition = transform.position - previousPosition;
                Quaternion deltaRotation = Quaternion.identity;

                // Check if both hands are grabbing
                if (otherHand && otherHand.grabbedObject == grabbedObject)
                {
                    // Calculate the composite rotation of both hands
                    Quaternion thisHandRotation = transform.rotation * Quaternion.Inverse(previousRotation);
                    Quaternion otherHandRotation = otherHand.transform.rotation * Quaternion.Inverse(otherHand.previousRotation);
                    deltaRotation = otherHandRotation * thisHandRotation;
                }
                else
                {
                    // Apply rotation of the current hand only
                    deltaRotation = transform.rotation * Quaternion.Inverse(previousRotation);
                }

                // Apply delta position and rotation to the grabbed object
                grabbedObject.position += deltaPosition;
                grabbedObject.rotation *= deltaRotation;

                // Store the current rotation for the next frame
                previousRotation = transform.rotation;
            }
        }
        else if (grabbedObject)
        {
            grabbedObject = null;
        }

        // Check if the double rotation action is triggered
        if (doubleRotationAction.action.triggered && grabbedObject != null)
        {
            // Double the rotation angle
            grabbedObject.Rotate(Vector3.up, 180f * rotationFactor, Space.World);
        }

        // Update previous position and rotation even if not grabbing
        previousPosition = transform.position;
        previousRotation = transform.rotation;
    }

    private Transform GetNearestGrabbableObject()
    {
        Transform nearestObject = null;
        float minDistance = float.MaxValue;
        foreach (Transform t in nearObjects)
        {
            float distance = Vector3.Distance(transform.position, t.position);
            if (distance < minDistance)
            {
                minDistance = distance;
                nearestObject = t;
            }
        }
        return nearestObject;
    }

    private void OnTriggerEnter(Collider other)
    {
        Transform t = other.transform;
        if (t.CompareTag("Grabbable"))
            nearObjects.Add(t);
    }

    private void OnTriggerExit(Collider other)
    {
        Transform t = other.transform;
        if (t.CompareTag("Grabbable"))
            nearObjects.Remove(t);
    }
}
