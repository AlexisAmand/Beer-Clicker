using System.IO;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.LightTransport;
using Unity.VisualScripting;
using Unity.Mathematics;

[System.Serializable]
public class Mahjong
{
    public string name;
    public string description;
}

[System.Serializable]
public class MahjongData
{
    public List<Mahjong> mahjong;
}

[System.Serializable]
public class Bonus
{
    public int cost;
    public bool isActive;
    public TextMeshProUGUI text;
    public int BonusDuration;
    public string textDescription;   
    public int beerAmount;
}

[System.Serializable]
public class UIElements
{
    public Text beersText;
    public Text chronoText;
    public Text bonusText;
    public TextMeshProUGUI funnyMessage;
}

public class BeerClicker : MonoBehaviour
{
    public int beersCollected; // Compteur de bières collectées
    public Text beersText; // Référence au texte UI pour le compteur
    // public Text specialBeerText; // Référence au texte UI pour la bière spéciale
    private List<string> specialBeers = new List<string>();
    public GameObject plusOnePrefab; // Référence au prefab pour "+1"

    public AudioSource clickSound; // Référence au composant AudioSource pour le son de clic
    public AudioSource bonusSound; // Référence au composant AudioSource pour le son de clic sur un bonus
    
    private List<string> specialBeersCollected = new List<string>(); // Liste des bières spéciales collectées
    public string savedBeers;
    
    public Text ChronoText; // Référence au texte UI pour le chrono du bonus
    public Text BonusText; // Référence au texte UI pour le bonus en cours
    // private int BonusDuration; // Durée d'un bonus

    public Button[] buttons;

  // buttons[2] Référence au bouton pour le bonus happy hour
    public Bonus happyHourBonus;
    
    // buttons[3] Référence au bouton pour le bonus Double Beer Time
    public Bonus DoubleBeerTime;

    // buttons[4] Référence au bouton pour le bonus Tipsy Taps
    public Bonus TipsyTaps;

    public TextMeshProUGUI FunnyMessage; // Référence au texte UI pour le message marrant
    public GameObject MessagePanel; // Le panel qui s'affiche/masque

    private int beerAmount = 2; 
    private int beerAmountBefore;

    private MessageData messageData;
    private MahjongData mahjongData;

    public GameObject specialBeerPanel;
    public TextMeshProUGUI specialBeerText;
    public GameObject specialBeerImage;
    public AudioSource specialBeerSound;

    public float shakeMagnitude = 0.1f; // Intensité de chaque déplacement
    public int shakeCount = 10; // Nombre de secousses

    public int BonusDuration;

    public ShopManager shopManager;
    public ConfirmationExit confirmationExit;

    public Inventory inventory;

    private void Start()
    {

        MessagePanel.gameObject.SetActive(false);

        // récupération de la liste des bières spéciales

        string filePath = Path.Combine(Application.streamingAssetsPath, "mahjong.json");

        if (File.Exists(filePath))
        {
            string json = File.ReadAllText(filePath);
            Debug.Log("Contenu du JSON : " + json); // Pour vérifier le contenu

            // Désérialisation avec JsonUtility
            mahjongData = JsonUtility.FromJson<MahjongData>(json);

            if (mahjongData != null && mahjongData.mahjong != null)
            {
                Debug.Log("JSON chargé correctement !");
            }
            else
            {
                Debug.LogError("mahjongData ou mahjongeData.mahjongs est null.");
            }
        }
        else
        {
            Debug.LogError("Le fichier JSON n'existe pas à ce chemin.");
        }

        LoadBeerNamesFromFile(filePath);

        // Récupérer le nombre de bières bues si le jeu a été relancé
        beersCollected = PlayerPrefs.GetInt("beersCollected", 0);
        beersText.text = "Bières bues : " + beersCollected; // Met à jour le texte affiché

        // Récupérer la liste des bières spéciales
        string savedBeers = PlayerPrefs.GetString("specialBeersCollected", "");
        specialBeersCollected = new List<string>(savedBeers.Split(new char[] { ',' }, System.StringSplitOptions.RemoveEmptyEntries));
        FindFirstObjectByType<Inventory>().UpdateInventoryImages();

        // On vide le text des bonus
        BonusText.text = "";
        // On masque le chrono
        ChronoText.gameObject.SetActive(false);

        // Afficher le coût des bonus
        happyHourBonus.text.text = happyHourBonus.cost.ToString();
        DoubleBeerTime.text.text = DoubleBeerTime.cost.ToString();
        TipsyTaps.text.text = TipsyTaps.cost.ToString();

    }

    // Update is called once per frame
    void Update()
    {
        // Vérifie les coûts pour chaque bonus et active/désactive les boutons individuellement
        buttons[2].interactable = (beersCollected >= happyHourBonus.cost) && !IsAnyBonusActive();
        buttons[3].interactable = (beersCollected >= DoubleBeerTime.cost) && !IsAnyBonusActive();
        buttons[4].interactable = (beersCollected >= TipsyTaps.cost) && !IsAnyBonusActive();
    }

    // Fonction qui vérifie si l'un des bonus est actif
    private bool IsAnyBonusActive()
    {
        return happyHourBonus.isActive || DoubleBeerTime.isActive || TipsyTaps.isActive;
    }

    public void OnMouseDown()
    {

        if (shopManager.ShopPanel.activeSelf || confirmationExit.ExitGame.activeSelf || inventory.inventoryPanel.activeSelf)
        {
            return; // Quitte la fonction sans rien faire
        }

        if (TipsyTaps.isActive == true)
        {
            beerAmount = UnityEngine.Random.Range(0, 15);
        }

        clickSound.Play(); // Joue le son à chaque clic    

        ShowPlusOneEffect(Input.mousePosition, beerAmount);        
    }

    private void ShowPlusOneEffect(Vector3 position, int amount)
    {

        if (plusOnePrefab != null)
        {
            plusOnePrefab.GetComponent<Text>().text = "+" + amount.ToString();
        }

        beersCollected = beersCollected + amount ; // Augmente le compteur de bières
        beersText.text = "Bières bues : " + FormatNumber(beersCollected); // Met à jour le texte affiché
        
        // Vérifie si une bière spéciale est trouvée
        if (UnityEngine.Random.value < 0.1f) // 10% de chance d'obtenir une bière spéciale
        {
            int randomIndex = UnityEngine.Random.Range(0, specialBeers.Count);
            string specialBeer = specialBeers[randomIndex];

            Debug.Log("bière trouvée : " + specialBeer);

            FindFirstObjectByType<Inventory>().AddSpecialBeer(specialBeer);
            StartCoroutine(ShowSpecialBeerMessage(specialBeer)); // Lance la coroutine pour afficher le message
            PlayerPrefs.Save(); // Sauvegarde l'inventaire
        }

        // Sauvegarde les données
        PlayerPrefs.SetInt("beersCollected", beersCollected);
        PlayerPrefs.Save(); // Sauvegarde des données

        // Instancie le prefab "+1"
        GameObject plusOne = Instantiate(plusOnePrefab);

        RectTransform rectTransform = plusOne.GetComponent<RectTransform>();
        rectTransform.SetParent(GameObject.Find("Canvas").transform, false); 

        // Convertit la position de la souris de l'espace écran à l'espace local du Canvas
        Vector2 screenPoint = position;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(rectTransform.parent as RectTransform, screenPoint, null, out Vector2 localPoint);
        rectTransform.anchoredPosition = localPoint; // Positionne le "+1" à l'endroit du curseur

        // Lance la coroutine pour faire disparaître le "+1"
        StartCoroutine(MovePlusOne(plusOne));
    }

    private IEnumerator MovePlusOne(GameObject plusOne)
    {
        float duration = 4f; // Durée du mouvement
        float elapsedTime = 0f;

        Vector3 startPos = plusOne.transform.position;
        Vector3 targetPos = startPos + new Vector3(0, 50, 0); // Déplace vers le haut

        // Boucle d'animation
        while (elapsedTime < duration)
        {
            plusOne.transform.position = Vector3.Lerp(startPos, targetPos, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        Destroy(plusOne); // Détruit l'objet après la fin de l'animation
    }

    // Coroutine pour afficher le message de bière spéciale
    private IEnumerator ShowSpecialBeerMessage(string specialBeer)
    {
        specialBeerText.text = "Vous avez trouvé la " + specialBeer + " !"; // Met à jour le texte de la bière spéciale

        Image imageComponent = specialBeerImage.GetComponent<Image>();
        imageComponent.sprite = Resources.Load<Sprite>("Images/" + specialBeer);

        specialBeerPanel.SetActive(true); // Affiche le panneau
        specialBeerSound.Play();
        yield return new WaitForSeconds(4f); // Attend 5 secondes
        specialBeerSound.Stop();
        specialBeerPanel.SetActive(false); // Masque le panneau
    }

    public void LoadMainMenu()
    {       
        SceneManager.LoadScene("MainScene"); 
    }

    
    // Charge les noms de bières depuis le fichier et remplit la liste
    public void LoadBeerNamesFromFile(string filePath)
    {

        foreach (Mahjong msg in mahjongData.mahjong)
        {
            specialBeers.Add(msg.name); // Remplis la liste avec les lignes du fichier
        }

    }

    public void HappyHourBonus()
    {
        ActivateBonus(happyHourBonus, 1);
    }

    public void DoubleBeerTimeBonus()
    {
        ActivateBonus(DoubleBeerTime, 2);
    }


    public void TipsyTapsBonus()
    {
        ActivateBonus(TipsyTaps, 3);
    }

    public void ActivateBonus(Bonus bonus, int bonusType)
    {
        if (beersCollected >= bonus.cost)
        {
            beersCollected -= bonus.cost;
            beersText.text = "Bières bues : " + beersCollected;
            bonus.isActive = true;
            bonus.cost = bonus.cost * 10; 
            bonus.text.text = FormatNumber(bonus.cost).ToString();
            bonusSound.Play();
            BonusText.text = bonus.textDescription;
            BonusText.gameObject.SetActive(true);
            BonusDuration = bonus.BonusDuration; // Durée spécifique du bonus
            beerAmountBefore = beerAmount;
            beerAmount = beerAmount * bonus.beerAmount;

            // Lance la bonne coroutine en fonction du type de bonus
            StartCoroutine(ActivateBonus(bonusType, bonus));

        }
    }
    private IEnumerator ActivateBonus(int bonusValue, Bonus bonus)
    {

        int max = bonus.BonusDuration;

        ChronoText.gameObject.SetActive(true); // Affiche le chrono

        for (int i = 0; i < max; i++) // Boucle pendant la durée du bonus
        {
            
            // Affiche le chrono en rouge pendant les 5 dernières secondes
            if (BonusDuration <= max / 4)
            {
                ChronoText.text = "<color=#FF0000>" + string.Format("00:{0:D2}", BonusDuration) + "</color>";
            }
            else
            {
                ChronoText.text = string.Format("00:{0:D2}", BonusDuration);
            }

            yield return new WaitForSeconds(1); // Attendre 1 seconde
            OnMouseDown(); // Incrémente le compteur de bières
            BonusDuration--;
        }

        bonus.isActive = false;
        // bonus.text = "vide";
        beerAmount = beerAmountBefore;
        BonusText.gameObject.SetActive(false); // cache le texte
        ChronoText.gameObject.SetActive(false); // Cache le chrono
    }

    public string FormatNumber(int number)
    {
        if (number >= 1000000000)
            return (number / 1000000000f).ToString("0.##") + "B";
        else if (number >= 1000000)
            return (number / 1000000f).ToString("0.##") + "M";
        else if (number >= 1000)
            return (number / 1000f).ToString("0.##") + "K";
        else
            return number.ToString();
    }

}
