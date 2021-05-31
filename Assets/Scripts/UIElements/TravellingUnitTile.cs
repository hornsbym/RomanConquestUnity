using UnityEngine;
using UnityEngine.UI;

public class TravellingUnitTile : MonoBehaviour
{
    [SerializeField] private Text unitNameText;
    [SerializeField] private Text destinationText;
    [SerializeField] private Text turnText;

    public void Initialize(TravellingUnit tUnit) {
        unitNameText.text = tUnit.unit.unitName;
        destinationText.text = tUnit.destination.placeName;
        turnText.text = Mathf.Clamp(tUnit.turnsToArrival, 0, Mathf.Infinity).ToString();
    }
}
