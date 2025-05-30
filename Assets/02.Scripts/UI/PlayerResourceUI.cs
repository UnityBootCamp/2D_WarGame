using TMPro;
using UnityEngine;

public class PlayerResourceUI : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI _unitResourceText;
    [SerializeField] TextMeshProUGUI _farmingUnitResourceText;
    [SerializeField] TextMeshProUGUI _resourceText;


    public void UpdateUnitResource(string value)
    {
        _unitResourceText.text = value;
    }
    public void UpdateFarmingUnitResource(string value)
    {
        _farmingUnitResourceText.text = value;
    }

    public void UpdateResource(int value)
    {
        _resourceText.text = value.ToString();
    }
}
