using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LockVRCameraPosition : MonoBehaviour
{
    private Vector3 fixedPosition = new Vector3(-2.243f, -0.3f, 0.81f); // Fixed position for the XR Rig

    void Start()
    {
        // Lock XR Rig position at the fixed point
        transform.position = fixedPosition;
    }

    void LateUpdate()
    {
        // Force XR Rig to stay at the fixed position
        transform.position = fixedPosition;
    }
}
