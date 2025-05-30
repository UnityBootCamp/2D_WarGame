using System;
using System.Collections.Generic;
using UnityEngine;

public class PoolManager : MonoBehaviour
{
    public static PoolManager Instance { get; private set; }

    private readonly Dictionary<string, IPool> poolDict = new();

    // 외부에서 주입받는 프리팹 로딩 함수 (Resource, Addressables 등과 분리 가능)
    public Func<string, GameObject> resourceLoader;

    // 싱글톤 초기화
    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    // 새로운 오브젝트 풀 생성 (프리팹 생성 함수를 받아 미리 preloadCount만큼 생성)
    public void CreatePool(string key, Func<GameObject> factory, int preloadCount = 50)
    {
        if (poolDict.ContainsKey(key))
            return;

        // 해당 풀에 오브젝트들을 담을 부모 오브젝트 생성
        var root = new GameObject($"{key}_PoolRoot").transform;
        root.SetParent(transform);

        // 풀 생성 및 초기화
        var pool = new ObjectPool { Trans = root };
        pool.Preload(factory, preloadCount);


        // 딕셔너리에 등록
        poolDict.Add(key, pool);
    }

 

    // 오브젝트를 풀에서 꺼내 반환 (경로를 통해 접근)
    public GameObject Get(string key, Action<GameObject> onGet = null)
    {
        if (!poolDict.TryGetValue(key, out var pool))
        {
            Debug.LogWarning($"No pool found for key: {key}");
            return null;
        }

        return pool.Get(onGet);
    }

    // 오브젝트를 풀에 반환
    public void Release(string key, GameObject obj, Action<GameObject> onRelease = null)
    {
        if (!poolDict.TryGetValue(key, out var pool))
        {
            Debug.LogWarning($"No pool found for key: {key}");
            return;
        }

        pool.Release(obj, onRelease);
    }

    // 편의 함수: 리소스 경로 기반으로 오브젝트 풀 생성
    public void CreatePoolFromPath(string path, int preloadCount = 5)
    {
        CreatePool(path, () => {
            var obj = resourceLoader?.Invoke(path);
            if (obj == null)
                throw new Exception($"Failed to load prefab at path: {path}");
            return obj;
        }, preloadCount);
    }
}