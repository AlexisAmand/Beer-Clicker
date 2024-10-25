using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.IO;
using TMPro;

public class Inventory : MonoBehaviour
{
    public GameObject inventoryPanel; // Le panel qui s'affiche/masque
    public GameObject beerImagePrefab; // Prefab d'image pour les bières
    public Transform inventoryContent; // Conteneur pour les images dans le Scroll Rect
    public TextMeshProUGUI foundText; // Texte pour les bières trouvées / à trouver
    private List<string> specialBeersCollected = new List<string>(); // Liste des bières spéciales collectées
    public GameObject boxColliderObject; // Référence au GameObject avec le BoxCollider
    private bool isActive;
    private int lineNumber;

    private MessageData messageData;
    private MahjongData mahjongData;

    private void Start()
    {
        inventoryPanel.SetActive(false); // Cache le panel au démarrage d'une partie
        LoadInventory(); // Charge les bières sauvegardées

        Sprite loadedSprite = Resources.Load<Sprite>("Images/beertest");
    }

    private void Update()
    {
        string filePath = Path.Combine(Application.streamingAssetsPath, "mahjong.json");
        string json = File.ReadAllText(filePath);

        mahjongData = JsonUtility.FromJson<MahjongData>(json);
        lineNumber = mahjongData.mahjong.Count;

        string savedBeers = PlayerPrefs.GetString("specialBeersCollected", "");
        specialBeersCollected = new List<string>(savedBeers.Split(new char[] { ',' }, System.StringSplitOptions.RemoveEmptyEntries));

        foundText.text = "Bières spéciales trouvées : " + specialBeersCollected.Count + " / " + lineNumber;

        BoxCollider collider = boxColliderObject.GetComponent<BoxCollider>();
        collider.enabled = !isActive; // Simplification de la gestion du collider
    }

    public void ToggleInventory()
    {
        isActive = !inventoryPanel.activeSelf;
        inventoryPanel.SetActive(isActive);
        if (isActive) UpdateInventoryImages(); // Met à jour les images si l'inventaire est ouvert
    }

    public void AddSpecialBeer(string beerName)
    {
        if (!specialBeersCollected.Contains(beerName))
        {
            Debug.Log("Trouvée: " + beerName);
            specialBeersCollected.Add(beerName);
            PlayerPrefs.SetString("specialBeersCollected", string.Join(",", specialBeersCollected));
            PlayerPrefs.Save();
            UpdateInventoryImages(); // Met à jour l'affichage des images
        }
    }

    // Met à jour l'affichage des images dans l'inventaire


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

            // Configure l'image
            Image beerImage = beerImageObject.GetComponent<Image>();
            Sprite loadedSprite = Resources.Load<Sprite>("Images/" + beerName); // chemin vers les pieces de mahjong

            beerImage.sprite = loadedSprite; 

            // Assigne le parent et réinitialise la position locale
            beerImageObject.transform.SetParent(inventoryContent, false); // 'false' pour garder l'échelle et la position locale
        }

        // Réajuste le contenu
        LayoutRebuilder.ForceRebuildLayoutImmediate(inventoryContent.GetComponent<RectTransform>());

    }

    public void LoadInventory()
    {
        string savedBeers = PlayerPrefs.GetString("specialBeersCollected", "");
        if (!string.IsNullOrEmpty(savedBeers))
        {
            specialBeersCollected = new List<string>(savedBeers.Split(new char[] { ',' }, System.StringSplitOptions.RemoveEmptyEntries));
            UpdateInventoryImages(); // Met à jour l'affichage avec les bières chargées
        }






        // string filePath = Path.Combine(Application.streamingAssetsPath, "beers.txt");
        // lineNumber = File.ReadAllLines(filePath).Length;
        foundText.text = "Bières spéciales trouvées : 0 / " + lineNumber;
    }
}
