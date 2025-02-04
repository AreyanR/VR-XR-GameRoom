using UnityEngine;

public class BattleManager : MonoBehaviour
{
    public static BattleManager Instance; // Singleton to access battle state anywhere
    public bool isBattleActive = false; // Battle state

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.S)) // Press S to start battle
        {
            isBattleActive = true;
            Debug.Log("Battle Started!");
        }

        if (Input.GetKeyDown(KeyCode.E)) // Press E to end battle
        {
            isBattleActive = false;
            Debug.Log("Battle Ended!");
        }

        if (Input.GetKeyDown(KeyCode.T)) // Press T to delete all Cubes and Spheres
        {
            DeleteAllShapes();
        }
    }

    void DeleteAllShapes()
    {
        GameObject[] allCubes = GameObject.FindGameObjectsWithTag("Cube");
        GameObject[] allSpheres = GameObject.FindGameObjectsWithTag("Sphere");

        foreach (GameObject cube in allCubes)
        {
            Destroy(cube);
        }

        foreach (GameObject sphere in allSpheres)
        {
            Destroy(sphere);
        }

        Debug.Log("All Cubes and Spheres have been deleted!");
    }
}
