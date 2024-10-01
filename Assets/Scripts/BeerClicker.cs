using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System;
using Unity.VisualScripting;
using UnityEngine.SceneManagement;

public class BeerClicker : MonoBehaviour
{
    public int beersCollected = 0; // Compteur de bi�res collect�es
    public Text beersText; // R�f�rence au texte UI pour le compteur
    public Text specialBeerText; // R�f�rence au texte UI pour la bi�re sp�ciale
    public List<string> specialBeers = new List<string> { "Beer-illiant", "Cheers to Beer", "Beer-lievable", "Beer-rito", "Beer-thday", "Beer-ocity" }; // Liste des bi�res sp�ciales

    public GameObject plusOnePrefab; // R�f�rence au prefab pour "+1"
    public int beersDrank = 0; // Nombre de bi�res bues

    private void Start()
    {
        specialBeerText.gameObject.SetActive(false); // Masque le texte de la bi�re sp�ciale au d�marrage

        // R�cup�rer le nombre de bi�res bues si le jeu a �t� relanc�
        beersCollected = PlayerPrefs.GetInt("BeersDrank", 0);
    }

    private void OnMouseDown()
    {

        beersCollected++; // Augmente le compteur de bi�res
        beersText.text = "Drunk beers : " + beersCollected; // Met � jour le texte affich�

        // V�rifie si une bi�re sp�ciale est trouv�e
        if (UnityEngine.Random.value < 0.1f) // 10% de chance d'obtenir une bi�re sp�ciale
        {
            int randomIndex = UnityEngine.Random.Range(0, specialBeers.Count);
            string specialBeer = specialBeers[randomIndex];
            Debug.Log("You drink a " + specialBeer + " !");
            StartCoroutine(ShowSpecialBeerMessage(specialBeer)); // Lance la coroutine pour afficher le message
        }

        // Affiche "+1" � l'endroit du curseur
        ShowPlusOneEffect(Input.mousePosition);
    }

    private void ShowPlusOneEffect(Vector3 position)
    {
        // Instancie le prefab "+1"
        GameObject plusOne = Instantiate(plusOnePrefab);
        RectTransform rectTransform = plusOne.GetComponent<RectTransform>();
        rectTransform.SetParent(GameObject.Find("Canvas").transform, false); // Assure-toi que le "+1" est sous le Canvas

        // Convertit la position de la souris de l'espace �cran � l'espace local du Canvas
        Vector2 screenPoint = position;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(rectTransform.parent as RectTransform, screenPoint, null, out Vector2 localPoint);
        rectTransform.anchoredPosition = localPoint; // Positionne le "+1" � l'endroit du curseur

        // Lance la coroutine pour faire dispara�tre le "+1"
        StartCoroutine(MovePlusOne(plusOne));
    }

    private IEnumerator MovePlusOne(GameObject plusOne)
    {
        float duration = 4f; // Dur�e du mouvement
        float elapsedTime = 0f;

        Vector3 startPos = plusOne.transform.position;
        Vector3 targetPos = startPos + new Vector3(0, 50, 0); // D�place vers le haut

        // Boucle d'animation
        while (elapsedTime < duration)
        {
            plusOne.transform.position = Vector3.Lerp(startPos, targetPos, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        Destroy(plusOne); // D�truit l'objet apr�s la fin de l'animation
    }

    // Coroutine pour afficher le message de bi�re sp�ciale
    private IEnumerator ShowSpecialBeerMessage(string specialBeer)
    {
        specialBeerText.text = "You've drunk a " + specialBeer; // Met � jour le texte de la bi�re sp�ciale
        specialBeerText.gameObject.SetActive(true); // Assure-toi que le texte est visible

        yield return new WaitForSeconds(1f); // Attend 3 secondes

        specialBeerText.gameObject.SetActive(false); // Masque le texte apr�s 3 secondes
    }

    public void LoadMainMenu()
    {
        PlayerPrefs.SetInt("BeersDrank", beersDrank);
        PlayerPrefs.Save(); // Sauvegarde des donn�es
        SceneManager.LoadScene("MainScene"); // Remplace par le nom de ta sc�ne de menu
    }

}
