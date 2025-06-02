using UnityEngine;

public class UnitSize : MonoBehaviour
{


    protected Vector3 CalcSpriteScale(GameObject unit)
    {
        Bounds totalBounds = new Bounds(unit.transform.position, Vector3.zero);

        foreach (var sr in unit.GetComponentsInChildren<SpriteRenderer>())
        {
            totalBounds.Encapsulate(sr.bounds); // 각 스프라이트의 영역을 통합
        }

        return totalBounds.size;
    }
}