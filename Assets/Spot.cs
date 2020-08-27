using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spot : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        gameObject.transform.position = new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f));
        gameObject.transform.localScale = new Vector2(Random.Range(.5f, 2f), Random.Range(.5f, 2f));
        gameObject.transform.rotation = new Quaternion(0, 0, Random.Range(1f, 360f), 0);
        gameObject.GetComponent<SpriteRenderer>().sprite = SpotArray.spots[Random.Range(0, 62)];
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
