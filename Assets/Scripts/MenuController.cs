using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour
{
    // Fonction pour charger la scène de jeu
    public void PlayGame()
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
