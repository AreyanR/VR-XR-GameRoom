using UnityEngine;

public class SphereSpawner : MonoBehaviour
{
    public GameObject spherePrefab; // Assign the sphere prefab in the Inspector
    public Transform tableTransform; // Assign the table object in the Inspector


    public void SpawnSphere()
    {
        if (spherePrefab == null || tableTransform == null)
        {
            Debug.LogError("SpherePrefab or TableTransform is not assigned!");
            return;
        }

        // Generate random X and Z within ±5 units of the spawner's position
        float randomX = transform.position.x + Random.Range(-0.6f, 0.5f);
        float spawnZ = transform.position.z + Random.Range(-0.7f, 0.7f);
        float spawnY = transform.position.y; // Keep Y the same

        // Set the random spawn position
        Vector3 spawnPosition = new Vector3(randomX, spawnY, spawnZ);

        // Instantiate the sphere at the calculated position
        GameObject newSphere = Instantiate(spherePrefab, spawnPosition, transform.rotation);

        // Make the sphere a child of the table
        newSphere.transform.SetParent(tableTransform);
    }
}
