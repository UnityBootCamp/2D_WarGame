public class EnemySpawnedUnitList : SpawnedUnitList<EnemyUnit>
{
    // Methods
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