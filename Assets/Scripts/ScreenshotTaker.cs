using UnityEngine;
using UnityEngine.SceneManagement;

public class ScreenshotTaker : MonoBehaviour
{
    public string screenshotFileName = "screenshot";
    public string sceneToLoad;

    void Update()
    {
        // Si on appuie sur F12, prends une capture d'�cran
        if (Input.GetKeyDown(KeyCode.S))
        {
            // Nom du fichier de capture avec un horodatage pour �viter les �crasements
            string filePath = screenshotFileName + "_" + System.DateTime.Now.ToString("yyyyMMdd_HHmmss") + ".png";
            ScreenCapture.CaptureScreenshot(filePath);
        }
    }

    // Fonction pour retourner � l'�cran principal
    public void LoadScene()
    {
        Debug.Log("Chargement de la sc�ne : " + sceneToLoad);
        if (!string.IsNullOrEmpty(sceneToLoad))
        {
            SceneManager.LoadScene(sceneToLoad);
        }
        else
        {
            Debug.LogError("Le nom de la sc�ne est vide ou invalide !");
        }
    }

}