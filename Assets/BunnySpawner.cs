using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BunnySpawner : MonoBehaviour
{
    public GameObject prefab;
    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < 4; i++) Instantiate(prefab);
    }
}
