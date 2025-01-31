using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LockVRCameraPosition : MonoBehaviour
{
    public Vector3 fixedPosition = new Vector3(0f, 0f, 0f); // Adjust for seating position

    void Start()
    {
        // Reset position to ensure it always starts at (0,0,0)
        transform.position = fixedPosition;
    }

    void LateUpdate()
    {
        // Keep Camera Offset fixed in place
        transform.position = fixedPosition;
    }
}
