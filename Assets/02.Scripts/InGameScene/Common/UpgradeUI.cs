using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UpgradeUI : MonoBehaviour
{

    [SerializeField] protected Image _lockImage;
    [SerializeField] protected TextMeshProUGUI _upgradeCostText;

    protected Button _upgradeButton;


    private void Awake()
    {
        _upgradeButton = GetComponent<Button>();
    }

}
