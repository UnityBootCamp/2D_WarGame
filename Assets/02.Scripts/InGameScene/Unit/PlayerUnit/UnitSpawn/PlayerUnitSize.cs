using UnityEngine;

public class PlayerUnitSize : UnitSize
{
    [SerializeField]GameObject[] _playerUnits;

    [HideInInspector] public static Vector3[] PlayerUnitSizes;


    private void Awake()
    {
        PlayerUnitSizes = new Vector3[_playerUnits.Length]; 


        for (int i = 0; i < _playerUnits.Length; i++)
        {
            PlayerUnitSizes[i] = CalcSpriteScale(_playerUnits[i]);
        }
    }


}