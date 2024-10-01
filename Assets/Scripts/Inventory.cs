using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public GameObject inventoryPanel; // Le panel qui s'affiche/masque
    // public Text inventoryText; // Le texte o� s'affichent les bi�res sp�ciales
    private List<string> specialBeersCollected = new List<string>(); // Liste des bi�res sp�ciales collect�es

    private void Start()
    {
        inventoryPanel.SetActive(false); // Cache le panel au d�marrage
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
