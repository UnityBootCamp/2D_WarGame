using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UnitUpgradeUI : UpgradeUI, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] protected Image _infoUI;
    [SerializeField] protected TextMeshProUGUI _upgradeValue;
    [SerializeField] protected TextMeshProUGUI _upgradeDesc;
    [SerializeField] protected Image _icon;


    protected virtual void Start()
    {
        _upgradeCostText.text = PlayerSpawnManager.Instance.PlayerUnitSpawner.UnitUpgradeCost.ToString();
        _upgradeValue.text = "+0";
        _upgradeDesc.text = "0% ¡ı∞°";
        _infoUI.gameObject.SetActive(false);
        AbleUpgradeButton();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (eventData != null)
        {
            _infoUI.gameObject.SetActive(true);
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (eventData != null)
        {
            _infoUI.gameObject.SetActive(false);
        }
    }

    public void AbleUpgradeButton()
    {
        _upgradeButton.gameObject.transform.GetChild(0).GetComponent<TextMeshProUGUI>().color = new Color(1f, 1f, 1f, 1f);
        _lockImage.gameObject.SetActive(false);
        _icon.color = new Color(1f, 1f, 1f, 1f);
        _upgradeButton.gameObject.transform.GetChild(1).gameObject.SetActive(true);
        _upgradeButton.interactable = true;
    }

    public void DisableUpgradeButton()
    {
        _upgradeButton.gameObject.transform.GetChild(0).GetComponent<TextMeshProUGUI>().color = new Color(1f, 1f, 1f, 0.2f);
        _lockImage.gameObject.SetActive(true);
        _icon.color = new Color(1f, 1f, 1f, 0.2f);
        _upgradeButton.gameObject.transform.GetChild(1).gameObject.SetActive(false);
        _upgradeButton.interactable = false;
    }


}
