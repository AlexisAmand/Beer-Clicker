using UnityEngine;
using UnityEngine.SceneManagement;

public class ScreenshotTaker : MonoBehaviour
{
    public string screenshotFileName = "screenshot";
    public string sceneToLoad;

    void Update()
    {
        // Si on appuie sur F12, prends une capture d'écran
        if (Input.GetKeyDown(KeyCode.S))
        {
            // Nom du fichier de capture avec un horodatage pour éviter les écrasements
            string filePath = screenshotFileName + "_" + System.DateTime.Now.ToString("yyyyMMdd_HHmmss") + ".png";
            ScreenCapture.CaptureScreenshot(filePath);
        }
    }

    // Fonction pour retourner à l'écran principal
    public void LoadScene()
    {
        Debug.Log("Chargement de la scène : " + sceneToLoad);
        if (!string.IsNullOrEmpty(sceneToLoad))
        {
            SceneManager.LoadScene(sceneToLoad);
        }
        else
        {
            Debug.LogError("Le nom de la scène est vide ou invalide !");
        }
    }

}