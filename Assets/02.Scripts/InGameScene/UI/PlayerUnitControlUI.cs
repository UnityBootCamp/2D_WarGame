using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUnitControlUI : MonoBehaviour
{
    // Fields
    [SerializeField] Button[] _levelUnitButton;     // 레벨업 시 해금되는 유닛
    [SerializeField] Image[] _levelUnitLockImage;   // 잠금 이미지
    [SerializeField] Image[] _levelUnitImage;       // 유닛 이미지
    [SerializeField] TextMeshProUGUI[] _levelUnitName;  // 잠겨있는 유닛 이름
    [SerializeField] TextMeshProUGUI[] _levelUnitCost;  // 잠겨있는 유닛의 코스트


    // UnityLifeCycle
    private void Awake()
    {
        // 부등호 값은 플레이어 본진의 최대 레벨
        for (int i=0; i<2; i++)
        {
            DisableLevelUnit(i);
        }
    }


    // Methods
    public void OnSpawn(int index)
    {
        SoundManager.Instance.ClickUI();
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
}
