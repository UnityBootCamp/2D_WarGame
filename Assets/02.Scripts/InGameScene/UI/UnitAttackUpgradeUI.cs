public class UnitAttackUpgradeUI :UnitUpgradeUI
{
    // UnityLifeCycle
    protected override void Start()
    {
        base.Start();
        _upgradeButton.onClick.AddListener(OnAttackUpgrade);
    }


    // Methods
    public void OnAttackUpgrade()
    {
        PlayerSpawnManager.Instance.PlayerUnitSpawner.AttackUpgrade();

        SoundManager.Instance.ClickUI();

        _upgradeValue.text = $"+{PlayerSpawnManager.Instance.PlayerUnitSpawner.AttackUpgradeValue}";
        _upgradeDesc.text = $"{10 * PlayerSpawnManager.Instance.PlayerUnitSpawner.AttackUpgradeValue}% ¡ı∞°";

        if (PlayerSpawnManager.Instance.PlayerUnitSpawner.AttackUpgradeValue == 10)
        {
            _lockImage.gameObject.SetActive(true);
            _upgradeButton.interactable = false;
            DisableUpgradeButton();
        }
    }
}
