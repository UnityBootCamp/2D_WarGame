using System.Collections.Generic;
using UnityEngine;

public class UnitSpawner<T> : MonoBehaviour
{
    [SerializeField] protected List<T> _units;  //������ �����͸� ��� Scriptable Object

    public List<T> Units => _units;
     
    protected int _maxUnitCount;                    // ���� ������ �������� �ƴ��� �Ǻ��� ���� �ʵ�
    public int MaxUnitCount => _maxUnitCount;




    protected virtual void Start()
    {
        _maxUnitCount = 10;

    }


}
