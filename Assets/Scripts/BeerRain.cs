using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeerRain : MonoBehaviour
{

    public GameObject beerPrefab; // Assure-toi que c'est un prefab UI
    public RectTransform canvas; // Référence à ton canvas

    void Start()
    {
        float canvasWidth = canvas.rect.width;
        float canvasHeight = canvas.rect.height;

        for (var i = 0; i < 10; i++)
        {
            // Créer des positions aléatoires
            float offsetX = beerPrefab.GetComponent<RectTransform>().rect.width / 2;
            float offsetY = beerPrefab.GetComponent<RectTransform>().rect.height / 2;
            
            Vector2 spawnPosition = new Vector2(Random.Range(offsetX, canvasWidth - offsetX), Random.Range(offsetY, canvasHeight - offsetY));
            Debug.Log(spawnPosition);
            GameObject newBeer = Instantiate(beerPrefab, spawnPosition, Quaternion.identity);
            newBeer.transform.SetParent(canvas.transform, false);
            RectTransform rectTransform = newBeer.GetComponent<RectTransform>();
            rectTransform.anchoredPosition = spawnPosition;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
