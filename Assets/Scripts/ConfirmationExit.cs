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

    public void ShowConfirmation()
    {
        isConfirmationWindowActive = true;
        // Afficher la fen�tre de confirmation
        ExitGame.gameObject.SetActive(true);
    }

    public void HideConfirmation()
    {
        SceneManager.LoadScene("MainScene");
        isConfirmationWindowActive = false;
        // Cacher la fen�tre de confirmation
        ExitGame.gameObject.SetActive(false);
    }
}
