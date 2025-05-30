

public class EnemySpawnedUnitList : SpawnedUnitList<EnemyUnit>
{

    public int TotalUnitCount()
    {
        int res = 0;

        for (int i = 0; i < UnitsCount.Count; i++)
        {
            res += UnitsCount[i];
        }

        return res;
    }

}