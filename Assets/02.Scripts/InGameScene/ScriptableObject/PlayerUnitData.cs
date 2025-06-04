using UnityEngine;

// 플레이어 유닛 데이터
[CreateAssetMenu(fileName = "PlayerUnit", menuName = "Scriptable Objects/Unit/PlayerUnit")]
public class PlayerUnitData : UnitData
{
    [field: SerializeField] public Sprite UnitPortrait { get; set; }        // 유닛 초상화
    [field: SerializeField] public PlayerUnitType UnitType { get; set; }
}
