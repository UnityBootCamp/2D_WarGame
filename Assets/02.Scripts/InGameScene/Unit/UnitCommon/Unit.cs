using System.Collections;
using Unity.VisualScripting;
using UnityEngine;



public class Unit<T> : MonoBehaviour
{
    [SerializeField] protected T _unitData;

    // 유닛의 기본데이터
    public float _hp;                   // 테스트 이후 protected로 변경할 것
    public float _attackForce;
    protected float _moveSpeed;
    protected float _attackDelay;
    protected Animator _unitAnim;
    protected Vector3 _unitSize;
    protected SpriteRenderer[] _spriteRenderers;        // SPUM으로 생성한 스프라이트에 접근하기 위한 변수
    protected Color[] _originalColors;

    // 바로 앞 유닛 관련
    protected Vector3 _prevUnitSize;

    protected bool IsCanMove;

    private void Awake()
    {
        _spriteRenderers = GetComponentsInChildren<SpriteRenderer>();
        _originalColors = new Color[_spriteRenderers.Length];

        for (int i = 0; i < _spriteRenderers.Length; i++)
        {
            _originalColors[i] = _spriteRenderers[i].color;
        }
    }


    protected IEnumerator C_MoveCool()
    {
        IsCanMove = false;
        yield return new WaitForSeconds(0.5f);
        IsCanMove = true;
    }


}