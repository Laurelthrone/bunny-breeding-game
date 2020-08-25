using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestBehavior : MonoBehaviour
{
    public GameObject prefab;
    public GameObject agoutiLayer;

    Genome genome;
    SpriteRenderer sprite, agoutiSprite;
    Sprite agouti, tanOtter;

    Color black, brown, agoutiBlack, agoutiBrown;
    
    int[] phenotype;

    // Start is called before the first frame update
    void Start()
    {
        gameObject.transform.position = new Vector3(UnityEngine.Random.Range(-1f,1f), 0, 0);

        ColorUtility.TryParseHtmlString("#3A2222", out brown);
        ColorUtility.TryParseHtmlString("#252323", out black);
        ColorUtility.TryParseHtmlString("#606060", out agoutiBlack);
        ColorUtility.TryParseHtmlString("#745353", out agoutiBrown);

        sprite = GetComponent<SpriteRenderer>();
        agoutiSprite = agoutiLayer.GetComponent<SpriteRenderer>();
        agoutiSprite.enabled = true;

        genome = new Genome();
        phenotype = genome.phenotype;
        

        Debug.Log("DISPLAYING GENOME:");

        switch (phenotype[0])
        {
            case 0:
                Debug.Log("A1 = " + genome.genes[0,0] + "|A2 = " + genome.genes[1,0] + "|Phenotype = " + "Agouti");
                break;
            case 1:
                Debug.Log("A1 = " + genome.genes[0,0] + " | A2 = " + genome.genes[1,0] + " | Phenotype = " + "Tan/Otter");
                break;
            case 2:
                Debug.Log("A1 = " + genome.genes[0,0] + "|A2 = " + genome.genes[1,0] + "|Phenotype = " +  "Self");
                agoutiSprite.enabled = false;
                break;
        }

        switch (phenotype[1])
        {
            case 0:
                Debug.Log("B1 = " + genome.genes[0,1] + " | B2 = " + genome.genes[1,1] + " | Phenotype = " + "Black");
                sprite.color = black;
                agoutiSprite.color = agoutiBlack;
                break;
            case 1:
                Debug.Log("B1 = " + genome.genes[0,1] + " | B2 = " + genome.genes[1,1] + " | Phenotype = " + "Chocolate");
                sprite.color = brown;
                agoutiSprite.color = agoutiBrown;
                break;
        }

        switch (phenotype[2])
        {
            case 0:
                Debug.Log("C1 = " + genome.genes[0,2] + " | C2 = " + genome.genes[1,2] + " | Phenotype = " + "Normal");
                break;
            case 1:
                Debug.Log("C1 = " + genome.genes[0, 2] + " | C2 = " + genome.genes[1, 2] + " | Phenotype = " + "Dark chinchilla");
                break;
            case 2:
                Debug.Log("C1 = " + genome.genes[0, 2] + " | C2 = " + genome.genes[1, 2] + " | Phenotype = " + "Light chinhilla");
                break;
            case 3:
                Debug.Log("C1 = " + genome.genes[0, 2] + " | C2 = " + genome.genes[1, 2] + " | Phenotype = " + "Himalayan (overrides all)");
                break;
            case 4:
                Debug.Log("C1 = " + genome.genes[0, 2] + " | C2 = " + genome.genes[1, 2] + " | Phenotype = " + "Albino (overrides all)");
                break;
        }

        switch (phenotype[3])
        {
            case 0:
                Debug.Log("D1 = " + genome.genes[0, 3] + " | D2 = " + genome.genes[1, 3] + " | Phenotype = " + "Full strength");
                break;
            case 1:
                Debug.Log("D1 = " + genome.genes[0, 3] + " | D2 = " + genome.genes[1, 3] + " | Phenotype = " + "Dilute");
                break;
        }

        switch (phenotype[4])
        {
            case 0:
                Debug.Log("E1 = " + genome.genes[0, 4] + " | E2 = " + genome.genes[1, 4] + " | Phenotype = " + "Steel");
                break;
            case 1:
                Debug.Log("E1 = " + genome.genes[0, 4] + " | E2 = " + genome.genes[1, 4] + " | Phenotype = " + "Normal");
                break;
            case 2:
                Debug.Log("E1 = " + genome.genes[0, 4] + " | E2 = " + genome.genes[1, 4] + " | Phenotype = " + "Japanese");
                break;
            case 3:
                Debug.Log("E1 = " + genome.genes[0, 4] + " | E2 = " + genome.genes[1, 4] + " | Phenotype = " + "Nonextension (overrides A gene)");
                break;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            GameObject newBun = Instantiate(prefab);
            newBun.name = gameObject.name;
            Destroy(gameObject);
        }
    }
}
