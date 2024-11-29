using UnityEngine;

// Ce script gére le background des divers écrans 

public class BackgroundScaler : MonoBehaviour
{
    private void Start()
    {
        ScaleToScreen();
    }

    // Cette fonction permet d'adapter le fond à la taille de l'écran

    private void ScaleToScreen()
    {
        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer != null)
        {
            // Obtient la taille de l'écran
            float screenRatio = (float)Screen.width / (float)Screen.height;
            float targetRatio = spriteRenderer.sprite.bounds.size.x / spriteRenderer.sprite.bounds.size.y;

            if (screenRatio >= targetRatio)
            {
                // l'écran est plus large que le sprite
                transform.localScale = new Vector3(screenRatio / targetRatio, 1, 1);
            }
            else
            {
                // l'écran est plus haut que le sprite
                transform.localScale = new Vector3(1, targetRatio / screenRatio, 1);
            }
        }
    }
}
