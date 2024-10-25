using UnityEngine;

// Ce script g�re la fen�tre de confirmation de sortie d'une partie

public class ConfirmationExit : MonoBehaviour
{
    public GameObject boxColliderObject; // R�f�rence au GameObject avec le BoxCollider
    public GameObject ExitGame; // R�f�rence � la fen�tre de confirmation

    private bool isConfirmationWindowActive;

    void Update()
    {
        boxColliderObject.SetActive(!isConfirmationWindowActive);
    }

    public void ShowConfirmation()
    {
        isConfirmationWindowActive = true;
        // D�sactiver le BoxCollider quand la fen�tre de confirmation est ouverte
        ExitGame.SetActive(true);
    }

    public void HideConfirmation()
    {
        // Cacher la fen�tre de confirmation
        ExitGame.gameObject.SetActive(false);
        isConfirmationWindowActive = false;
    }
}
