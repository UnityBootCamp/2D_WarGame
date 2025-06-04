using UnityEngine;

public class EnemyUnitSize : UnitSize
{
    // Fields
    [SerializeField] GameObject[] _enemyUnits;
    [HideInInspector] public static Vector3[] EnemyUnitSizes;


    // UnityLifeCycle
    private void Awake()
    {
        EnemyUnitSizes = new Vector3[_enemyUnits.Length];

        for (int i = 0; i < _enemyUnits.Length; i++)
        {
            EnemyUnitSizes[i] = CalcSpriteScale(_enemyUnits[i]);
        }
    }
}