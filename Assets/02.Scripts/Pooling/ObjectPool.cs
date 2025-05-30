using System;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : IPool
{
    // 풀의 부모 오브젝트 트랜스폼 (씬 계층에서 정리 용도)
    public Transform Trans { get; set; }

    // 오브젝트를 재사용하기 위한 큐 (비활성화된 오브젝트 저장소)
    private Queue<GameObject> pool = new();

    // 같은 오브젝트가 중복으로 Release 되는 것을 방지하기 위한 집합
    private readonly HashSet<GameObject> pooledSet = new();


    // 풀에서 오브젝트를 꺼낼 때 사용
    public GameObject Get(Action<GameObject> onGet = null)
    {
        if (pool.Count == 0)
        {
            Debug.LogWarning("ObjectPool is empty!");
            return null;
        }

        var obj = pool.Dequeue();         // 큐에서 하나 꺼내기
        pooledSet.Remove(obj);            // HashSet에서도 제거
        obj.SetActive(true);              // 활성화
        onGet?.Invoke(obj);               // 후처리 콜백 (초기 위치 지정 등)
        return obj;
    }

    // 오브젝트를 풀에 반환 (비활성화, 부모 재지정 등)
    public void Release(GameObject obj, Action<GameObject> onRelease = null)
    {
        if (pooledSet.Contains(obj))
        {
            Debug.LogWarning("This object is already in the pool.");
            return;
        }

        obj.SetActive(false);             // 비활성화
        obj.transform.SetParent(Trans);   // 부모를 풀로 지정
        pool.Enqueue(obj);                // 큐에 추가
        pooledSet.Add(obj);               // 중복 방지를 위한 추적
        onRelease?.Invoke(obj);           // 후처리 콜백 (상태 초기화 등)
    }

    // 오브젝트를 미리 생성하여 풀에 저장해두는 메서드
    public void Preload(Func<GameObject> factory, int count)
    {
        for (int i = 0; i < count; i++)
        {
            var obj = factory.Invoke();   // 프리팹 생성
            obj.SetActive(false);         // 비활성화
            obj.transform.SetParent(Trans, worldPositionStays : true);
            pool.Enqueue(obj);            // 풀에 추가
            pooledSet.Add(obj);           // 중복 방지용 HashSet에 등록
        }
    }
}