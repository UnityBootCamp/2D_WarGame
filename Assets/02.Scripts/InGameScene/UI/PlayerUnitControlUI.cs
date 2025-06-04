using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUnitControlUI : MonoBehaviour
{
    // Fields
    [SerializeField] Button[] _levelUnitButton;     // ������ �� �رݵǴ� ����
    [SerializeField] Image[] _levelUnitLockImage;   // ��� �̹���
    [SerializeField] Image[] _levelUnitImage;       // ���� �̹���
    [SerializeField] TextMeshProUGUI[] _levelUnitName;  // ����ִ� ���� �̸�
    [SerializeField] TextMeshProUGUI[] _levelUnitCost;  // ����ִ� ������ �ڽ�Ʈ


    // UnityLifeCycle
    private void Awake()
    {
        // �ε�ȣ ���� �÷��̾� ������ �ִ� ����
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
