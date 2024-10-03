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
            LoadInventory(); // Charge les bi�res sauvegard�es
    }

        // Cette fonction affiche/masque l'inventaire
        public void ToggleInventory()
            {    
                inventoryPanel.SetActive(!inventoryPanel.activeSelf);
            }

    // Ajoute une bi�re sp�ciale � la liste, v�rifie les doublons, et sauvegarde la liste mise � jour.

    public void AddSpecialBeer(string beerName)
        {
            // V�rifie si la bi�re n'est pas d�j� dans l'inventaire
            if (!specialBeersCollected.Contains(beerName))
            {
                specialBeersCollected.Add(beerName);
                UpdateInventoryText(specialBeersCollected); // Met � jour le texte de l'inventaire

                // Sauvegarde la liste des bi�res sp�ciales
                PlayerPrefs.SetString("specialBeersCollected", string.Join(",", specialBeersCollected));
                PlayerPrefs.Save();
            }
        }

        // Met � jour le texte de l'inventaire pour afficher les bi�res collect�es.

        public void UpdateInventoryText(List<string> beerList)
            {
                inventoryText.text = "Special Beers:\n";

                foreach (string beer in beerList)
                {
                    inventoryText.text += beer + "\n";
                }
            }


        // Cette m�thode charge les bi�res sauvegard�es depuis PlayerPrefs et met � jour l'affichage.
        public void LoadInventory()
        {
            string savedBeers = PlayerPrefs.GetString("specialBeersCollected", "");
            if (!string.IsNullOrEmpty(savedBeers))
            {
                specialBeersCollected = new List<string>(savedBeers.Split(new char[] { ',' }, System.StringSplitOptions.RemoveEmptyEntries));
                UpdateInventoryText(specialBeersCollected); // Met � jour le texte de l'inventaire avec les bi�res charg�es
            }
        }

}
