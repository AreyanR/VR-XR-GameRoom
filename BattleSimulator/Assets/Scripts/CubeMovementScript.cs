using UnityEngine;
using System.Collections.Generic;

public class CubeMovementScript : MonoBehaviour
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
        FindTarget(); // Find closest teammate, then Sphere

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
        GameObject[] allCubes = GameObject.FindGameObjectsWithTag("Cube"); // Find all cubes
        GameObject[] allSpheres = GameObject.FindGameObjectsWithTag("Sphere"); // Find all spheres

        float closestTeammateDistance = Mathf.Infinity;
        float closestSphereDistance = Mathf.Infinity;
        Transform closestTeammate = null;
        Transform closestSphere = null;

        // Search for the closest teammate cube
        foreach (GameObject cube in allCubes)
        {
            CubeMovementScript cubeScript = cube.GetComponent<CubeMovementScript>();

            if (cube != this.gameObject && cubeScript != null)
            {
                float distance = Vector3.Distance(transform.position, cube.transform.position);

                if (cubeScript.teamTag == teamTag) // Find closest teammate
                {
                    if (distance < closestTeammateDistance)
                    {
                        closestTeammateDistance = distance;
                        closestTeammate = cube.transform;
                    }
                }
            }
        }

        // Search for the closest sphere (opponent)
        foreach (GameObject sphere in allSpheres)
        {
            float distance = Vector3.Distance(transform.position, sphere.transform.position);

            if (distance < closestSphereDistance)
            {
                closestSphereDistance = distance;
                closestSphere = sphere.transform;
            }
        }

        // Prioritize moving to the closest teammate
        if (closestTeammate != null)
        {
            target = closestTeammate;
        }
        // If no teammates exist, target the closest sphere
        else if (closestSphere != null)
        {
            target = closestSphere;
        }
        else
        {
            target = null; // Stay idle if no valid targets
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        CubeMovementScript otherCube = collision.gameObject.GetComponent<CubeMovementScript>();

        if (otherCube != null && otherCube.teamTag == teamTag) // Only merge with teammates
        {
            if (this.transform.localScale.magnitude >= otherCube.transform.localScale.magnitude) // Bigger cube absorbs smaller one
            {
                Grow();
                Destroy(otherCube.gameObject);
            }
            else
            {
                otherCube.Grow();
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
