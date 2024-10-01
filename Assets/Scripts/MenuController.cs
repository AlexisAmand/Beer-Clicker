using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour
{
    public int beersCollected; // Compteur de bi�res collect�es

    // Fonction pour une nouvelle partie
    public void NewGame()
    {
        beersCollected = 0;
        PlayerPrefs.SetInt("beersCollected", beersCollected);
        PlayerPrefs.Save(); // Sauvegarde des donn�es

        SceneManager.LoadScene("Scene01"); // Remplace par le nom de ta sc�ne
    }

    // Fonction pour charger une ancienne partie
    public void ContinueGame()
    {
        SceneManager.LoadScene("Scene01"); // Remplace par le nom de ta sc�ne
    }

    // Fonction pour quitter le jeu
    public void QuitGame()
    {
        Debug.Log("Quitter le jeu !");
        Application.Quit(); // Ne fonctionne pas dans l'�diteur, mais fonctionne en build
    }
}
