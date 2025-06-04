using TMPro;
using UnityEngine;

public class HomeUpgradeUI : UpgradeUI
{
    // UnityLifeCycle
    private void Start()
    {
        _upgradeButton.onClick.AddListener(OnHomeUpgrade);
        AbleUpgradeButton();
    }

    
    // Methods
    public void SetUpgradeCost(float value)
    {
        _upgradeCostText.text = ((int)value).ToString();
    }

    public void AbleUpgradeButton()
    {
        _upgradeButton.gameObject.transform.GetChild(0).GetComponent<TextMeshProUGUI>().color = new Color(1f, 1f, 1f, 1f);
        _lockImage.gameObject.SetActive(false);
        _upgradeButton.gameObject.transform.GetChild(1).gameObject.SetActive(true);
        _upgradeButton.interactable = true;
    }

    public void DisableUpgradeButton()
    {
        _upgradeButton.gameObject.transform.GetChild(0).GetComponent<TextMeshProUGUI>().color = new Color(1f, 1f, 1f, 0.2f);
        _lockImage.gameObject.SetActive(true);
        _upgradeButton.gameObject.transform.GetChild(1).gameObject.SetActive(false);
        _upgradeButton.interactable = false;
    }

    public void OnHomeUpgrade()
    {
        SoundManager.Instance.ClickUI();
        UnitAttackManager.Instance.PlayerHome.SetHomeNextLevel();
    }

}
