using TMPro;
using UnityEngine;

public class PlayerResourceUI : MonoBehaviour
{
    // Fields
    [SerializeField] TextMeshProUGUI _unitResourceText;
    [SerializeField] TextMeshProUGUI _farmingUnitResourceText;
    [SerializeField] TextMeshProUGUI _resourceText;


    // Methods
    public void UpdateUnitResource(string value)
    {
        _unitResourceText.text = value;
    }

    public void UpdateFarmingUnitResource(string value)
    {
        _farmingUnitResourceText.text = value;
    }

    public void UpdateResource(float value)
    {
        _resourceText.text = ((int)value).ToString();
    }
}
