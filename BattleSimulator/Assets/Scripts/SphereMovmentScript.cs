using UnityEngine;
using System.Collections.Generic;
using UnityEngine.XR.Interaction.Toolkit;

public class SphereMovementScript : MonoBehaviour
{
    public string teamTag; // "TeamA" or "TeamB"
    public float moveForce = 5f; // Movement force
    public float stopThreshold = 2f; // Distance to stop moving
    public float sizeIncreaseFactor = 0.1f; // Growth factor when merging
    public float groundCheckDistance = 0.2f; // Distance for raycasting to detect table

    private Rigidbody rb;
    private Transform target; // Movement target
    private bool isGrowing = false; // Prevent multiple grow calls
    private bool isOnTable = false; // Track if sphere is near the table

    private Renderer sphereRenderer;
    private Color sphereColor;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.isKinematic = false; // Ensure Rigidbody is NOT kinematic

        sphereRenderer = GetComponent<Renderer>();

        // Assign a random color at the start
        sphereColor = new Color(Random.value, Random.value, Random.value);
        sphereRenderer.material.color = sphereColor;
    }

    void Update()
{
    // Ensure uniform scale
    float maxScale = Mathf.Max(transform.localScale.x, transform.localScale.y, transform.localScale.z);
    transform.localScale = new Vector3(maxScale, maxScale, maxScale);

    if (transform.position.y < 0.65f)
    {
        Destroy(gameObject);
    }
}


    void FixedUpdate()
    {
        CheckIfOnTable(); // Use raycasting to detect the table

        if (!isOnTable || !BattleManager.Instance.isBattleActive)
        {
            rb.useGravity = true; // Allow natural falling when not on the table
            return;
        }



        FindTarget();

        if (target != null)
        {
            Vector3 direction = (target.position - transform.position).normalized;
            float scaledMoveForce = moveForce * transform.localScale.x;

            if (Vector3.Distance(transform.position, target.position) > stopThreshold)
            {
                rb.AddForce(direction * scaledMoveForce, ForceMode.Acceleration);
            }
            else
            {
                rb.velocity = Vector3.zero;
            }
        }
    }

void CheckIfOnTable()
{
    Vector3 origin = transform.position;
    float objectScale = transform.localScale.x;  // Adjust based on size
    float adjustedGroundCheckDistance = objectScale * 0.6f; // Scales with object size
    float offset = objectScale * 0.5f; // Adjust raycast points based on size
    bool tableDetected = false;
    RaycastHit hit;

    // Fire multiple rays from different points
    Vector3[] raycastOrigins = new Vector3[]
    {
        origin,                                 // Center
        origin + new Vector3(offset, 0, 0),    // Front
        origin + new Vector3(-offset, 0, 0),   // Back
        origin + new Vector3(0, 0, offset),    // Right
        origin + new Vector3(0, 0, -offset)    // Left
    };

    foreach (Vector3 rayOrigin in raycastOrigins)
    {
        if (Physics.Raycast(rayOrigin, Vector3.down, out hit, adjustedGroundCheckDistance))
        {
            if (hit.collider.CompareTag("Table"))
            {
                tableDetected = true;
                break; // No need to check further if already detected
            }
        }
    }

    isOnTable = tableDetected;
}



    void FindTarget()
    {
        if (!isOnTable)
        {
            target = null;
            return;
        }

        GameObject[] allSpheres = GameObject.FindGameObjectsWithTag("Sphere");
        GameObject[] allCubes = GameObject.FindGameObjectsWithTag("Cube");

        float closestTeammateDistance = Mathf.Infinity;
        float closestCubeDistance = Mathf.Infinity;
        Transform closestTeammate = null;
        Transform closestCube = null;

        foreach (GameObject sphere in allSpheres)
        {
            SphereMovementScript sphereScript = sphere.GetComponent<SphereMovementScript>();

            if (sphere != this.gameObject && sphereScript != null)
            {
                float distance = Vector3.Distance(transform.position, sphere.transform.position);

                if (sphereScript.teamTag == teamTag)
                {
                    if (distance < closestTeammateDistance)
                    {
                        closestTeammateDistance = distance;
                        closestTeammate = sphere.transform;
                    }
                }
            }
        }

        foreach (GameObject cube in allCubes)
        {
            float distance = Vector3.Distance(transform.position, cube.transform.position);

            if (distance < closestCubeDistance)
            {
                closestCubeDistance = distance;
                closestCube = cube.transform;
            }
        }

        if (closestTeammate != null)
        {
            target = closestTeammate;
        }
        else if (closestCube != null)
        {
            target = closestCube;
        }
        else
        {
            target = null;
        }
    }

    void OnCollisionEnter(Collision collision)
{
    if (isGrowing) return;

    SphereMovementScript otherSphere = collision.gameObject.GetComponent<SphereMovementScript>();

    if (otherSphere != null && otherSphere.teamTag == teamTag && !otherSphere.isGrowing)
    {
        XRGrabInteractable thisGrab = GetComponent<XRGrabInteractable>();
        XRGrabInteractable otherGrab = otherSphere.GetComponent<XRGrabInteractable>();

        bool thisIsHeld = thisGrab != null && thisGrab.isSelected;
        bool otherIsHeld = otherGrab != null && otherGrab.isSelected;

        // Prevent merging if either sphere is currently held
        if (thisIsHeld || otherIsHeld)
        {
            return;
        }

        float thisSize = transform.localScale.x;
        float otherSize = otherSphere.transform.localScale.x;

        if (thisSize >= otherSize)
        {
            isGrowing = true;
            sphereColor = Color.Lerp(sphereColor, otherSphere.sphereColor, 0.5f);
            sphereRenderer.material.color = sphereColor;
            Vector3 growPosition = transform.position;
            Grow();
            transform.position = growPosition;
            Destroy(otherSphere.gameObject);
            isGrowing = false;
        }
    }
}

    void Grow()
{
    Vector3 currentPos = transform.position;
    Quaternion currentRot = transform.rotation;

    bool wasKinematic = rb.isKinematic;
    rb.isKinematic = true;

    float growthFactor = 1.05f;
    float newSize = transform.localScale.x * growthFactor;
    
    // Apply uniform scaling
    transform.localScale = new Vector3(newSize, newSize, newSize);

    rb.mass *= Mathf.Pow(growthFactor, 2);

    transform.position = currentPos;
    transform.rotation = currentRot;

    rb.isKinematic = wasKinematic;
}

}
