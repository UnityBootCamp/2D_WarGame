using UnityEngine;

public class PlayerUnitSize : UnitSize
{
    // Fields
    [SerializeField]GameObject[] _playerUnits;                  // 게임에 등장하는 모든 플레이어 유닛 프리팹 참조
    [HideInInspector] public static Vector3[] PlayerUnitSizes;  // 사이즈를 저장하고 제공하는 변수


    // UnityLifeCycle
    private void Awake()
    {
        PlayerUnitSizes = new Vector3[_playerUnits.Length]; 

        for (int i = 0; i < _playerUnits.Length; i++)
        {
            PlayerUnitSizes[i] = CalcSpriteScale(_playerUnits[i]);
        }
    }


}