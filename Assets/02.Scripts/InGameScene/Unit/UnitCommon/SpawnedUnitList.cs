
using System.Collections.Generic;
using System.Diagnostics;

public class SpawnedUnitList<T>
{
    public List<int> UnitsCount; 

    public Queue<T> SpawnedBattleUnit;


    public SpawnedUnitList()
    {
        UnitsCount = new List<int>(6);
        SpawnedBattleUnit = new Queue<T>(300);

        for (int i = 0; i < 6; i++)
        {
            UnitsCount.Add(0);
        }

    }


    public void EnqueueUnitList(T unit)
    {
        SpawnedBattleUnit.Enqueue(unit);
    }

    public void DequeueUnitList()
    {
        SpawnedBattleUnit.Dequeue();
    }
}