using System.Collections;
using Unity.VisualScripting;
using UnityEngine;



public class Unit<T> : MonoBehaviour
{
    [SerializeField] protected T _unitData;

    // 유닛의 기본데이터
    protected float _hp;
    protected float _attackForce;
    protected float _moveSpeed;
    protected float _attackDelay;
    protected Animator _unitAnim;
    protected Vector3 _unitSize;


    // 바로 앞 유닛 관련
    protected Vector3 _prevUnitSize;

    protected bool IsCanMove;

   
    protected IEnumerator C_MoveCool()
    {
        IsCanMove = false;
        yield return new WaitForSeconds(0.5f);
        IsCanMove = true;
    }


}