using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerSpawnQueueUI : MonoBehaviour
{
    [SerializeField] List<Image> _unitPortraits;
    [SerializeField] Slider _spawnSlider;
    [SerializeField] Sprite _defaultQueueSprite;

    
    PlayerSpawnQueue _spawnQueue;

    Sprite _unitPortrait;

    public int WaitingUnits;


    private void Awake()
    {
        _spawnSlider.value = 0;
        
        for(int i =0; i<_unitPortraits.Capacity; i++)
        {
            _unitPortraits[i].sprite = _defaultQueueSprite;
        }

    }

    public void SetSlider(float value)
    {
        _spawnSlider.value = value;
    }

    public void OnSpawn(int index)
    {
        //≥Û∫Œ
        if (index == 0)
        {
            if (WaitingUnits < 5 && PlayerSpawnManager.Instance.IsCanSpawnFarmingUnit
                && PlayerSpawnManager.Instance.Mineral - PlayerSpawnManager.Instance.PlayerUnitSpawner.Units[index].Cost>=0)
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
        //¿¸≈ı ¿Ø¥÷
        else
        {
            if (WaitingUnits < 5 && PlayerSpawnManager.Instance.IsCanSpawnUnit
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
