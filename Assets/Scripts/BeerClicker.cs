using System.IO;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class BeerClicker : MonoBehaviour
{
    public int beersCollected; // Compteur de bi�res collect�es
    public Text beersText; // R�f�rence au texte UI pour le compteur
    public Text specialBeerText; // R�f�rence au texte UI pour la bi�re sp�ciale
    // public List<string> specialBeers = new List<string> { "Beer-illiant", "Cheers to Beer", "Beer-lievable", "Beer-rito", "Beer-thday", "Beer-ocity" }; // Liste des bi�res sp�ciales
    public List<string> specialBeers = new List<string>();
    public GameObject plusOnePrefab; // R�f�rence au prefab pour "+1"
    public AudioSource clickSound; // R�f�rence au composant AudioSource pour le son de clic
    private List<string> specialBeersCollected = new List<string>(); // Liste des bi�res sp�ciales collect�es
    public string savedBeers;


    private void Start()
    {
        // r�cup�ration de la liste des bi�res sp�ciales
        string filePath = Application.dataPath + "/beers.txt"; // Chemin relatif vers le fichier
        LoadBeerNamesFromFile(filePath);
        // Test d'affichage de la liste pour voir s'il y a des bugs
        foreach (string beer in specialBeers)
        {
            Debug.Log("Bi�re sp�ciale charg�e: " + beer);
        }

        // R�cup�rer le nombre de bi�res bues si le jeu a �t� relanc�
        beersCollected = PlayerPrefs.GetInt("beersCollected", 0);
        beersText.text = "Drunk beers : " + beersCollected; // Met � jour le texte affich�

        // R�cup�rer la liste des bi�res sp�ciales
        string savedBeers = PlayerPrefs.GetString("specialBeersCollected", "");
        Debug.Log("inventaire de tout � l'heure :" + savedBeers);
        specialBeersCollected = new List<string>(savedBeers.Split(new char[] { ',' }, System.StringSplitOptions.RemoveEmptyEntries));
        Debug.Log("Nombre de bi�res sp�ciales collect�es : " + specialBeersCollected.Count);

        
        FindObjectOfType<Inventory>().UpdateInventoryText(specialBeersCollected);
          
    }

    private void OnMouseDown()
    {
        clickSound.Play(); // Joue le son � chaque clic

        beersCollected++; // Augmente le compteur de bi�res
        beersText.text = "Drunk beers : " + beersCollected; // Met � jour le texte affich�

        // V�rifie si une bi�re sp�ciale est trouv�e
        if (UnityEngine.Random.value < 0.1f) // 10% de chance d'obtenir une bi�re sp�ciale
        {
            int randomIndex = UnityEngine.Random.Range(0, specialBeers.Count);
            string specialBeer = specialBeers[randomIndex];
            Debug.Log("You drink a " + specialBeer + " !");
            FindObjectOfType<Inventory>().AddSpecialBeer(specialBeer);
            StartCoroutine(ShowSpecialBeerMessage(specialBeer)); // Lance la coroutine pour afficher le message
            PlayerPrefs.Save(); // Sauvegarde l'inventaire
        }

        // Affiche "+1" � l'endroit du curseur
        ShowPlusOneEffect(Input.mousePosition);
        
        Debug.Log("Saving beersCollected: " + beersCollected); // Debug pour v�rifier la valeur avant sauvegarde
        PlayerPrefs.SetInt("beersCollected", beersCollected); 
        PlayerPrefs.Save(); // Sauvegarde des donn�es
    }

    private void ShowPlusOneEffect(Vector3 position)
    {
        // Instancie le prefab "+1"
        GameObject plusOne = Instantiate(plusOnePrefab);
        RectTransform rectTransform = plusOne.GetComponent<RectTransform>();
        rectTransform.SetParent(GameObject.Find("Canvas").transform, false); 

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
        specialBeerText.gameObject.SetActive(true); 
        
        yield return new WaitForSeconds(1f); 

        specialBeerText.gameObject.SetActive(false); // Masque le texte apr�s 3 secondes
    }

    public void LoadMainMenu()
    {       
        SceneManager.LoadScene("MainScene"); 
    }

    // Charge les noms de bi�res depuis le fichier et remplit la liste
    public void LoadBeerNamesFromFile(string filePath)
    {
        if (File.Exists(filePath))
        {
            string[] beersFromFile = File.ReadAllLines(filePath);
            specialBeers.AddRange(beersFromFile); // Remplis la liste avec les lignes du fichier
        }
        else
        {
            Debug.LogError("Fichier non trouv�: " + filePath);
        }
    }

}
