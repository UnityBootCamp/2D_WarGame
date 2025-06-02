using UnityEngine;


// 난이도에 따라 달라지는 수치
[CreateAssetMenu(fileName = "DifficultyData", menuName = "Scriptable Objects/DifficultyData")]
public class DifficultyData : ScriptableObject
{
    [field: SerializeField] public float DifficultyCoefficient { get; set; } // 난이도 적의 능력치 배율
    [field: SerializeField] public int EnemyMineralEarn { get; set; }      // 1초당 적이 얻는 미네랄의 양
    [field: SerializeField] public float EnemyHomeHp { get; set; }           // 적의 본진 체력
}
