using System.Collections.Generic;
using UnityEngine;

public class UnitSpawner<T> : MonoBehaviour
{
    [SerializeField] protected List<T> _units;  //유닛의 데이터를 담는 Scriptable Object

    public List<T> Units => _units;
     
    protected int _maxUnitCount;                    // 유닛 스폰이 가능한지 아닌지 판별을 위한 필드
    public int MaxUnitCount => _maxUnitCount;




    protected virtual void Start()
    {
        _maxUnitCount = 10;

    }


}
