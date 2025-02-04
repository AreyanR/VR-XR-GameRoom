using UnityEngine;

public class SphereSpawner : MonoBehaviour
{
    public GameObject spherePrefab; // Assign the sphere prefab in the Inspector
    public Transform tableTransform; // Assign the table object in the Inspector
    public int maxSpheres = 20; // Limit the number of spheres spawned
    private int sphereCount = 0;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return)) // Press Enter to spawn a sphere
        {
            if (sphereCount < maxSpheres)
            {
                SpawnSphere();
            }
            else
            {
                Debug.Log("Max sphere limit reached.");
            }
        }
    }

    void SpawnSphere()
    {
        if (spherePrefab == null || tableTransform == null)
        {
            Debug.LogError("SpherePrefab or TableTransform is not assigned!");
            return;
        }

        // Spawn the sphere at the spawner's position
        GameObject newSphere = Instantiate(spherePrefab, transform.position, transform.rotation);

        // Make the sphere a child of the table
        newSphere.transform.SetParent(tableTransform);

        sphereCount++;
    }
}
