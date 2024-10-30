using UnityEngine;

public class ShopManager : MonoBehaviour
{

    public GameObject ShopPanel; // Le panel qui s'affiche/masque
    // public GameObject boxColliderObject; // Référence au GameObject avec le BoxCollider

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
     //   BoxCollider collider = boxColliderObject.GetComponent<BoxCollider>();
     //   collider.enabled = !ShopPanel.activeSelf; // Simplification de la gestion du collider
    }

    public void ToggleShop()
    {
        ShopPanel.SetActive(!ShopPanel.activeSelf);
    }

}
