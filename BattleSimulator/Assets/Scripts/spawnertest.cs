using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class spawnertest : MonoBehaviour
{
    public GameObject spawnable;
    public Transform spawnpoint;



    public void sapwnjawn()
    {
        Instantiate(spawnable, spawnpoint.position, spawnpoint.rotation);
    }
}
