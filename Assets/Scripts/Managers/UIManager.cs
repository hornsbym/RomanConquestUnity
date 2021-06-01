using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class UIManager : MonoBehaviour 
{
    public static UIManager instance {get; set;}

    // Contains the various UIs that can be created.
    // Each will contain their own draw logic.
    [SerializeField] private Canvas defaultUIPrefab;
    [SerializeField] private Canvas selectedCityUIPrefab;
    [SerializeField] private Canvas selectedRoadUIPrefab;
    [SerializeField] private Canvas combineUnitsUIPrefab;
    [SerializeField] private Canvas moveUnitsUIPrefab;
    [SerializeField] private Canvas attackUIPrefab;

    // Track the instantiated UIs here
    private Canvas defaultUI;
    private Canvas selectedCityUI;
    private Canvas selectedRoadUI;
    private Canvas combineUnitsUI;
    private Canvas moveUnitsUI;
    private Canvas attackUI;

    void Awake()
    {
        EventManager.OnDefaultSelected += ActivateDefaultUI;
        EventManager.OnSelectedCityUpdated += ActivateSelectedCityUI;
        EventManager.OnSelectedRoadUpdatedEvent += ActivateSelectedRoadUI;
        EventManager.OnTurnEnd += ActivateDefaultUI;
        EventManager.OnCombineSelected += ActivateCombineUnitsUI;
        EventManager.OnMoveUnitsSelected += ActivateMoveUnitsUI;
        EventManager.OnAttackSelected += ActivateAttackUI;
    }


    void Start()
    {
        instance = this;

        // Instantiate all UI screens, but deactivate them. 
        defaultUI = Instantiate(defaultUIPrefab, Vector3.zero, Quaternion.identity);
        selectedCityUI = Instantiate(selectedCityUIPrefab, Vector3.zero, Quaternion.identity);
        selectedRoadUI = Instantiate(selectedRoadUIPrefab, Vector3.zero, Quaternion.identity);
        combineUnitsUI = Instantiate(combineUnitsUIPrefab, Vector3.zero, Quaternion.identity);
        moveUnitsUI = Instantiate(moveUnitsUIPrefab, Vector3.zero, Quaternion.identity);
        attackUI = Instantiate(attackUIPrefab, Vector3.zero, Quaternion.identity);

        ActivateDefaultUI();
    }

    void ActivateDefaultUI()
    {
        DeactivateAllUIs();
        defaultUI.gameObject.SetActive(true);
    }

    void ActivateSelectedCityUI(City city) 
    {
        DeactivateAllUIs();
        selectedCityUI.gameObject.SetActive(true);
    }

    void ActivateSelectedRoadUI(Road road)
    {
        DeactivateAllUIs();
        selectedRoadUI.gameObject.SetActive(true);
    }

    void ActivateCombineUnitsUI(City city)
    {
        DeactivateAllUIs();
        combineUnitsUI.gameObject.SetActive(true);
    }

    void ActivateMoveUnitsUI(City city)
    {
        DeactivateAllUIs();
        moveUnitsUI.gameObject.SetActive(true);
    }

    void ActivateAttackUI(City city) {
        DeactivateAllUIs();
        attackUI.gameObject.SetActive(true);
    }

    void DeactivateAllUIs() 
    {
        defaultUI.gameObject.SetActive(false);
        selectedCityUI.gameObject.SetActive(false);
        selectedRoadUI.gameObject.SetActive(false);
        combineUnitsUI.gameObject.SetActive(false);
        moveUnitsUI.gameObject.SetActive(false);
        attackUI.gameObject.SetActive(false);
    }
}