using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CityDetailsPanel : MonoBehaviour
{
    [SerializeField] private Text wealthAmountText;
    [SerializeField] private Text publicUnrestPercentageText;
    [SerializeField] private InputField taxInputField;
    [SerializeField] private Text allegianceValueText;


    void Awake()
    {
        EventManager.OnSelectedCityUpdated += PopulateComponents;
        taxInputField.onValueChanged.AddListener(delegate { SetTaxPercentage(); });
    }

    /// <summary>
    /// Fills in the components.
    /// </summary>
    private void PopulateComponents (City city) 
    {

        allegianceValueText.text = Utilities.instance.UppercaseFirstLetter(city.allegiance.ToString());
        wealthAmountText.text = city.wealth.ToString() + " Gold";
        publicUnrestPercentageText.text = ((int) (city.publicUnrest * 100)).ToString() + "%";
        
        taxInputField.text = ((int) (city.taxRate * 100)).ToString() + "%";
        if (city.allegiance == GameManager.instance.playerAllegiance) {
            taxInputField.enabled = true;
            taxInputField.ActivateInputField();
        } else {
            taxInputField.enabled = false;
            taxInputField.DeactivateInputField();
        }
    }

    private void SetTaxPercentage() 
    {
        // TODO: Do I need to copy this or can I do it directly?
        string inputFieldCopy = string.Copy(taxInputField.text);

        // Remove the % symbol
        inputFieldCopy.TrimEnd('%');
 
        // Make the string into an int, turn it into a decimal percent, set the selected city's tax percentage 
        // Make sure it stays within the bounds of 0 and 1.
        GameManager.instance.currentCity.taxRate = Mathf.Clamp((float.Parse(inputFieldCopy) / 100f), 0, 1);
        
    }
}