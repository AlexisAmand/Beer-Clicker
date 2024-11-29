using UnityEngine;

// Ce script gère la fenêtre de confirmation de sortie d'une partie

public class ConfirmationExit : MonoBehaviour
{
    // public GameObject boxColliderObject; // Référence au GameObject avec le BoxCollider
    public GameObject ExitGame; // Référence à la fenêtre de confirmation

    // private bool isConfirmationWindowActive;

    public void ShowConfirmation()
    {
        // isConfirmationWindowActive = true;
        // Désactiver le BoxCollider quand la fenêtre de confirmation est ouverte
        ExitGame.SetActive(true);
    }

    public void HideConfirmation()
    {
        // Cacher la fenêtre de confirmation
        ExitGame.gameObject.SetActive(false);
        // isConfirmationWindowActive = false;
    }
}
