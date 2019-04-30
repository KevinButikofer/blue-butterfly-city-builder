using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_InformationDetailsHandler : MonoBehaviour
{
    public Text textTaxes;
    public Image imageMoneyDetails;
    public Image imagePopulationDetails;
    public Text textMoneyDetails;
    public Text textPopulationDetails;
    public Text textHappinessDetails;
    public Text textMoney;
    public Text textPopulation;
    public Text textHappiness;
    public Image imageMoney;
    public Image imagePopulation;
    public Sprite upArrow;
    public Sprite downArrow;
    private Color colorGreen = new Color32(0, 224, 68, 255);
    private Color colorRed = new Color32(255, 0, 0, 255);


    public GameObject canvasDetails;

    public void UpdateGameUI(float money, int population, float populationSatisfaction, bool isWinningMoney,bool isPopulationGrowing)
    {

        textMoney.text = money.ToString();
        textPopulation.text = population.ToString();
        textHappiness.text = populationSatisfaction.ToString();

        textMoneyDetails.text = money.ToString();
        textPopulationDetails.text = population.ToString();
        textHappinessDetails.text = populationSatisfaction.ToString();

        if (isWinningMoney)
        {
            imageMoney.sprite = upArrow;
            imageMoney.color = colorGreen;
            imageMoneyDetails.sprite = upArrow;
            imageMoneyDetails.color = colorGreen;
        }
        else
        {
            imageMoney.sprite = downArrow;
            imageMoney.color = colorRed;
            imageMoneyDetails.sprite = downArrow;
            imageMoneyDetails.color = colorRed;
        }
        if (isPopulationGrowing)
        {
            imagePopulation.sprite = upArrow;
            imagePopulation.color = colorGreen;
            imagePopulationDetails.sprite = upArrow;
            imagePopulationDetails.color = colorGreen;
        }
        else
        {
            imagePopulation.sprite = downArrow;
            imagePopulation.color = colorRed;
            imagePopulationDetails.sprite = downArrow;
            imagePopulationDetails.color = colorRed;

        }
    }


    public void ShowHideDetails()
    {
        canvasDetails.SetActive(!canvasDetails.activeSelf);
    }

}
