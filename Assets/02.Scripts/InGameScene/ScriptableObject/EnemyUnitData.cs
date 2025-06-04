using UnityEngine;

// 적 유닛 데이터
[CreateAssetMenu(fileName = "EnemyUnit", menuName = "Scriptable Objects/Unit/EnemyUnit")]
public class EnemyUnitData : UnitData
{
    [field: SerializeField] public EnemyUnitType UnitType { get; set; }
    [field: SerializeField] public int Weight { get; set; }                 // 랜덤 생산에 사용되는 가중치
}
