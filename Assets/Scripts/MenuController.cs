using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour
{
    public int beersCollected; // Compteur de bières collectées

    // Fonction pour une nouvelle partie
    public void NewGame()
    {
        beersCollected = 0;
        PlayerPrefs.SetInt("beersCollected", beersCollected);
        PlayerPrefs.Save(); // Sauvegarde des données

        SceneManager.LoadScene("Scene01"); // Remplace par le nom de ta scène
    }

    // Fonction pour charger une ancienne partie
    public void ContinueGame()
    {
        SceneManager.LoadScene("Scene01"); // Remplace par le nom de ta scène
    }

    // Fonction pour quitter le jeu
    public void QuitGame()
    {
        Debug.Log("Quitter le jeu !");
        Application.Quit(); // Ne fonctionne pas dans l'éditeur, mais fonctionne en build
    }
}
