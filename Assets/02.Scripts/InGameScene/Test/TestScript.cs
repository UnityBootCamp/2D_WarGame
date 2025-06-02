using UnityEngine;
using UnityEngine.InputSystem;

public class TestScript : MonoBehaviour
{
    Animator _playerAnim;
    Vector2 _moveDirection;
    
    public GameObject ParentObject;
    public float _speed;
    public Vector3 PlayerSize;

    void Start()
    {
        ParentObject = transform.parent.gameObject;
        _playerAnim = GetComponent<Animator>();

        Bounds totalBounds = new Bounds(transform.position, Vector3.zero);

        foreach (var sr in GetComponentsInChildren<SpriteRenderer>())
        {
            totalBounds.Encapsulate(sr.bounds); // 각 스프라이트의 영역을 통합
        }

        PlayerSize = totalBounds.size;
    }

    // Update is called once per frame
    void Update()
    {
        ParentObject.transform.Translate(_moveDirection * _speed *  Time.deltaTime);
    }

    public void OnAttack(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            Debug.Log("111111111");
            _playerAnim.SetTrigger("2_Attack");
        }
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        _moveDirection = context.ReadValue<Vector2>();

        if (_moveDirection.x < 0)
        {
            ParentObject.transform.localScale = new Vector3(1, 1, 1); // 왼쪽
        }
        else if (_moveDirection.x > 0)
        {
            ParentObject.transform.localScale = new Vector3(-1, 1, 1);  // 오른쪽
        }

        if (_moveDirection != Vector2.zero)
        {
            _playerAnim.SetBool("1_Move", true);
        }
        else
        {
            _playerAnim.SetBool("1_Move", false);
        }

    }
}
