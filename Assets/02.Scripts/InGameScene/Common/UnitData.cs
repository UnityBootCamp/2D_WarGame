using System;
using UnityEngine;


public class UnitData : ScriptableObject
{
    [field: SerializeField] public GameObject UnitPrefab { get; set; }      // À¯´Ö ÇÁ¸®ÆÕ
    [field: SerializeField] public int Cost { get; set; }                   // À¯´Ö ÄÚ½ºÆ®
    [field: SerializeField] public float SpawnCoolDown { get; set; }        // À¯´Ö »ý»ê´ë±â ½Ã°£
    [field: SerializeField] public float Hp { get; set; }                   // À¯´Ö Ã¼·Â
    [field: SerializeField] public float AttackForce { get; set; }          // À¯´Ö °ø°Ý·Â
    [field: SerializeField] public float MoveSpeed { get; set; }            // À¯´Ö ÀÌµ¿¼Óµµ
    [field: SerializeField] public float AttackDelay { get; set; }          // À¯´Ö °ø°Ýµô·¹ÀÌ


    
}
