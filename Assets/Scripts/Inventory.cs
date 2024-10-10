using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.IO;
using TMPro;

// Ce script gére l'inventaire

public class Inventory : MonoBehaviour
    {
        public GameObject inventoryPanel; // Le panel qui s'affiche/masque
        public Text inventoryText; // Le texte où s'affichent les bières spéciales
        public TextMeshProUGUI foundText; // Le texte où s'affichent les bières spéciales trouvées / à trouver
        private List<string> specialBeersCollected = new List<string>(); // Liste des bières spéciales collectées
        public GameObject boxColliderObject; // Référence au GameObject avec le BoxCollider
        private bool isActive;

    private void Start()
        {
            inventoryPanel.SetActive(false); // Cache le panel au démarrage d'une partie
            LoadInventory(); // Charge les bières sauvegardées
            
        }

        private void Update()
        {
            // Récupération du nombre de lignes du fichier qui contient les bières à collectionner
            string filePath = Application.dataPath + "/beers.txt";
            int lineNumber = File.ReadAllLines(filePath).Length;

            // Récupération du nombre de bières déjà trouvées
            string savedBeers = PlayerPrefs.GetString("specialBeersCollected", "");
            specialBeersCollected = new List<string>(savedBeers.Split(new char[] { ',' }, System.StringSplitOptions.RemoveEmptyEntries));

            foundText.text = "Special beers found : " + specialBeersCollected.Count +" / " + lineNumber;

            BoxCollider collider = boxColliderObject.GetComponent<BoxCollider>();

            
                if (isActive)
                {

                    collider.enabled = false;
                }
                else
                {

                    collider.enabled = true;
                }
            

        }

    // Cette fonction affiche/masque l'inventaire
    public void ToggleInventory()
        {
            isActive = !inventoryPanel.activeSelf;
            inventoryPanel.SetActive(isActive);
        }    

        // Ajoute une bière spéciale à la liste, vérifie les doublons, et sauvegarde la liste mise à jour.

        public void AddSpecialBeer(string beerName)
            {
                // Vérifie si la bière n'est pas déjà dans l'inventaire
                if (!specialBeersCollected.Contains(beerName))
                {
                    specialBeersCollected.Add(beerName);
                    UpdateInventoryText(specialBeersCollected); // Met à jour le texte de l'inventaire

                    // Sauvegarde la liste des bières spéciales
                    PlayerPrefs.SetString("specialBeersCollected", string.Join(",", specialBeersCollected));
                    PlayerPrefs.Save();
                }
            }

        // Met à jour le texte de l'inventaire pour afficher les bières collectées.

        public void UpdateInventoryText(List<string> beerList)
            {
                inventoryText.text = "Special Beers:\n";

                foreach (string beer in beerList)
                {
                    inventoryText.text += beer + "\n";
                }
            }

        // Cette méthode charge les bières sauvegardées depuis PlayerPrefs et met à jour l'affichage.
        public void LoadInventory()
        {
            string savedBeers = PlayerPrefs.GetString("specialBeersCollected", "");
            if (!string.IsNullOrEmpty(savedBeers))
            {
                specialBeersCollected = new List<string>(savedBeers.Split(new char[] { ',' }, System.StringSplitOptions.RemoveEmptyEntries));
                UpdateInventoryText(specialBeersCollected); // Met à jour le texte de l'inventaire avec les bières chargées
            }
        }

}
