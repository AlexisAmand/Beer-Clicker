using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

// Ce script g�re l'inventaire

public class Inventory : MonoBehaviour
    {
        public GameObject inventoryPanel; // Le panel qui s'affiche/masque
        public Text inventoryText; // Le texte o� s'affichent les bi�res sp�ciales
        private List<string> specialBeersCollected = new List<string>(); // Liste des bi�res sp�ciales collect�es

        private void Start()
        {
            inventoryPanel.SetActive(false); // Cache le panel au d�marrage d'une partie
        }

        // Cette fonction affiche/masque l'inventaire
        public void ToggleInventory()
            {    
                inventoryPanel.SetActive(!inventoryPanel.activeSelf);
            }

        // Cette fonction ajouter une bi�re sp�ciale dans l'inventaire

        public void AddSpecialBeer(string beerName)
        {
            specialBeersCollected.Add(beerName);
            UpdateInventoryText(specialBeersCollected); // Met � jour le texte de l'inventaire

            // Sauvegarde la liste des bi�res sp�ciales
            PlayerPrefs.SetString("specialBeersCollected", string.Join(",", specialBeersCollected));
            PlayerPrefs.Save();
        }

        // Cette fonction met � jour le texte dans l'inventaire

        public void UpdateInventoryText(List<string> beerList)
            {
                inventoryText.text = "Special Beers:\n";

                foreach (string beer in beerList)
                {
                    inventoryText.text += beer + "\n";
                }
            }

     }
