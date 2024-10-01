using UnityEngine;

public class BackgroundScaler : MonoBehaviour
{
    private void Start()
    {
        ScaleToScreen();
    }

    private void ScaleToScreen()
    {
        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer != null)
        {
            // Obtient la taille de l'�cran
            float screenRatio = (float)Screen.width / (float)Screen.height;
            float targetRatio = spriteRenderer.sprite.bounds.size.x / spriteRenderer.sprite.bounds.size.y;

            if (screenRatio >= targetRatio)
            {
                // L'�cran est plus large que le sprite
                transform.localScale = new Vector3(screenRatio / targetRatio, 1, 1);
            }
            else
            {
                // L'�cran est plus haut que le sprite
                transform.localScale = new Vector3(1, targetRatio / screenRatio, 1);
            }
        }
    }
}
