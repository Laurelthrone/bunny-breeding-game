using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestBehavior : MonoBehaviour
{
    public GameObject prefab;
    public GameObject patternLayer1, eyeLayer;

    Genome genome;
   
    SpriteRenderer baseSprite, patternLayer1Sprite, eyeSprite;
    public Sprite agouti, tanOtter, himalayan;

    Color black, chocolate, agoutiBlack, agoutiChocolate, blackTan, chocolateTan;
    
    int[] phenotype;

    // Start is called before the first frame update
    void Start()
    {
        gameObject.transform.position = new Vector3(UnityEngine.Random.Range(-1f, 1f), 0, 0);

        ColorUtility.TryParseHtmlString("#2E2521", out chocolate);
        ColorUtility.TryParseHtmlString("#252323", out black);
        ColorUtility.TryParseHtmlString("#606060", out agoutiBlack);
        ColorUtility.TryParseHtmlString("#745353", out agoutiChocolate);
        ColorUtility.TryParseHtmlString("#A48B73", out blackTan);
        ColorUtility.TryParseHtmlString("#CDB59E", out chocolateTan);

        baseSprite = GetComponent<SpriteRenderer>();
        patternLayer1Sprite = patternLayer1.GetComponent<SpriteRenderer>();
        eyeSprite = eyeLayer.GetComponent<SpriteRenderer>();
        eyeSprite.enabled = false;
        patternLayer1Sprite.enabled = true;

        genome = new Genome();
        phenotype = genome.phenotype;


        Debug.Log("DISPLAYING GENOME:");

        switch (phenotype[0])
        {
            case 0:
                Debug.Log("A1 = " + genome.genes[0, 0] + "| A2 = " + genome.genes[1, 0] + "| Phenotype = " + "Agouti");
                patternLayer1Sprite.sprite = agouti;
                break;
            case 1:
                Debug.Log("A1 = " + genome.genes[0, 0] + " | A2 = " + genome.genes[1, 0] + " | Phenotype = " + "Tan/Otter");
                patternLayer1Sprite.sprite = tanOtter;
                patternLayer1Sprite.color = blackTan;
                break;
            case 2:
                Debug.Log("A1 = " + genome.genes[0, 0] + "|A2 = " + genome.genes[1, 0] + "|Phenotype = " + "Self");
                patternLayer1Sprite.enabled = false;
                break;
        }

        switch (phenotype[1])
        {
            case 0:
                Debug.Log("B1 = " + genome.genes[0, 1] + " | B2 = " + genome.genes[1, 1] + " | Phenotype = " + "Black");
                baseSprite.color = black;
                if (patternLayer1Sprite.sprite == agouti) patternLayer1Sprite.color = agoutiBlack;
                break;
            case 1:
                Debug.Log("B1 = " + genome.genes[0, 1] + " | B2 = " + genome.genes[1, 1] + " | Phenotype = " + "Chocolate");
                baseSprite.color = chocolate;
                if (patternLayer1Sprite.sprite == agouti)
                {
                    patternLayer1Sprite.color = agoutiChocolate;
                }
                else patternLayer1Sprite.color = chocolateTan;
                break;
        }

        switch (phenotype[2])
        {
            case 0:
                Debug.Log("C1 = " + genome.genes[0, 2] + " | C2 = " + genome.genes[1, 2] + " | Phenotype = " + "Normal");
                break;
            case 1:
                Debug.Log("C1 = " + genome.genes[0, 2] + " | C2 = " + genome.genes[1, 2] + " | Phenotype = " + "Dark chinchilla");
                break;
            case 2:
                Debug.Log("C1 = " + genome.genes[0, 2] + " | C2 = " + genome.genes[1, 2] + " | Phenotype = " + "Light chinhilla");
                break;
            case 3:
                baseSprite.color = new Color(1, 1, 1);
                eyeSprite.enabled = true;
                patternLayer1Sprite.sprite = himalayan;
                patternLayer1Sprite.color = phenotype[1] == 1 ? black : chocolate;
                patternLayer1Sprite.enabled = true;
                return;
            case 4:
                Debug.Log("C1 = " + genome.genes[0, 2] + " | C2 = " + genome.genes[1, 2] + " | Phenotype = " + "Albino (overrides all)");
                baseSprite.color = new Color(1, 1, 1);
                patternLayer1Sprite.enabled = false;
                eyeSprite.enabled = true;
                return;
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
        if (Input.GetKeyDown(KeyCode.Space))
        {
            GameObject newBun = Instantiate(prefab);
            newBun.name = gameObject.name;
            Destroy(gameObject);
        }
    }
}
