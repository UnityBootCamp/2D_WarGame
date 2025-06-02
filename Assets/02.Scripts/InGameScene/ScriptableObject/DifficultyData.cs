using UnityEngine;


// ���̵��� ���� �޶����� ��ġ
[CreateAssetMenu(fileName = "DifficultyData", menuName = "Scriptable Objects/DifficultyData")]
public class DifficultyData : ScriptableObject
{
    [field: SerializeField] public float DifficultyCoefficient { get; set; } // ���̵� ���� �ɷ�ġ ����
    [field: SerializeField] public int EnemyMineralEarn { get; set; }      // 1�ʴ� ���� ��� �̳׶��� ��
    [field: SerializeField] public float EnemyHomeHp { get; set; }           // ���� ���� ü��
}
