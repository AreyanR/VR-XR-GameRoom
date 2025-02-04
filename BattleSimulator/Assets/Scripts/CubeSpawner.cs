using UnityEngine;

public class CubeSpawner : MonoBehaviour
{
    public GameObject cubePrefab; // Assign the cube prefab in the Inspector
    public Transform tableTransform; // Assign the table object in the Inspector


    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space)) // Press Space to spawn a cube
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

        // Spawn the cube exactly at the spawner's position
        GameObject newCube = Instantiate(cubePrefab, transform.position, transform.rotation);

        // Make the cube a child of the table
        newCube.transform.SetParent(tableTransform);


    }
}
