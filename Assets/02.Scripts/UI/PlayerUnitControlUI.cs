using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUnitControlUI : MonoBehaviour
{

    // 레벨업 획득 유닛
    [SerializeField] Button[] _levelUnitButton;
    [SerializeField] Image[] _levelUnitLockImage;   // 잠금 이미지
    [SerializeField] Image[] _levelUnitImage;       // 유닛 이미지

    [SerializeField] TextMeshProUGUI[] _levelUnitName;
    [SerializeField] TextMeshProUGUI[] _levelUnitCost;


    // 업그레이드 버튼
    [SerializeField] Image _upgradeLockImage;   // 잠금 이미지
    [SerializeField] Button _upgradeButton;     // 업그레이드 버튼

    private void Awake()
    {
        AbleUpgradeButton();

        // 부등호 값은 플레이어 본진의 최대 레발
        for (int i=0; i<2; i++)
        {
            DisableLevelUnit(i);
        }
    }

    public void OnSpawn(int index)
    {
        PlayerSpawnManager.Instance.SpawnQueueUI.OnSpawn(index);

    }

    public void AbleLevelUnit(int level)
    {

        _levelUnitButton[level].interactable = true;
        _levelUnitLockImage[level].gameObject.SetActive(false);
        _levelUnitName[level].color = Color.white;
        _levelUnitCost[level].color = Color.white;
        _levelUnitImage[level].color = Color.white;
    }


    public void DisableLevelUnit(int level)
    {
        _levelUnitButton[level].interactable = false;
        _levelUnitLockImage[level].gameObject.SetActive(true);
        _levelUnitName[level].color = new Color(1f, 1f, 1f, 0.2f);
        _levelUnitCost[level].color = new Color(1f, 1f, 1f, 0.2f);
        _levelUnitImage[level].color = new Color(1f, 1f, 1f, 0.2f);
    }


    public void AbleUpgradeButton()
    {
        _upgradeButton.gameObject.transform.GetChild(0).GetComponent<TextMeshProUGUI>().color = new Color(1f, 1f, 1f, 1f);
        _upgradeLockImage.gameObject.SetActive(false);
        _upgradeButton.gameObject.transform.GetChild(1).gameObject.SetActive(true);
        _upgradeButton.interactable = true;
    }

    public void DisableUpgradeButton()
    {
        _upgradeButton.gameObject.transform.GetChild(0).GetComponent<TextMeshProUGUI>().color = new Color(1f, 1f, 1f, 0.2f);
        _upgradeLockImage.gameObject.SetActive(true);
        _upgradeButton.gameObject.transform.GetChild(1).gameObject.SetActive(false);
        _upgradeButton.interactable = false;
    }


}
