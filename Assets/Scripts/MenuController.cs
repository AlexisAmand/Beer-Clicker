using UnityEngine;
using UnityEngine.SceneManagement;

// Ce script contr�le le menu principal
// et par extension, les boutons sur les �crans de jeu

public class MenuController : MonoBehaviour
{
    public int beersCollected; // Compteur de bi�res collect�es

    // Fonction pour une nouvelle partie
    public void NewGame()
    {
        // Nouvelle partie : le nombre de bi�res bues est initialis� � 0
        beersCollected = 0;
        PlayerPrefs.SetInt("beersCollected", beersCollected);
        // Nouvelle partie : la liste des bi�res sp�ciales est vid�e
        PlayerPrefs.SetString("specialBeersCollected", "");
        // Sauvegarde des donn�es
        PlayerPrefs.Save(); 

        // Chargement de la sc�ne de jeu
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

    // Fonction pour retourner � l'�cran principal
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
