using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.IO;
using TMPro;

// Ce script g�re l'inventaire

public class Inventory : MonoBehaviour
    {
        public GameObject inventoryPanel; // Le panel qui s'affiche/masque
        public Text inventoryText; // Le texte o� s'affichent les bi�res sp�ciales
        public TextMeshProUGUI foundText; // Le texte o� s'affichent les bi�res sp�ciales trouv�es / � trouver
        private List<string> specialBeersCollected = new List<string>(); // Liste des bi�res sp�ciales collect�es
        public GameObject boxColliderObject; // R�f�rence au GameObject avec le BoxCollider
        private bool isActive;

    private void Start()
        {
            inventoryPanel.SetActive(false); // Cache le panel au d�marrage d'une partie
            LoadInventory(); // Charge les bi�res sauvegard�es
            
        }

        private void Update()
        {
            // R�cup�ration du nombre de lignes du fichier qui contient les bi�res � collectionner
            string filePath = Application.dataPath + "/beers.txt";
            int lineNumber = File.ReadAllLines(filePath).Length;

            // R�cup�ration du nombre de bi�res d�j� trouv�es
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
