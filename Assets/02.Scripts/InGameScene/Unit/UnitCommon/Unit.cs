using System.Collections;
using Unity.VisualScripting;
using UnityEngine;



public class Unit<T> : MonoBehaviour
{
    [SerializeField] protected T _unitData;

    // ������ �⺻������
    public float _hp;                   // �׽�Ʈ ���� protected�� ������ ��
    public float _attackForce;
    protected float _moveSpeed;
    protected float _attackDelay;
    protected Animator _unitAnim;
    protected Vector3 _unitSize;
    protected SpriteRenderer[] _spriteRenderers;        // SPUM���� ������ ��������Ʈ�� �����ϱ� ���� ����
    protected Color[] _originalColors;

    // �ٷ� �� ���� ����
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