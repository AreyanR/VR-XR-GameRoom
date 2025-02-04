using UnityEngine;

public class CubeSpawner : MonoBehaviour
{
    public GameObject cubePrefab;
    public Transform tableTransform;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            SpawnCube();
        }
    }

    void SpawnCube()
    {
        if (cubePrefab == null || tableTransform == null)
        {
            Debug.LogError("CubePrefab or TableTransform is not assigned!");
            return;
        }

        // Randomize X and Z within ±5 units of the spawner's position
        float randomX = transform.position.x + Random.Range(-2f, 1.5f);
        float spawnZ = transform.position.z + Random.Range(-2f, 2f);
        float spawnY = transform.position.y; // Keep Y the same

        // Set the random spawn position
        Vector3 spawnPosition = new Vector3(randomX, spawnY, spawnZ);

        // Instantiate the cube at the calculated position
        GameObject newCube = Instantiate(cubePrefab, spawnPosition, Quaternion.identity);

        // Make the cube a child of the table
        newCube.transform.SetParent(tableTransform);
    }
}
