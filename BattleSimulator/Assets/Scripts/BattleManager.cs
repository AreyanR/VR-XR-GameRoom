using UnityEngine;

public class BattleManager : MonoBehaviour
{
    public static BattleManager Instance; 
    public bool isBattleActive = false; 

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
        // Exit the game when Escape is pressed
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }
    }
    public void ToggleBattleState()
    {
        isBattleActive = !isBattleActive;
        Debug.Log(isBattleActive ? "Battle Started!" : "Battle Ended!");
    }

    public void DeleteAllShapes()
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
