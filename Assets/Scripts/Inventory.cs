using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public GameObject inventoryPanel; // Le panel qui s'affiche/masque
    // public Text inventoryText; // Le texte où s'affichent les bières spéciales
    private List<string> specialBeersCollected = new List<string>(); // Liste des bières spéciales collectées

    private void Start()
    {
        inventoryPanel.SetActive(false); // Cache le panel au démarrage
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OpenInventory()
    {
        // Affiche/masque l'inventaire
        inventoryPanel.SetActive(!inventoryPanel.activeSelf);

        if (inventoryPanel.activeSelf)
        {
            Debug.Log("Inventaire ouvert");
        }
    }
}
