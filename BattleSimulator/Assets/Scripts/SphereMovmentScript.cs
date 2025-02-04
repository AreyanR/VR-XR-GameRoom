using UnityEngine;
using System.Collections.Generic;

public class SphereMovementScript : MonoBehaviour
{
    public string teamTag; // "TeamA" or "TeamB"
    public float moveForce = 5f; // Movement force
    public float stopThreshold = 2f; // Distance to stop moving
    public float sizeIncreaseFactor = 1.1f; // Growth factor when merging

    private Rigidbody rb;
    private Transform target; // Movement target

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.isKinematic = false; // Ensure Rigidbody is NOT kinematic
    }

    void FixedUpdate()
    {
        FindTarget(); // Find closest teammate, then Cube

        if (target != null)
        {
            Vector3 direction = (target.position - transform.position).normalized;

            // Move towards the target if not too close
            if (Vector3.Distance(transform.position, target.position) > stopThreshold)
            {
                rb.AddForce(direction * moveForce, ForceMode.Acceleration);
            }
            else
            {
                rb.velocity = Vector3.zero; // Stop moving when close enough
            }
        }
    }

    void FindTarget()
    {
        GameObject[] allSpheres = GameObject.FindGameObjectsWithTag("Sphere"); // Find all spheres (teammates)
        GameObject[] allCubes = GameObject.FindGameObjectsWithTag("Cube"); // Find all cubes (opponents)

        float closestTeammateDistance = Mathf.Infinity;
        float closestCubeDistance = Mathf.Infinity;
        Transform closestTeammate = null;
        Transform closestCube = null;

        // Search for the closest teammate sphere
        foreach (GameObject sphere in allSpheres)
        {
            SphereMovementScript sphereScript = sphere.GetComponent<SphereMovementScript>();

            if (sphere != this.gameObject && sphereScript != null)
            {
                float distance = Vector3.Distance(transform.position, sphere.transform.position);

                if (sphereScript.teamTag == teamTag) // Find closest teammate
                {
                    if (distance < closestTeammateDistance)
                    {
                        closestTeammateDistance = distance;
                        closestTeammate = sphere.transform;
                    }
                }
            }
        }

        // Search for the closest cube (opponent)
        foreach (GameObject cube in allCubes)
        {
            float distance = Vector3.Distance(transform.position, cube.transform.position);

            if (distance < closestCubeDistance)
            {
                closestCubeDistance = distance;
                closestCube = cube.transform;
            }
        }

        // Prioritize moving to the closest teammate
        if (closestTeammate != null)
        {
            target = closestTeammate;
        }
        // If no teammates exist, target the closest cube (opponent)
        else if (closestCube != null)
        {
            target = closestCube;
        }
        else
        {
            target = null; // Stay idle if no valid targets
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        SphereMovementScript otherSphere = collision.gameObject.GetComponent<SphereMovementScript>();

        if (otherSphere != null && otherSphere.teamTag == teamTag) // Only merge with teammates
        {
            if (this.transform.localScale.magnitude >= otherSphere.transform.localScale.magnitude) // Bigger sphere absorbs smaller one
            {
                Grow();
                Destroy(otherSphere.gameObject);
            }
            else
            {
                otherSphere.Grow();
                Destroy(this.gameObject);
            }
        }
    }

void Grow()
{
    float growthAmount = 0.2f; // Small fixed increase in size
    transform.localScale += new Vector3(growthAmount, growthAmount, growthAmount);
}



}
