using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestBehavior : MonoBehaviour
{
    static int numBunnies = 0;

    Genome genome;
    SpriteRenderer sprite;

    Color black, brown;
    
    int[] phenotype;

    // Start is called before the first frame update
    void Start()
    {
        gameObject.transform.position = new Vector3(UnityEngine.Random.Range(-1f,1f), 0, 0);

        ColorUtility.TryParseHtmlString("#3A2222", out brown);
        ColorUtility.TryParseHtmlString("#252323", out black);
        sprite = GetComponent<SpriteRenderer>();
        genome = new Genome();
        phenotype = genome.phenotype;

        Debug.Log(phenotype[0]);

        switch (phenotype[0])
        {
            case 0:
                Debug.Log("Setblack");
                sprite.color = black;
                break;
            case 1:
                Debug.Log("Setbrown");
                sprite.color = brown;
                break;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            Instantiate(gameObject);
            Debug.Log("NewBun");
            Destroy(gameObject);
        }
    }
}
