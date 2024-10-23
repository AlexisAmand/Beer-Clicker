using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.IO;
using TMPro;

public class Inventory : MonoBehaviour
{
    public GameObject inventoryPanel; // Le panel qui s'affiche/masque
    public GameObject beerImagePrefab; // Prefab d'image pour les bi�res
    public Transform inventoryContent; // Conteneur pour les images dans le Scroll Rect
    public TextMeshProUGUI foundText; // Texte pour les bi�res trouv�es / � trouver
    private List<string> specialBeersCollected = new List<string>(); // Liste des bi�res sp�ciales collect�es
    public GameObject boxColliderObject; // R�f�rence au GameObject avec le BoxCollider
    private bool isActive;
    private int lineNumber;

    private void Start()
    {
        inventoryPanel.SetActive(false); // Cache le panel au d�marrage d'une partie
        LoadInventory(); // Charge les bi�res sauvegard�es

        Sprite loadedSprite = Resources.Load<Sprite>("Images/beertest");
        if (loadedSprite != null)
        {
            Debug.Log("Sprite loaded successfully!");
        }
        else
        {
            Debug.Log("Failed to load sprite.");
        }
    }

    private void Update()
    {
        string filePath = Path.Combine(Application.streamingAssetsPath, "beers.txt");
        lineNumber = File.ReadAllLines(filePath).Length;

        string savedBeers = PlayerPrefs.GetString("specialBeersCollected", "");
        specialBeersCollected = new List<string>(savedBeers.Split(new char[] { ',' }, System.StringSplitOptions.RemoveEmptyEntries));

        foundText.text = "Bi�res sp�ciales trouv�es : " + specialBeersCollected.Count + " / " + lineNumber;

        BoxCollider collider = boxColliderObject.GetComponent<BoxCollider>();
        collider.enabled = !isActive; // Simplification de la gestion du collider
    }

    public void ToggleInventory()
    {
        isActive = !inventoryPanel.activeSelf;
        inventoryPanel.SetActive(isActive);
        if (isActive) UpdateInventoryImages(); // Met � jour les images si l'inventaire est ouvert
    }

    public void AddSpecialBeer(string beerName)
    {
        if (!specialBeersCollected.Contains(beerName))
        {
            Debug.Log("Trouv�e: " + beerName);
            specialBeersCollected.Add(beerName);
            PlayerPrefs.SetString("specialBeersCollected", string.Join(",", specialBeersCollected));
            PlayerPrefs.Save();
            UpdateInventoryImages(); // Met � jour l'affichage des images
        }
    }

    // Met � jour l'affichage des images dans l'inventaire


    public void UpdateInventoryImages()
    {
        // Supprime tous les enfants existants
        foreach (Transform child in inventoryContent)
        {
            Destroy(child.gameObject);
        }

        // Instancie les nouvelles images
        foreach (string beerName in specialBeersCollected)
        {
            GameObject beerImageObject = Instantiate(beerImagePrefab, inventoryContent);

            // Configure l'image (assurez-vous que l'image est correctement assign�e)
            Image beerImage = beerImageObject.GetComponent<Image>();
            Sprite loadedSprite = Resources.Load<Sprite>("Images/" + beerName); // Assure-toi que le chemin est correct

            //Sprite loadedSprite = Resources.Load<Sprite>("Images/beertest"); // Assure-toi que le chemin est correct

            beerImage.sprite = loadedSprite; // Assignez ici le sprite correspondant � beerName;

            // Assigne le parent et r�initialise la position locale
            beerImageObject.transform.SetParent(inventoryContent, false); // 'false' pour garder l'�chelle et la position locale
        }

        // R�ajuste le contenu
        LayoutRebuilder.ForceRebuildLayoutImmediate(inventoryContent.GetComponent<RectTransform>());

    }

    public void LoadInventory()
    {
        string savedBeers = PlayerPrefs.GetString("specialBeersCollected", "");
        if (!string.IsNullOrEmpty(savedBeers))
        {
            specialBeersCollected = new List<string>(savedBeers.Split(new char[] { ',' }, System.StringSplitOptions.RemoveEmptyEntries));
            UpdateInventoryImages(); // Met � jour l'affichage avec les bi�res charg�es
        }

        string filePath = Path.Combine(Application.streamingAssetsPath, "beers.txt");
        lineNumber = File.ReadAllLines(filePath).Length;
        foundText.text = "Bi�res sp�ciales trouv�es : 0 / " + lineNumber;
    }
}
