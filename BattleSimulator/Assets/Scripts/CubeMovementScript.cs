using UnityEngine;
using System.Collections.Generic;

public class CubeMovementScript : MonoBehaviour
{
    public string teamTag; // "TeamA" or "TeamB"
    public float moveForce = 5f; // Movement force
    public float stopThreshold = 2f; // Distance to stop moving
    public float sizeIncreaseFactor = 1.1f; // Growth factor when merging
    public float groundCheckDistance = 0.2f; // Distance for raycasting to detect table

    private Rigidbody rb;
    private Transform target; // Movement target
    private bool isGrowing = false; // Flag to prevent multiple grow calls
    private bool isOnTable = false; // Track if cube is touching or near the table

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.isKinematic = false; // Ensure Rigidbody is NOT kinematic
    }

    void Update()
    {
        if (transform.position.y < -1f)
        {
            Destroy(gameObject);
        }
    }

    void FixedUpdate()
    {
        CheckIfOnTable(); // Use raycasting to detect the table

        if (!isOnTable || !BattleManager.Instance.isBattleActive)
        {
            rb.useGravity = true; // Allow natural falling
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

        GameObject[] allCubes = GameObject.FindGameObjectsWithTag("Cube");
        GameObject[] allSpheres = GameObject.FindGameObjectsWithTag("Sphere");

        float closestTeammateDistance = Mathf.Infinity;
        float closestSphereDistance = Mathf.Infinity;
        Transform closestTeammate = null;
        Transform closestSphere = null;

        foreach (GameObject cube in allCubes)
        {
            CubeMovementScript cubeScript = cube.GetComponent<CubeMovementScript>();

            if (cube != this.gameObject && cubeScript != null)
            {
                float distance = Vector3.Distance(transform.position, cube.transform.position);

                if (cubeScript.teamTag == teamTag)
                {
                    if (distance < closestTeammateDistance)
                    {
                        closestTeammateDistance = distance;
                        closestTeammate = cube.transform;
                    }
                }
            }
        }

        foreach (GameObject sphere in allSpheres)
        {
            float distance = Vector3.Distance(transform.position, sphere.transform.position);

            if (distance < closestSphereDistance)
            {
                closestSphereDistance = distance;
                closestSphere = sphere.transform;
            }
        }

        if (closestTeammate != null)
        {
            target = closestTeammate;
        }
        else if (closestSphere != null)
        {
            target = closestSphere;
        }
        else
        {
            target = null;
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (isGrowing) return;

        CubeMovementScript otherCube = collision.gameObject.GetComponent<CubeMovementScript>();

        if (otherCube != null && otherCube.teamTag == teamTag && !otherCube.isGrowing)
        {
            float thisSize = transform.localScale.x;
            float otherSize = otherCube.transform.localScale.x;

            if (thisSize >= otherSize)
            {
                isGrowing = true;
                Vector3 growPosition = transform.position;
                Grow();
                transform.position = growPosition;
                Destroy(otherCube.gameObject);
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

        float growthFactor = 1.1f;
        float newSize = transform.localScale.x * growthFactor;
        transform.localScale = new Vector3(newSize, newSize, newSize);

        rb.mass *= Mathf.Pow(growthFactor, 2);

        transform.position = currentPos;
        transform.rotation = currentRot;

        rb.isKinematic = wasKinematic;
    }
}
