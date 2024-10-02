using UnityEngine;
using UnityEngine.SceneManagement;

// Ce script contrôle le menu principal
// et par extension, les boutons sur les écrans de jeu

public class MenuController : MonoBehaviour
{
    public int beersCollected; // Compteur de bières collectées

    // Fonction pour une nouvelle partie
    public void NewGame()
    {
        // Nouvelle partie : le nombre de bières bues est initialisé à 0
        beersCollected = 0;
        PlayerPrefs.SetInt("beersCollected", beersCollected);
        // Nouvelle partie : la liste des bières spéciales est vidée
        PlayerPrefs.SetString("specialBeersCollected", "");
        // Sauvegarde des données
        PlayerPrefs.Save(); 

        // Chargement de la scène de jeu
        SceneManager.LoadScene("Scene01"); 
    }

    // Fonction pour charger une ancienne partie
    public void ContinueGame()
    {
        SceneManager.LoadScene("Scene01"); 
    }

    // Fonction pour quitter le jeu
    public void QuitGame()
    {
        Debug.Log("Quitter le jeu !");
        Application.Quit();
    }

    // Fonction pour retourner à l'écran principal
    public void LoadMainMenu()
    {
        SceneManager.LoadScene("MainScene"); 
    }

    // Fonction pour afficher la page d'aider
    public void LoadHelp()
    {
        SceneManager.LoadScene("HelpScene"); 
    }
}
