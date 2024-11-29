using UnityEngine;
using TMPro;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class ShopItem
{
    public string name;
    public TextMeshProUGUI cost;
    public TextMeshProUGUI level;
    public int baseCost;
    public int currentLevel = 0;

    // Calculer le coût en fonction du niveau actuel
    public int GetCurrentCost()
    {
        return baseCost * (currentLevel + 1);  // Par exemple, chaque niveau coûte plus
    }
}

public class ShopManager : MonoBehaviour
{

    public GameObject ShopPanel; // Le panel qui s'affiche/masque

    public List<ShopItem> shopItems;

    public BeerClicker beerClicker;

    public void UpdateSpecificShopItem(int itemIndex)
    {
        if (itemIndex >= 0 && itemIndex < shopItems.Count)
        {
            UpdateShopItem(shopItems[itemIndex]);
        }
    }

    public void UpdateShopItem(ShopItem item)
    {
        // Logique d'achat pour le snack
        int cost = item.GetCurrentCost();
        
        item.currentLevel++;

        item.cost.text = "Prix : " + item.GetCurrentCost().ToString();
        item.level.text = "Niveau : " + item.currentLevel.ToString();

        // On test !

        // Affichage du nom de l'item acheté dans la console
        Debug.Log("achat : " + item.name);
        // Affichage du coût de l'item acheté dans la console
        Debug.Log("coût : " + cost);
        // Affichage du nombre de bières collectées
        Debug.Log("vous avez actuellement : " + beerClicker.beersCollected);

        if (beerClicker.beersCollected >= cost)
        {
            Debug.Log("vous avez assez d'argent !");
            beerClicker.beersCollected = beerClicker.beersCollected - cost;
            Debug.Log("vous avez maintenant : " + beerClicker.beersCollected);
        }
        
        // Vérification des points du joueur
        // if (/* points du joueur >= cost */)
        //{
        //    item.currentLevel++;
        //    item.cost.text = "Coût : " + item.GetCurrentCost().ToString();
        //    item.level.text = "Niveau : " + item.currentLevel.ToString();
        //}
    }

    public void ToggleShop()
    {
        ShopPanel.SetActive(!ShopPanel.activeSelf);
    }

}
