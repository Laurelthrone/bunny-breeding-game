using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using UnityEngine;

public class PhenotypeConverter : MonoBehaviour
{
    public GameObject prefab, spot;
    public GameObject patternLayer1, patternLayer2, patternLayer3, eyeLayer;

    public Sprite[] harlequinSprites = new Sprite[5];

    public Genome genome;
   
    public SpriteRenderer baseSprite, patternLayer1Sprite, patternLayer2Sprite, patternLayer3Sprite, eyeSprite;
    public Sprite agouti, tanOtter, himalayan, sableMarten;
    public string inspectorChinLerp, inspectorSableLerp, inspectorDiluteLerp;

    public Color black, chocolate, agoutiPatternBlack, agoutiBaseBlack, agoutiBaseChocolate, agoutiPatternChocolate, blackTan, chocolateTan, spotColor, eyeRed, eyeBlue;

    Color chinchillaLerp, sableLerp, diluteLerp;
    
    public int[] phenotype;

    // Start is called before the first frame update
    void Start()
    {

        ColorUtility.TryParseHtmlString("#352620", out chocolate);
        ColorUtility.TryParseHtmlString("#252323", out black);
        ColorUtility.TryParseHtmlString("#353535", out agoutiBaseBlack);
        ColorUtility.TryParseHtmlString("#575350", out agoutiPatternBlack); 
        ColorUtility.TryParseHtmlString("#41362D", out agoutiBaseChocolate);
        ColorUtility.TryParseHtmlString("#856B56", out agoutiPatternChocolate);
        ColorUtility.TryParseHtmlString("#A48B73", out blackTan);
        ColorUtility.TryParseHtmlString("#CDB59E", out chocolateTan);
        ColorUtility.TryParseHtmlString("#5E0000", out eyeRed);
        ColorUtility.TryParseHtmlString("#385578", out eyeBlue);
        ColorUtility.TryParseHtmlString("#" + inspectorChinLerp, out chinchillaLerp);
        ColorUtility.TryParseHtmlString("#" + inspectorSableLerp, out sableLerp);
        ColorUtility.TryParseHtmlString("#" + inspectorDiluteLerp, out diluteLerp);


        baseSprite = GetComponent<SpriteRenderer>();
        patternLayer1Sprite = patternLayer1.GetComponent<SpriteRenderer>();
        patternLayer2Sprite = patternLayer2.GetComponent<SpriteRenderer>();
        patternLayer3Sprite = patternLayer3.GetComponent<SpriteRenderer>();
        eyeSprite = eyeLayer.GetComponent<SpriteRenderer>();
        eyeSprite.color = new Color(0, 0, 0);
        eyeSprite.enabled = true;
        patternLayer2Sprite.enabled = false;
        patternLayer1Sprite.enabled = true;
        patternLayer3Sprite.enabled = false;

        genome = new Genome(true);
        phenotype = genome.phenotype;


        Debug.Log("DISPLAYING GENOME:");

        gameObject.transform.position = new Vector3(0, 0, 0);
        gameObject.transform.localScale = new Vector3(1, 1, 1);

        if (phenotype[5] == 1 || phenotype[2] == 4 || phenotype[2] == 3) gameObject.transform.position = new Vector3(UnityEngine.Random.Range(-5, 5), UnityEngine.Random.Range(-5, 5), 0);  

        foreach (Transform child in transform)
        {
            if (child.gameObject.name == "Spot(Clone)") Destroy(child.gameObject);
        }

        spawnBun();
    }

    void spawnBun()
    {
        if (phenotype[2] == 3 || phenotype[2] == 4)
        {
            cGene();
            return;
        }

        //Each method processes a different gene
        dwarfGene();
        bGene();
        dGene();
        aGene();
        cGene();
        eGene();
        enGene();
        vGene();
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

    private void dwarfGene()
    {
        switch (phenotype[6])
        {
            //Sets size
            case 0:
                gameObject.transform.localScale = new Vector3(.9f, .9f, 1);
                break;
            case 1:
                gameObject.transform.localScale = new Vector3(1, 1, 1);
                break;
            case 2:
                gameObject.transform.localScale = new Vector3(.85f, .85f, 1);
                break;
        }
    }

    private void aGene()
    {
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
    }

    private void bGene()
    {
        switch (phenotype[1])
        {
            //Case for black base
            case 0:
                Debug.Log("B1 = " + genome.genes[0, 1] + " | B2 = " + genome.genes[1, 1] + " | Phenotype = " + "Black");
                baseSprite.color = black;
                spotColor = baseSprite.color;
                if (patternLayer1Sprite.sprite == agouti) patternLayer1Sprite.color = agoutiPatternBlack;
                break;

            //Case for chocolate base
            case 1:
                Debug.Log("B1 = " + genome.genes[0, 1] + " | B2 = " + genome.genes[1, 1] + " | Phenotype = " + "Chocolate");

                //
                baseSprite.color = chocolate;
                spotColor = baseSprite.color;

                if (patternLayer1Sprite.sprite == agouti)
                {
                    patternLayer1Sprite.color = agoutiPatternChocolate;
                }
                else patternLayer1Sprite.color = chocolateTan;
                break;
        }
    }

    void cGene()
    {
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
                eyeSprite.color = new Color(1, 1, 1);
                eyeSprite.enabled = true;
                patternLayer2Sprite.color = phenotype[1] == 1 ? chocolate : black;
                patternLayer2Sprite.sprite = himalayan;
                patternLayer2Sprite.enabled = true;
                patternLayer1Sprite.enabled = false;
                eyeSprite.color = eyeRed;
                return;

            //Case for REW/albino
            case 4:
                Debug.Log("C1 = " + genome.genes[0, 2] + " | C2 = " + genome.genes[1, 2] + " | Phenotype = " + "Albino (overrides all)");
                baseSprite.color = new Color(1, 1, 1);
                eyeSprite.color = new Color(1, 1, 1);
                patternLayer1Sprite.enabled = false;
                patternLayer2Sprite.enabled = false;
                eyeSprite.enabled = true;
                eyeSprite.color = eyeRed;
                return;
        }
    }

    private void dGene()
    {
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
                spotColor = baseSprite.color;
                patternLayer1Sprite.color = Color.Lerp(patternLayer1Sprite.color, diluteLerp, .8f);
                patternLayer2Sprite.color = Color.Lerp(patternLayer2Sprite.color, diluteLerp, .8f);
                break;
        }
    }

    private void eGene()
    {
        switch (phenotype[4])
        {
            //Case for steels
            case 0:
                Debug.Log("E1 = " + genome.genes[0, 4] + " | E2 = " + genome.genes[1, 4] + " | Phenotype = " + "Steel");
                //Fuck off i'm not implementing steels now
                break;

            //Case for normal
            case 1:
                Debug.Log("E1 = " + genome.genes[0, 4] + " | E2 = " + genome.genes[1, 4] + " | Phenotype = " + "Normal");
                break;

            //Case for harlequins
            case 2:
                Debug.Log("E1 = " + genome.genes[0, 4] + " | E2 = " + genome.genes[1, 4] + " | Phenotype = " + "Harlequin");
                patternLayer3Sprite.enabled = true;
                UnityEngine.Random.InitState(genome.personalSeed);
                patternLayer3Sprite.sprite = harlequinSprites[UnityEngine.Random.Range(0, 4)];
                patternLayer3Sprite.color = patternLayer1Sprite.color;
                break;

            //Case for nonextension (makes agoutis look like selves)
            case 3:
                Debug.Log("E1 = " + genome.genes[0, 4] + " | E2 = " + genome.genes[1, 4] + " | Phenotype = " + "Nonextension (overrides A gene)");
                if (phenotype[0] == 0) patternLayer1Sprite.enabled = false;
                break;
        }
    }

    private void enGene()
    {
        switch(phenotype[5])
        {
            case 0:
                Debug.Log("En1 = " + genome.genes[0, 5] + " | En2 = " + genome.genes[1, 5] + " | Phenotype = " + "Broken");
                baseSprite.color = new Color(1, 1, 1);
                patternLayer1Sprite.enabled = patternLayer2Sprite.enabled = patternLayer3Sprite.enabled = false;
                StartCoroutine(doSpots(40, 50));
                break;
            case 1:
                Debug.Log("En1 = " + genome.genes[0, 5] + " | En2 = " + genome.genes[1, 5] + " | Phenotype = " + "Normal");
                break;
            case 2:
                Debug.Log("En1 = " + genome.genes[0, 5] + " | En2 = " + genome.genes[1, 5] + " | Phenotype = " + "Charlie");
                baseSprite.color = new Color(1, 1, 1);
                patternLayer1Sprite.enabled = patternLayer2Sprite.enabled = patternLayer3Sprite.enabled = false;
                StartCoroutine(doSpots(30, 40));
                break;
        }
    }

    private void vGene()
    {
        if (phenotype[7] == 1) eyeSprite.color = eyeBlue;
    }

    IEnumerator doSpots(int min, int max)
    {
        UnityEngine.Random.InitState(genome.personalSeed);
        Spot.numSpots = 0;
        int numSpots = UnityEngine.Random.Range(min, max);
        for (int i = 0; i < numSpots; i++)
        {
            Instantiate(spot, gameObject.transform);
            yield return new WaitForEndOfFrame();
            Spot.numSpots++;
        }
        gameObject.transform.position = new Vector3(UnityEngine.Random.Range(-5, 5), UnityEngine.Random.Range(-5, 5), 0);
    }
}
