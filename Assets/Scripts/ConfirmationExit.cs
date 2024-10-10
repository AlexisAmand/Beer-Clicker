using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ConfirmationExit : MonoBehaviour
{
    public GameObject boxColliderObject; // Référence au GameObject avec le BoxCollider
    public GameObject ExitGame; // Référence à la fenêtre de confirmation

    private bool isConfirmationWindowActive;

    void Update()
    {
        if (isConfirmationWindowActive)
        {
            // Désactiver les interactions ailleurs
            boxColliderObject.GetComponent<Collider>().enabled = false;
        }
        else
        {
            // Réactiver les interactions
            boxColliderObject.GetComponent<Collider>().enabled = true;
        }
    }

    public void ShowConfirmation()
    {
        isConfirmationWindowActive = true;
        // Afficher la fenêtre de confirmation
        ExitGame.gameObject.SetActive(true);
    }

    public void HideConfirmation()
    {
        SceneManager.LoadScene("MainScene");
        isConfirmationWindowActive = false;
        // Cacher la fenêtre de confirmation
        ExitGame.gameObject.SetActive(false);
    }
}
