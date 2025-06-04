using System.Collections;
using UnityEngine;


public class PlayerFarmingUnit : MonoBehaviour
{
    // Fields
    [SerializeField] PlayerUnitData _unitData;
    [SerializeField] GameObject _mineral;

    // ������ �⺻������
    public PlayerUnitType UnitType;
    float _moveSpeed;
    Animator _unitAnim;

    bool IsHoldMineral; // �� ������ �̳׶��� ä���� ������ Ȯ���ϴ� bool
    Coroutine Farming;  // �ڿ�ä�� �ڷ�ƾ�� �����ϴ� ����. �ڷ�ƾ ��ø ����


    // UnityLifeCycle
    private void OnEnable()
    {
        SetData();
        _mineral.SetActive(false);
    }

    private void Start()
    {
        _unitAnim = GetComponent<Animator>();
    }

    private void Update()
    {
        if (GameManager.Instance.IsGameOver)
            return;

        if (IsHoldMineral == false && transform.position.x > -21f)
        {
            transform.parent.Translate(Vector3.left * _moveSpeed * Time.deltaTime);
            transform.parent.localScale = new Vector3(1.5f, 1.5f, 1.5f);
            _unitAnim.SetBool("1_Move", true);
        }
        else if (IsHoldMineral == false && transform.position.x <= -21f)
        {
            if(Farming == null)
            {
                _unitAnim.SetBool("1_Move", false);
                Farming = StartCoroutine(C_Farming());
            }
        }
        else if (IsHoldMineral == true && transform.position.x < -16f)
        {
            transform.parent.Translate(Vector3.right * _moveSpeed * Time.deltaTime);
            transform.parent.localScale = new Vector3(-1.5f, 1.5f, 1.5f);
            _unitAnim.SetBool("1_Move", true);
        }
        else if(IsHoldMineral == true && transform.position.x >= -16f)
        {
            SoundManager.Instance.GetResource();
            IsHoldMineral = false;
            PlayerSpawnManager.Instance.Mineral += 50;
            _mineral.SetActive(false);
        }
       
    }


    // Methods
    IEnumerator C_Farming()
    {
        float cumulativeTime = 0f;
        while (cumulativeTime < 5f)
        {

            _unitAnim.SetTrigger("2_Attack");
            SoundManager.Instance.FarmingResource();
            cumulativeTime ++;
            yield return new WaitForSeconds(1f);
            
        }

        IsHoldMineral = true;
        _mineral.SetActive(true);

        Farming = null;
    }

    void SetData()
    {
        UnitType = _unitData.UnitType;
        _moveSpeed = _unitData.MoveSpeed;
    }

}