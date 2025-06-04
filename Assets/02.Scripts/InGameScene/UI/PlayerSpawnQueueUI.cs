using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerSpawnQueueUI : MonoBehaviour
{
    // Fields
    [SerializeField] List<Image> _unitPortraits; // ���� �ʻ�ȭ �����ص� ����Ʈ
    [SerializeField] Slider _spawnSlider;        // �������ð��� ����ȭ�� �����̴�
    [SerializeField] Sprite _defaultQueueSprite; // �ƹ� ���ֵ� ���� ��� ĭ�� ��������Ʈ
    Sprite _unitPortrait;                        // ������ ������ ��ų ������ �ʻ�ȭ
    public int WaitingUnits;                     // ������� ����
    const int MAX_WAITING_UNITS = 5;             // �ִ�� ť�� ���� �� �ִ� ���� ��


    // UnityLifeCycle
    private void Awake()
    {
        _spawnSlider.value = 0;
        
        for(int i =0; i<_unitPortraits.Capacity; i++)
        {
            _unitPortraits[i].sprite = _defaultQueueSprite;
        }

    }

    // Methods
    public void SetSlider(float value)
    {
        _spawnSlider.value = value;
    }

    public void OnSpawn(int index)
    {
        //���
        if (index == 0)
        {
            if (WaitingUnits < MAX_WAITING_UNITS && PlayerSpawnManager.Instance.IsCanSpawnFarmingUnit
                    && PlayerSpawnManager.Instance.Mineral - PlayerSpawnManager.Instance.PlayerUnitSpawner.Units[index].Cost >= 0)
            {
                PlayerSpawnManager.Instance.Mineral -= PlayerSpawnManager.Instance.PlayerUnitSpawner.Units[index].Cost;


                _unitPortrait = PlayerSpawnManager.Instance.PlayerUnitSpawner.Units[index].UnitPortrait;
                _unitPortraits[WaitingUnits].sprite = _unitPortrait;
                PlayerSpawnManager.Instance.PlayerSpawnQueue.UnitEnqueue(PlayerSpawnManager.Instance.PlayerUnitSpawner.Units[index]);
                WaitingUnits++;
                PlayerSpawnManager.Instance.UnitList.UnitsCount[index]++;
                PlayerSpawnManager.Instance.UpdateFarmingUnitResourceUI();
            }
        }
        //���� ����
        else
        {
            if (WaitingUnits < MAX_WAITING_UNITS && PlayerSpawnManager.Instance.IsCanSpawnUnit
                 && PlayerSpawnManager.Instance.Mineral - PlayerSpawnManager.Instance.PlayerUnitSpawner.Units[index].Cost >= 0)
            {
                PlayerSpawnManager.Instance.Mineral -= PlayerSpawnManager.Instance.PlayerUnitSpawner.Units[index].Cost;

                _unitPortrait = PlayerSpawnManager.Instance.PlayerUnitSpawner.Units[index].UnitPortrait;
                _unitPortraits[WaitingUnits].sprite = _unitPortrait;
                PlayerSpawnManager.Instance.PlayerSpawnQueue.UnitEnqueue(PlayerSpawnManager.Instance.PlayerUnitSpawner.Units[index]);
                WaitingUnits++;
                PlayerSpawnManager.Instance.UnitList.UnitsCount[index]++;
                PlayerSpawnManager.Instance.UpdateUnitResourceUI();
            }
        }

    }
    
    public void UpdateQueueUI(PlayerUnitType unitType)
    {
        PlayerSpawnManager.Instance.PlayerUnitSpawner.Spawn(unitType);

        for(int i=0; i<_unitPortraits.Count-1; i++)
        {
            _unitPortraits[i].sprite = _unitPortraits[i + 1].sprite;
        }
        _unitPortraits[_unitPortraits.Count-1].sprite = _defaultQueueSprite;
        SetSlider(0);
    }

}
