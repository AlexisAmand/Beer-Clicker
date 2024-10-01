using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour
{
    // Fonction pour charger la sc�ne de jeu
    public void PlayGame()
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
