using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;

public class SpotArray : MonoBehaviour
{

    public static Sprite[] spots;
    public Sprite[] spotsList;

    // Start is called before the first frame update
    void Start()
    {
        spots = spotsList;
    }
}
