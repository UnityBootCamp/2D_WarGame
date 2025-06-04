using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerSpawnQueueUI : MonoBehaviour
{
    // Fields
    [SerializeField] List<Image> _unitPortraits; // 유닛 초상화 저장해둔 리스트
    [SerializeField] Slider _spawnSlider;        // 스폰대기시간을 가시화한 슬라이더
    [SerializeField] Sprite _defaultQueueSprite; // 아무 유닛도 없는 대기 칸의 스프라이트
    Sprite _unitPortrait;                        // 다음에 생산대기 시킬 유닛의 초상화
    public int WaitingUnits;                     // 대기중인 유닛
    const int MAX_WAITING_UNITS = 5;             // 최대로 큐에 넣을 수 있는 유닛 수


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
        //농부
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
        //전투 유닛
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
