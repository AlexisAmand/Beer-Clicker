using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

// Ce script gére l'inventaire

public class Inventory : MonoBehaviour
    {
        public GameObject inventoryPanel; // Le panel qui s'affiche/masque
        public Text inventoryText; // Le texte où s'affichent les bières spéciales
        private List<string> specialBeersCollected = new List<string>(); // Liste des bières spéciales collectées

        private void Start()
        {
            inventoryPanel.SetActive(false); // Cache le panel au démarrage d'une partie
            LoadInventory(); // Charge les bières sauvegardées
    }

        // Cette fonction affiche/masque l'inventaire
        public void ToggleInventory()
            {    
                inventoryPanel.SetActive(!inventoryPanel.activeSelf);
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
