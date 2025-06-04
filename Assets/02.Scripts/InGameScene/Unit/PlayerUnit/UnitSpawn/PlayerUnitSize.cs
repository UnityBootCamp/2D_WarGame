using UnityEngine;

public class PlayerUnitSize : UnitSize
{
    // Fields
    [SerializeField]GameObject[] _playerUnits;                  // ���ӿ� �����ϴ� ��� �÷��̾� ���� ������ ����
    [HideInInspector] public static Vector3[] PlayerUnitSizes;  // ����� �����ϰ� �����ϴ� ����


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