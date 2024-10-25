using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;

[System.Serializable]
public class Message
{
    public string key;
    public string value;
}

[System.Serializable]
public class MessageData
{
    public List<Message> messages;
}

public class Messages : MonoBehaviour
{
    public TextMeshProUGUI FunnyMessage; // R�f�rence au texte UI pour le message marrant
    public GameObject MessagePanel; // Le panel qui s'affiche/masque
    public BeerClicker beerClicker;
    private MessageData messageData;
    private int beersCollected;

    // Start is called befor e the first frame update
    void Start()
    {
        beersCollected = beerClicker.beersCollected;

        string MessageFilePath = Path.Combine(Application.streamingAssetsPath, "messages.json");
        Debug.Log("Chemin du fichier JSON: " + MessageFilePath);

        if (File.Exists(MessageFilePath))
        {
            string json = File.ReadAllText(MessageFilePath);
            Debug.Log("Contenu du JSON : " + json); // Pour v�rifier le contenu

            // D�s�rialisation avec JsonUtility
            messageData = JsonUtility.FromJson<MessageData>(json);

            if (messageData != null && messageData.messages != null)
            {
                Debug.Log("JSON charg� correctement !");
            }
            else
            {
                Debug.LogError("messageData ou messageData.messages est null.");
            }
        }
        else
        {
            Debug.LogError("Le fichier JSON n'existe pas � ce chemin.");
        }

    }

    private void Update()
    {
        int currentBeersCollected = beerClicker.beersCollected;

        // Appelle GetFunnyMessage seulement si le nombre de bi�res a chang�
        if (currentBeersCollected != beersCollected)
        {
            beersCollected = currentBeersCollected;
            GetFunnyMessage(beersCollected);
        }
    }

    private void GetFunnyMessage(int beers)
    {
        //Debug.Log("Valeur de beers : " + beers); // V�rifie la valeur de beers

        // Convertir beers en string
        string beersKey = beers.ToString();

        // Rechercher le message correspondant
        foreach (Message msg in messageData.messages)
        {
            if (msg.key == beersKey)
            {
                //Debug.Log("Message trouv� : " + msg.value);
                FunnyMessage.text = msg.value; // Assigne le message trouv�

                // V�rifie si le panel n'est pas d�j� actif avant de le r�afficher
                if (!MessagePanel.activeSelf)
                {
                    StartCoroutine(ShowMessage()); // Affiche le message
                }
                return; // Sortir de la m�thode apr�s avoir trouv� le message
            }
        }

        //Debug.LogWarning("Aucun message trouv� pour " + beers);
    }

    public IEnumerator ShowMessage()
    {
        MessagePanel.gameObject.SetActive(false);
        Debug.Log("Activation du panel");
        MessagePanel.gameObject.SetActive(true);
        yield return new WaitForSeconds(5f);
        Debug.Log("D�sactivation du panel");
        MessagePanel.gameObject.SetActive(false);
    }

}
