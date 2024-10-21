using UnityEngine;

public class ScreenshotTaker : MonoBehaviour
{
    public string screenshotFileName = "screenshot";

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
    
}