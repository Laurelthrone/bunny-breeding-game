using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spot : MonoBehaviour
{
    // Start is called before the first frame update
    static public int numSpots;

    PhenotypeConverter controller;
    SpriteRenderer spotSprite;

    void Start()
    {
        controller = transform.parent.gameObject.GetComponent<PhenotypeConverter>();
        spotSprite = GetComponent<SpriteRenderer>();

        Random.InitState(controller.genome.personalSeed + (numSpots^3 + 2^10/Time.frameCount));

        if (controller.phenotype == null) return;

        if (controller.phenotype[5] == 0) gameObject.transform.localScale = new Vector2(Random.Range(1f, 2f), Random.Range(1f, 2f));
        else gameObject.transform.localScale = new Vector2(Random.Range(.5f, 1.5f), Random.Range(.5f, 1.5f));

        gameObject.transform.position = new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f));
        
        gameObject.transform.rotation = new Quaternion(0, 0, Random.Range(1f, 360f), 0);
        spotSprite.sprite = SpotArray.spots[Random.Range(0, 62)];
        spotSprite.color = controller.spotColor;
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            Destroy(gameObject);
        }
    }
}
