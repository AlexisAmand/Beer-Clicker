using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ConfirmationExit : MonoBehaviour
{
    public GameObject boxColliderObject; // R�f�rence au GameObject avec le BoxCollider
    public GameObject ExitGame; // R�f�rence � la fen�tre de confirmation

    private bool isConfirmationWindowActive;

    void Update()
    {
        if (isConfirmationWindowActive)
        {
            // D�sactiver les interactions ailleurs
            boxColliderObject.GetComponent<Collider>().enabled = false;
        }
        else
        {
            // R�activer les interactions
            boxColliderObject.GetComponent<Collider>().enabled = true;
        }
    }

    // affiche la fen�tre de confirmation
    public void ShowConfirmation()
    {
        Debug.Log("Ouverture de la fen�tre");
        isConfirmationWindowActive = true;
        // Afficher la fen�tre de confirmation
        ExitGame.gameObject.SetActive(true);
    }

    // masque la fen�tre de confirmation
    public void HideConfirmation()
    {
        Debug.Log("Fermeture de la fen�tre");

        isConfirmationWindowActive = false;
        // Cacher la fen�tre de confirmation
        ExitGame.gameObject.SetActive(false);
    }
}
