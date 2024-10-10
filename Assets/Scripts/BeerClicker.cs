using System.IO;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using Unity.Mathematics;
using System;
using Unity.Burst.CompilerServices;
using Unity.VisualScripting;
using TMPro;

public class BeerClicker : MonoBehaviour
{
    public int beersCollected; // Compteur de bières collectées
    public Text beersText; // Référence au texte UI pour le compteur
    public Text specialBeerText; // Référence au texte UI pour la bière spéciale
    // public List<string> specialBeers = new List<string> { "Beer-illiant", "Cheers to Beer", "Beer-lievable", "Beer-rito", "Beer-thday", "Beer-ocity" }; // Liste des bières spéciales
    public List<string> specialBeers = new List<string>();
    public GameObject plusOnePrefab; // Référence au prefab pour "+1"

    public AudioSource clickSound; // Référence au composant AudioSource pour le son de clic
    public AudioSource bonusSound; // Référence au composant AudioSource pour le son de clic sur un bonus

    private List<string> specialBeersCollected = new List<string>(); // Liste des bières spéciales collectées
    public string savedBeers;
    
    public Text ChronoText; // Référence au texte UI pour le chrono du bonus
    public Text BonusText; // Référence au texte UI pour le bonus en cours

    public Button ButtonHappyHour; // Référence au bouton pour le bonus happy hour
    private int CostBonus1 = 10; // Coût du bonus happy hour
    public TextMeshProUGUI HappyHourText; // Référence au texte UI pour le bonus happy hour
    private bool isBonus1Active = false; // Indicateur si le bonus est actif

    public Button ButtonDoubleBeerTime; // Référence au bouton pour le bonus happy hour
    private int CostBonus2 = 7; // Coût du bonus happy hour
    public TextMeshProUGUI DoubleBeerTimeText; // Référence au texte UI pour le bonus happy hour
    private bool isBonus2Active = false; // Indicateur si le bonus est actif

    public int beerAmount = 1;

    private void Start()
    {
        // récupération de la liste des bières spéciales
        string filePath = Application.dataPath + "/beers.txt"; // Chemin relatif vers le fichier
        LoadBeerNamesFromFile(filePath);

        // Récupérer le nombre de bières bues si le jeu a été relancé
        beersCollected = PlayerPrefs.GetInt("beersCollected", 0);
        beersText.text = "Drunk beers : " + beersCollected; // Met à jour le texte affiché

        // Récupérer la liste des bières spéciales
        string savedBeers = PlayerPrefs.GetString("specialBeersCollected", "");
        specialBeersCollected = new List<string>(savedBeers.Split(new char[] { ',' }, System.StringSplitOptions.RemoveEmptyEntries));
        FindObjectOfType<Inventory>().UpdateInventoryText(specialBeersCollected);

        // On vide le text des bonus
        BonusText.text = "";
        // On masque le chrono
        ChronoText.gameObject.SetActive(false);

        // Afficher le coût des bonus
        HappyHourText.text = CostBonus1.ToString();
        DoubleBeerTimeText.text = CostBonus2.ToString();
    }

    // Update is called once per frame
    void Update()
    {

        if (beersCollected < CostBonus1)
        {
            ButtonHappyHour.interactable = false;


        }
        else
        {
            ButtonHappyHour.interactable = true;
        }

        if (beersCollected < CostBonus2)
        {
         
            ButtonDoubleBeerTime.interactable = false;
        }
        else
        {
            ButtonDoubleBeerTime.interactable = true;
        }

        if (isBonus1Active == true || isBonus2Active == true)
        {
            ButtonHappyHour.interactable = false;
            ButtonDoubleBeerTime.interactable = false;

        }
        else
        {
            ButtonHappyHour.interactable = true;
            ButtonDoubleBeerTime.interactable = true;

        }

    }

    private void OnMouseDown()
    {
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
        beersText.text = "Drunk beers : " + beersCollected; // Met à jour le texte affiché

        // Vérifie si une bière spéciale est trouvée
        if (UnityEngine.Random.value < 0.1f) // 10% de chance d'obtenir une bière spéciale
        {
            int randomIndex = UnityEngine.Random.Range(0, specialBeers.Count);
            string specialBeer = specialBeers[randomIndex];
            FindObjectOfType<Inventory>().AddSpecialBeer(specialBeer);
            StartCoroutine(ShowSpecialBeerMessage(specialBeer)); // Lance la coroutine pour afficher le message
            PlayerPrefs.Save(); // Sauvegarde l'inventaire
        }

        // Affiche "+1" à l'endroit du curseur
        // ShowPlusOneEffect(Input.mousePosition); // a remettre si je restaure onmousedown

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
        specialBeerText.text = "You've drunk a " + specialBeer; // Met à jour le texte de la bière spéciale
        specialBeerText.gameObject.SetActive(true); 
        
        yield return new WaitForSeconds(1f); 

        specialBeerText.gameObject.SetActive(false); // Masque le texte après 3 secondes
    }

    public void LoadMainMenu()
    {       
        SceneManager.LoadScene("MainScene"); 
    }

    
    // Charge les noms de bières depuis le fichier et remplit la liste
    public void LoadBeerNamesFromFile(string filePath)
    {
        if (File.Exists(filePath))
        {
            string[] beersFromFile = File.ReadAllLines(filePath);
            specialBeers.AddRange(beersFromFile); // Remplis la liste avec les lignes du fichier
        }
    }

    public void Bonus1()
    {
        if (beersCollected >= CostBonus1)
        {
            beersCollected = beersCollected - CostBonus1;
            beersText.text = "Drunk beers : " + beersCollected;
            bonusSound.Play(); // Joue le son quand on clic sur le bonus 
            StartCoroutine(ActivateBonus1(1)); // Indicateur si le bonus est actif)); // Démarre la coroutine
        }
    }

    public void DoubleBeerTimeBonus()
    {
        if (beersCollected >= CostBonus2)
        {
            beersCollected = beersCollected - CostBonus2;
            beersText.text = "Drunk beers : " + beersCollected;
            bonusSound.Play(); // Joue le son quand on clic sur le bonus 
            StartCoroutine(ActivateBonus1(2));
        }
    }

    private IEnumerator ActivateBonus1(int bonus)
    {
        switch (bonus)
        {
            case 1:
                BonusText.text = "It's happy hour !";
                beerAmount = 5;
                CostBonus1 = CostBonus1 * 10;
                HappyHourText.text = FormatNumber(CostBonus1).ToString();
                isBonus1Active = true;
                break;

            case 2:
                BonusText.text = "It's double beer time !";
                beerAmount = 2;
                CostBonus2 = CostBonus2 * 10;
                DoubleBeerTimeText.text = FormatNumber(CostBonus2).ToString();
                isBonus2Active = true;
                break;

            default:
                Debug.Log("Oup... pas de bonus...");
                break;
        }

        int BonusDuration = 20;
        int max = BonusDuration;

        ChronoText.gameObject.SetActive(true); // Affiche le chrono

        for (int i = 0; i < max; i++) // Boucle pendant la durée du bonus
        {

            // Affiche le chrono en rouge pendant les 5 dernières secondes
            if (BonusDuration <= 5)
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

        switch (bonus)
        {
            case 1:
                isBonus1Active = false;
                break;

            case 2:
                isBonus2Active = false;
                break;

            default:
                Debug.Log("Oup... pas de bonus...");
                break;
        }

        BonusText.text = "";
        beerAmount = 1;
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
