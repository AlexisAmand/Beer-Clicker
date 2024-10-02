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
        }

        // Cette fonction affiche/masque l'inventaire
        public void ToggleInventory()
            {    
                inventoryPanel.SetActive(!inventoryPanel.activeSelf);
            }

        // Cette fonction ajouter une bière spéciale dans l'inventaire

        public void AddSpecialBeer(string beerName)
        {
            specialBeersCollected.Add(beerName);
            UpdateInventoryText(specialBeersCollected); // Met à jour le texte de l'inventaire

            // Sauvegarde la liste des bières spéciales
            PlayerPrefs.SetString("specialBeersCollected", string.Join(",", specialBeersCollected));
            PlayerPrefs.Save();
        }

        // Cette fonction met à jour le texte dans l'inventaire

        public void UpdateInventoryText(List<string> beerList)
            {
                inventoryText.text = "Special Beers:\n";

                foreach (string beer in beerList)
                {
                    inventoryText.text += beer + "\n";
                }
            }

     }
