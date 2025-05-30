

public class PlayerSpawnedUnitList : SpawnedUnitList<PlayerUnit>
{


    public int TotalUnitCount()
    {
        int res = 0;

        for (int i = 1; i < UnitsCount.Count; i++)
        {
            res += UnitsCount[i];
        }

        return res;
    }

    public int TotalFarmingUnitCount()
    {
        return UnitsCount[0];
    }

}