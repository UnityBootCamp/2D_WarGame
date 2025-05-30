using UnityEngine;

public class UnitAttackManager : MonoBehaviour
{

    [SerializeField] public PlayerUnit PlayerFirstUnit;
    [SerializeField] public EnemyUnit EnemyFirstUnit;

    #region ½Ì±ÛÅæ
    public static UnitAttackManager Instance => _instance;

    static UnitAttackManager _instance;

    private void Awake()
    {
        _instance = this;

    }
    #endregion
    public void SetPlayerFirstUnit(PlayerUnit gameObject)
    {
        PlayerFirstUnit = gameObject;
    }

    public void SetEnemyFirstUnit(EnemyUnit gameObject)
    {
        EnemyFirstUnit = gameObject;
    }


}