using TMPro;
using System.IO;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Collections;
using Unity.VisualScripting;
using static UnityEditor.PlayerSettings;


public class MahjongPiece : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{

    public TMP_Text pieceDescription;
    private string pieceName;

    private MessageData messageData;
    private MahjongData mahjongData;

    public float typingSpeed = 0.05f;
    private Coroutine typingCoroutine;

    void Start()
    {

        // pieceName = GetComponent<SpriteRenderer>().sprite.name;
        pieceName = GetComponent<Image>().sprite.name;

        string filePath = Path.Combine(Application.streamingAssetsPath, "mahjong.json");

        if (File.Exists(filePath))
        {
            string json = File.ReadAllText(filePath);
            Debug.Log("Contenu du JSON : " + json); // Pour v�rifier le contenu

            // D�s�rialisation avec JsonUtility
            mahjongData = JsonUtility.FromJson<MahjongData>(json);

            if (mahjongData != null && mahjongData.mahjong != null)
            {
                Debug.Log("JSON charg� correctement !");
            }
            else
            {
                Debug.LogError("mahjongData ou mahjongeData.mahjongs est null.");
            }
        }
        else
        {
            Debug.LogError("Le fichier JSON n'existe pas � ce chemin.");
        }
    }

    private void Update()
    {

    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        string description = GetDescriptionFromJSON(pieceName);
        Debug.Log(description);
        ShowText(description); // Met � jour le texte
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        ShowText(""); // Efface le texte ou remet � z�ro
    }


    void OnMouseExit()
    {
        ShowText(""); // Efface la description
    }

    private string GetDescriptionFromJSON(string name)
    {
        foreach (Mahjong msg in mahjongData.mahjong)
        {
            if (name == msg.name)
            {
                return msg.description; // Retourne la description d�s qu'elle est trouv�e
            }
        }

        // Si aucune description n'est trouv�e apr�s avoir v�rifi� toutes les pi�ces
        return "Oups, pas de description";
    }

    public void ShowText(string description)
    {
        // Si une coroutine est d�j� en cours, on l'arr�te
        if (typingCoroutine != null)
        {
            StopCoroutine(typingCoroutine);
        }

        // Lance la nouvelle coroutine
        typingCoroutine = StartCoroutine(TypeText(description));
    }

    private IEnumerator TypeText(string text)
    {
        pieceDescription.text = "";
        foreach (char letter in text.ToCharArray())
        {
            pieceDescription.text += letter;
            yield return new WaitForSeconds(typingSpeed);
        }
    }
}
