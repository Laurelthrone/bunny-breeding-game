using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhenotypeConverter : MonoBehaviour
{
    public GameObject prefab;
    public GameObject patternLayer1, patternLayer2, eyeLayer;

    Genome genome;
   
    SpriteRenderer baseSprite, patternLayer1Sprite, patternLayer2Sprite, eyeSprite;
    public Sprite agouti, tanOtter, himalayan, sableMarten;
    public string inspectorChinLerp, inspectorSableLerp, inspectorDiluteLerp;

    Color black, chocolate, agoutiPatternBlack, agoutiBaseBlack, agoutiBaseChocolate, agoutiPatternChocolate, blackTan, chocolateTan;

    Color chinchillaLerp, sableLerp, diluteLerp;
    
    int[] phenotype;

    // Start is called before the first frame update
    void Start()
    {
        gameObject.transform.position = new Vector3(UnityEngine.Random.Range(-1f, 1f), 0, 0);

        ColorUtility.TryParseHtmlString("#352620", out chocolate);
        ColorUtility.TryParseHtmlString("#252323", out black);
        ColorUtility.TryParseHtmlString("#353535", out agoutiBaseBlack);
        ColorUtility.TryParseHtmlString("#575350", out agoutiPatternBlack); 
        ColorUtility.TryParseHtmlString("#41362D", out agoutiBaseChocolate);
        ColorUtility.TryParseHtmlString("#856B56", out agoutiPatternChocolate);
        ColorUtility.TryParseHtmlString("#A48B73", out blackTan);
        ColorUtility.TryParseHtmlString("#CDB59E", out chocolateTan);
        ColorUtility.TryParseHtmlString("#" + inspectorChinLerp, out chinchillaLerp);
        ColorUtility.TryParseHtmlString("#" + inspectorSableLerp, out sableLerp);
        ColorUtility.TryParseHtmlString("#" + inspectorDiluteLerp, out diluteLerp);

        baseSprite = GetComponent<SpriteRenderer>();
        patternLayer1Sprite = patternLayer1.GetComponent<SpriteRenderer>();
        patternLayer2Sprite = patternLayer2.GetComponent<SpriteRenderer>();
        eyeSprite = eyeLayer.GetComponent<SpriteRenderer>();
        eyeSprite.enabled = false;
        patternLayer2Sprite.enabled = false;
        patternLayer1Sprite.enabled = true;

        genome = new Genome();
        phenotype = genome.phenotype;


        Debug.Log("DISPLAYING GENOME:");
    }

    // Update is called once per frame
    void Update()
    {
        ColorUtility.TryParseHtmlString("#" + inspectorChinLerp, out chinchillaLerp);
        ColorUtility.TryParseHtmlString("#" + inspectorSableLerp, out sableLerp);
        ColorUtility.TryParseHtmlString("#" + inspectorDiluteLerp, out diluteLerp);

        if (Input.GetKeyDown(KeyCode.Space))
        {
            GameObject newBun = Instantiate(prefab);
            newBun.name = gameObject.name;
            Destroy(gameObject);
        }

        switch (phenotype[1])
        {
            //Case for black base
            case 0:
                Debug.Log("B1 = " + genome.genes[0, 1] + " | B2 = " + genome.genes[1, 1] + " | Phenotype = " + "Black");
                baseSprite.color = black;
                if (patternLayer1Sprite.sprite == agouti) patternLayer1Sprite.color = agoutiPatternBlack;
                break;

            //Case for chocolate base
            case 1:
                Debug.Log("B1 = " + genome.genes[0, 1] + " | B2 = " + genome.genes[1, 1] + " | Phenotype = " + "Chocolate");
                baseSprite.color = chocolate;
                if (patternLayer1Sprite.sprite == agouti)
                {
                    patternLayer1Sprite.color = agoutiPatternChocolate;
                }
                else patternLayer1Sprite.color = chocolateTan;
                break;
        }

        switch (phenotype[0])
        {
            //Case for agouti pattern
            case 0:
                Debug.Log("A1 = " + genome.genes[0, 0] + "| A2 = " + genome.genes[1, 0] + "| Phenotype = " + "Agouti");
                patternLayer1Sprite.sprite = agouti;

                if (phenotype[1] == 0)
                {
                    patternLayer1Sprite.color = agoutiPatternBlack;
                    baseSprite.color = agoutiBaseBlack;
                }
                else
                {
                    patternLayer1Sprite.color = agoutiPatternChocolate;
                    baseSprite.color = agoutiBaseChocolate;
                }
                break;

            //Case for tan/otter pattern
            case 1:
                Debug.Log("A1 = " + genome.genes[0, 0] + " | A2 = " + genome.genes[1, 0] + " | Phenotype = " + "Tan/Otter");
                patternLayer1Sprite.sprite = tanOtter;
                patternLayer1Sprite.color = blackTan;
                break;

            //Case for self pattern
            case 2:
                Debug.Log("A1 = " + genome.genes[0, 0] + "|A2 = " + genome.genes[1, 0] + "|Phenotype = " + "Self");
                patternLayer1Sprite.enabled = false;
                break;
        }


        switch (phenotype[2])
        {
            //Case for normal
            case 0:
                Debug.Log("C1 = " + genome.genes[0, 2] + " | C2 = " + genome.genes[1, 2] + " | Phenotype = " + "Normal");
                break;

            //Case for chinchillas
            case 1:
                Debug.Log("C1 = " + genome.genes[0, 2] + " | C2 = " + genome.genes[1, 2] + " | Phenotype = " + "Chin");
                
                //Self chins look like normal self
                if (phenotype[0] == 2) break;
                
                //Adjust coat color based on chin modifier
                baseSprite.color = Color.Lerp(baseSprite.color, chinchillaLerp, .5f);
                patternLayer1Sprite.color = Color.Lerp(patternLayer1Sprite.color, chinchillaLerp, .5f);
                break;

            //Case for sable
            case 2:
                Debug.Log("C1 = " + genome.genes[0, 2] + " | C2 = " + genome.genes[1, 2] + " | Phenotype = " + "Sable");
                
                ///Self-sable looks exactly like normal self
                if (phenotype[0] == 2) break;

                //Sable martens (sable + tan/otter) are handled separately
                if (phenotype[0] == 1)
                {
                    baseSprite.color = new Color(1, 1, 1);
                    patternLayer1Sprite.enabled = false;
                    patternLayer2Sprite.sprite = sableMarten;
                }
                else
                {
                    baseSprite.color = Color.Lerp(baseSprite.color, sableLerp, .5f);
                    patternLayer2Sprite.sprite = himalayan;
                }

                // Adjust coat color based on sable modifier
                patternLayer1Sprite.color = Color.Lerp(patternLayer1Sprite.color, sableLerp, .5f);
                patternLayer2Sprite.color = phenotype[1] == 1 ? chocolate : black;
                patternLayer2Sprite.enabled = true;
                break;

            //Case for Himalayan/Californian pattern
            case 3:
                baseSprite.color = new Color(1, 1, 1);
                eyeSprite.enabled = true;
                patternLayer2Sprite.color = phenotype[1] == 1 ? chocolate : black;
                patternLayer2Sprite.sprite = himalayan;
                patternLayer2Sprite.enabled = true;
                patternLayer1Sprite.enabled = false;
                return;

            //Case for REW/albino
            case 4:
                Debug.Log("C1 = " + genome.genes[0, 2] + " | C2 = " + genome.genes[1, 2] + " | Phenotype = " + "Albino (overrides all)");
                baseSprite.color = new Color(1, 1, 1);
                patternLayer1Sprite.enabled = false;
                patternLayer2Sprite.enabled = false;
                eyeSprite.enabled = true;
                return;
        }

        switch (phenotype[3])
        {
            //Case for normal
            case 0:
                Debug.Log("D1 = " + genome.genes[0, 3] + " | D2 = " + genome.genes[1, 3] + " | Phenotype = " + "Full strength");
                break;
            
            //Case for diluted
            case 1:
                Debug.Log("D1 = " + genome.genes[0, 3] + " | D2 = " + genome.genes[1, 3] + " | Phenotype = " + "Dilute");
                baseSprite.color = Color.Lerp(baseSprite.color, diluteLerp, .8f);
                patternLayer1Sprite.color = Color.Lerp(patternLayer1Sprite.color, diluteLerp, .8f);
                patternLayer2Sprite.color = Color.Lerp(patternLayer2Sprite.color, diluteLerp, .8f);
                break;
        }

        switch (phenotype[4])
        {
            //Case for steels
            case 0:
                Debug.Log("E1 = " + genome.genes[0, 4] + " | E2 = " + genome.genes[1, 4] + " | Phenotype = " + "Steel");
                break;

            //Case for normal
            case 1:
                Debug.Log("E1 = " + genome.genes[0, 4] + " | E2 = " + genome.genes[1, 4] + " | Phenotype = " + "Normal");
                break;

            //Case for harlequins
            case 2:
                Debug.Log("E1 = " + genome.genes[0, 4] + " | E2 = " + genome.genes[1, 4] + " | Phenotype = " + "Harlequin");
                break;

            //Case for nonextension (makes agoutis look like selves)
            case 3:
                Debug.Log("E1 = " + genome.genes[0, 4] + " | E2 = " + genome.genes[1, 4] + " | Phenotype = " + "Nonextension (overrides A gene)");
                if (phenotype[0] == 0) patternLayer1Sprite.enabled = false;
                break;
        }

        switch (phenotype[6])
        {
            case 0:
                gameObject.transform.localScale = new Vector3(.75f, .75f, 1);
                break;
            case 1:
                gameObject.transform.localScale = new Vector3(1, 1, 1);
                break;
            case 2:
                gameObject.transform.localScale = new Vector3(.7f, .7f, 1);
                break;
        }
    }
}
