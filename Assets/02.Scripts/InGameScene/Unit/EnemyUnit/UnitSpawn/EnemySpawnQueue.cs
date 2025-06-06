using System.Collections;
using UnityEngine;

public class EnemySpawnQueue : SpawnQueue<EnemyUnitData>
{
    // UnityLifeCycle
    private void Update()
    {
        if (GameManager.Instance.IsGameOver)
            return;

        if (_queue.Count != 0 && _nextSpawnUnit == null)
        {
            StartCoroutine(C_SpawnCool());
        }
    }

   
    // Methods
    IEnumerator C_SpawnCool()
    {
        float spawnCool = 0f;
        _nextSpawnUnit = _queue.Peek(); // ��ȯ��� ����

        while (spawnCool < _nextSpawnUnit.SpawnCoolDown)
        {
            spawnCool += Time.deltaTime;
            yield return null;
        }

        EnemySpawnManager.Instance.EnemyUnitSpawner.Spawn(_nextSpawnUnit.UnitType);

        _queue.Dequeue();
        _nextSpawnUnit = null;
    }
}
