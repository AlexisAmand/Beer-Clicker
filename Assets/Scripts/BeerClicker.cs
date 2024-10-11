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
using UnityEngine.XR;

public class BeerClicker : MonoBehaviour
{
    public int beersCollected; // Compteur de bi�res collect�es
    public Text beersText; // R�f�rence au texte UI pour le compteur
    public Text specialBeerText; // R�f�rence au texte UI pour la bi�re sp�ciale
    public List<string> specialBeers = new List<string>();
    public GameObject plusOnePrefab; // R�f�rence au prefab pour "+1"

    public AudioSource clickSound; // R�f�rence au composant AudioSource pour le son de clic
    public AudioSource bonusSound; // R�f�rence au composant AudioSource pour le son de clic sur un bonus

    private List<string> specialBeersCollected = new List<string>(); // Liste des bi�res sp�ciales collect�es
    public string savedBeers;
    
    public Text ChronoText; // R�f�rence au texte UI pour le chrono du bonus
    public Text BonusText; // R�f�rence au texte UI pour le bonus en cours
    private int BonusDuration; // Dur�e d'un bonus

    public Button ButtonHappyHour; // R�f�rence au bouton pour le bonus happy hour
    private int CostBonus1 = 10; // Co�t du bonus happy hour
    public TextMeshProUGUI HappyHourText; // R�f�rence au texte UI pour le bonus happy hour
    private bool isBonus1Active = false; // Indicateur si le bonus happy hour est actif

    public Button ButtonDoubleBeerTime; // R�f�rence au bouton pour le bonus Double Beer Time
    private int CostBonus2 = 7; // Co�t du bonus Double Beer Time
    public TextMeshProUGUI DoubleBeerTimeText; // R�f�rence au texte UI pour le bonus Double Beer Time
    private bool isBonus2Active = false; // Indicateur si le bonus Double Beer Time est actif

    public Button ButtonTipsyTaps; // R�f�rence au bouton pour le bonus Tipsy Taps
    private int CostBonus3 = 15; // Co�t du bonus Tipsy Taps
    public TextMeshProUGUI TipsyTapsText; // R�f�rence au texte UI pour le bonus Tipsy Taps
    private bool isBonus3Active = false; // Indicateur si le bonus Tipsy Taps est actif

    private int beerAmount = 1;

    private void Start()
    {
        // r�cup�ration de la liste des bi�res sp�ciales
        string filePath = Application.dataPath + "/beers.txt"; // Chemin relatif vers le fichier
        LoadBeerNamesFromFile(filePath);

        // R�cup�rer le nombre de bi�res bues si le jeu a �t� relanc�
        beersCollected = PlayerPrefs.GetInt("beersCollected", 0);
        beersText.text = "Drunk beers : " + beersCollected; // Met � jour le texte affich�

        // R�cup�rer la liste des bi�res sp�ciales
        string savedBeers = PlayerPrefs.GetString("specialBeersCollected", "");
        specialBeersCollected = new List<string>(savedBeers.Split(new char[] { ',' }, System.StringSplitOptions.RemoveEmptyEntries));
        FindObjectOfType<Inventory>().UpdateInventoryText(specialBeersCollected);

        // On vide le text des bonus
        BonusText.text = "";
        // On masque le chrono
        ChronoText.gameObject.SetActive(false);

        // Afficher le co�t des bonus
        HappyHourText.text = CostBonus1.ToString();
        DoubleBeerTimeText.text = CostBonus2.ToString();
        TipsyTapsText.text = CostBonus3.ToString();

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

        if (beersCollected < CostBonus3)
        {
            ButtonTipsyTaps.interactable = false;
        }
        else
        {
            ButtonTipsyTaps.interactable = true;
        }

        if (isBonus1Active == true || isBonus2Active == true || isBonus3Active == true)
        {
            ButtonHappyHour.interactable = false;
            ButtonDoubleBeerTime.interactable = false;
            ButtonTipsyTaps.interactable = false;
        }
        else
        {
            ButtonHappyHour.interactable = true;
            ButtonDoubleBeerTime.interactable = true;
            ButtonTipsyTaps.interactable = true;
        }

    }

    private void OnMouseDown()
    {
        clickSound.Play(); // Joue le son � chaque clic    

        if (isBonus3Active)
        {
            beerAmount = UnityEngine.Random.Range(0, 15);
        }

        ShowPlusOneEffect(Input.mousePosition, beerAmount); 
    }

    private void ShowPlusOneEffect(Vector3 position, int amount)
    {

        if (plusOnePrefab != null)
        {
            plusOnePrefab.GetComponent<Text>().text = "+" + amount.ToString();
        }

        beersCollected = beersCollected + amount ; // Augmente le compteur de bi�res
        beersText.text = "Drunk beers : " + beersCollected; // Met � jour le texte affich�

        // V�rifie si une bi�re sp�ciale est trouv�e
        if (UnityEngine.Random.value < 0.1f) // 10% de chance d'obtenir une bi�re sp�ciale
        {
            int randomIndex = UnityEngine.Random.Range(0, specialBeers.Count);
            string specialBeer = specialBeers[randomIndex];
            FindObjectOfType<Inventory>().AddSpecialBeer(specialBeer);
            StartCoroutine(ShowSpecialBeerMessage(specialBeer)); // Lance la coroutine pour afficher le message
            PlayerPrefs.Save(); // Sauvegarde l'inventaire
        }

        // Affiche "+1" � l'endroit du curseur
        // ShowPlusOneEffect(Input.mousePosition); // a remettre si je restaure onmousedown

        PlayerPrefs.SetInt("beersCollected", beersCollected);
        PlayerPrefs.Save(); // Sauvegarde des donn�es

        // Instancie le prefab "+1"
        GameObject plusOne = Instantiate(plusOnePrefab);

        RectTransform rectTransform = plusOne.GetComponent<RectTransform>();
        rectTransform.SetParent(GameObject.Find("Canvas").transform, false); 

        // Convertit la position de la souris de l'espace �cran � l'espace local du Canvas
        Vector2 screenPoint = position;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(rectTransform.parent as RectTransform, screenPoint, null, out Vector2 localPoint);
        rectTransform.anchoredPosition = localPoint; // Positionne le "+1" � l'endroit du curseur

        // Lance la coroutine pour faire dispara�tre le "+1"
        StartCoroutine(MovePlusOne(plusOne));
    }

    private IEnumerator MovePlusOne(GameObject plusOne)
    {

        float duration = 4f; // Dur�e du mouvement
        float elapsedTime = 0f;

        Vector3 startPos = plusOne.transform.position;
        Vector3 targetPos = startPos + new Vector3(0, 50, 0); // D�place vers le haut

        // Boucle d'animation
        while (elapsedTime < duration)
        {
            plusOne.transform.position = Vector3.Lerp(startPos, targetPos, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        Destroy(plusOne); // D�truit l'objet apr�s la fin de l'animation
    }

    // Coroutine pour afficher le message de bi�re sp�ciale
    private IEnumerator ShowSpecialBeerMessage(string specialBeer)
    {
        specialBeerText.text = "You've drunk a " + specialBeer; // Met � jour le texte de la bi�re sp�ciale
        specialBeerText.gameObject.SetActive(true);         
        yield return new WaitForSeconds(1f); 
        specialBeerText.gameObject.SetActive(false); // Masque le texte apr�s 3 secondes
    }

    public void LoadMainMenu()
    {       
        SceneManager.LoadScene("MainScene"); 
    }

    
    // Charge les noms de bi�res depuis le fichier et remplit la liste
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
            StartCoroutine(ActivateBonus1(1)); // Indicateur si le bonus est actif)); // D�marre la coroutine
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

    public void TipsyTapsBonus()
    {
        if (beersCollected >= CostBonus3)
        { 
            beersCollected = beersCollected - CostBonus3;
            beersText.text = "Drunk beers : " + beersCollected;
            isBonus3Active = true;
            bonusSound.Play(); // Joue le son quand on clic sur le bonus 
            StartCoroutine(ActivateTistyTime());
        }
    }

    private IEnumerator ActivateBonus1(int bonus)
    {
        switch (bonus)
        {
            case 1:
                BonusText.text = "It's happy hour !";
                beerAmount = 5;
                BonusDuration = 15;
                CostBonus1 = CostBonus1 * 10;
                HappyHourText.text = FormatNumber(CostBonus1).ToString();
                isBonus1Active = true;
                break;

            case 2:
                BonusText.text = "It's double beer time !";
                beerAmount = 2;
                BonusDuration = 20;
                CostBonus2 = CostBonus2 * 10;
                DoubleBeerTimeText.text = FormatNumber(CostBonus2).ToString();
                isBonus2Active = true;
                break;

            default:
                    Debug.Log("Oup... pas de bonus...");
                    break;
            }

        int max = BonusDuration;

        ChronoText.gameObject.SetActive(true); // Affiche le chrono

        for (int i = 0; i < max; i++) // Boucle pendant la dur�e du bonus
        {

            // Affiche le chrono en rouge pendant les 5 derni�res secondes
            if (BonusDuration <= max / 4)
            {
                ChronoText.text = "<color=#FF0000>" + string.Format("00:{0:D2}", BonusDuration) + "</color>";
            }
            else
            {
                ChronoText.text = string.Format("00:{0:D2}", BonusDuration);
            }


            yield return new WaitForSeconds(1); // Attendre 1 seconde
            OnMouseDown(); // Incr�mente le compteur de bi�res
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

    private IEnumerator ActivateTistyTime()
    {
        BonusText.text = "It's Tipsy Taps time!";
        CostBonus3 = CostBonus3 * 10;
        TipsyTapsText.text = FormatNumber(CostBonus3).ToString();
        isBonus3Active = true;
             
        int BonusDuration = 10;
        int max = BonusDuration;

        ChronoText.gameObject.SetActive(true); // Affiche le chrono

        for (int i = 0; i < max; i++) // Boucle pendant la dur�e du bonus
        {

            // Affiche le chrono en rouge pendant les 5 derni�res secondes
            if (BonusDuration <= max / 4)
            {
                ChronoText.text = "<color=#FF0000>" + string.Format("00:{0:D2}", BonusDuration) + "</color>";
            }
            else
            {
                ChronoText.text = string.Format("00:{0:D2}", BonusDuration);
            }

            yield return new WaitForSeconds(1); // Attendre 1 seconde
            OnMouseDown(); // Incr�mente le compteur de bi�res
            BonusDuration--;
        }

        BonusText.text = "";
        beerAmount = 1;
        isBonus3Active = false;
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
