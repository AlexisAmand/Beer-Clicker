using UnityEngine;

public class ConfirmationExit : MonoBehaviour
{
    public GameObject boxColliderObject; // Référence au GameObject avec le BoxCollider
    public GameObject ExitGame; // Référence à la fenêtre de confirmation

    private bool isConfirmationWindowActive;

    void Update()
    {
        boxColliderObject.SetActive(!isConfirmationWindowActive);
    }

    public void ShowConfirmation()
    {
        isConfirmationWindowActive = true;
        // Désactiver le BoxCollider quand la fenêtre de confirmation est ouverte
        ExitGame.SetActive(true);
    }

    public void HideConfirmation()
    {
        // Cacher la fenêtre de confirmation
        ExitGame.gameObject.SetActive(false);
        isConfirmationWindowActive = false;

        // Charger la scène après avoir mis à jour l'état
        // SceneManager.LoadScene("MainScene");
    }
}
