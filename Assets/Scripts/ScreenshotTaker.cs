using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ScreenshotTaker : MonoBehaviour
{
    public string screenshotFileName = "screenshot";
    public string sceneToLoad;

    // Bouton pour le son
    public TextMeshProUGUI SoundButtonText;
    public GameObject SoundButtonImage;

    private void Start()
    {
        SoundButtonText.text = "Couper le son";
    }

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

    // On coupe le son (ou pas) !

    public void ToggleMute()
    {
        AudioListener.pause = !AudioListener.pause;
        Image imageComponent = SoundButtonImage.GetComponent<Image>();

        if (!AudioListener.pause)
        {
            Debug.Log("Couper le son");
            imageComponent.sprite = Resources.Load<Sprite>("Icones/sound");
        }
        else
        {
            Debug.Log("Activer le son");
            imageComponent.sprite = Resources.Load<Sprite>("Icones/nosound");
        }

    }
}