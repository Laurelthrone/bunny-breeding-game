using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class Bunny : Creature
{
    static int numBuns = 0;
    public int thisBunId = 0;

    PhenotypeConverter genetics;
    SpriteMask spriteMask;
    SortingGroup sortingGroup;

    // Start is called before the first frame update
    void Start()
    {
        cd = .2f;
        cdTime = 0;
        numBuns++;
        thisBunId = numBuns;
        genetics = GetComponent<PhenotypeConverter>();
        sortingGroup = gameObject.GetComponentInChildren<SortingGroup>();

        string targetLayer = numBuns.ToString();
        sortingGroup.sortingLayerName = targetLayer;
        
        initialize();
    }

    // Update is called once per frame
    void Update()
    {
        goToTargetTile();

        updateMotivation();

        //Debug.Log(topMotivator + " " + motivators[topMotivator] + " " + drinking);
       // Debug.Log(Time.time >= cdTime);
        if (Time.time >= cdTime && path != null)
        {   
            cd = Random.Range(.2f,.5f);
            cdTime = Time.time + cd;
            followPath(path);
        }
    }
}
