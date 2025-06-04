public class UnitHealthUpgradeUI :UnitUpgradeUI
{
    // UnityLifeCycle
    protected override void Start()
    {
        base.Start();
        _upgradeButton.onClick.AddListener(OnHealthUpgrade);
    }

    // Methods
    public void OnHealthUpgrade()
    {
        PlayerSpawnManager.Instance.PlayerUnitSpawner.HealthUpgrade();

        SoundManager.Instance.ClickUI();

        _upgradeValue.text = $"+{PlayerSpawnManager.Instance.PlayerUnitSpawner.HealthUpgradeValue}";
        _upgradeDesc.text = $"{10 * PlayerSpawnManager.Instance.PlayerUnitSpawner.HealthUpgradeValue}% ¡ı∞°";

        if (PlayerSpawnManager.Instance.PlayerUnitSpawner.HealthUpgradeValue == 10)
        {
            _lockImage.gameObject.SetActive(true);
            _upgradeButton.interactable = false;
            DisableUpgradeButton();
        }
    }
}
