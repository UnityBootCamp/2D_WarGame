using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using static UnityEngine.UI.Image;

public class TestEnemy : MonoBehaviour
{
    public TestScript target;
    Vector3 Size;
    float speed;
    bool isAttacking;
    Animator enemyAnimtor;
    public GameObject ParentObject;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        speed = 1f;
        ParentObject = transform.parent.gameObject;

        enemyAnimtor = GetComponent<Animator>();

        Bounds totalBounds = new Bounds(transform.position, Vector3.zero);  // transform.position : bound 중심, Vector3.zero : 바운드 너비

        // 하위 요소들의 스프라이트 모두 가져와서
        foreach (var sr in GetComponentsInChildren<SpriteRenderer>())
        {
            totalBounds.Encapsulate(sr.bounds); // 각 스프라이트의 영역을 통합. 이 부분은 Scriptable Object 에서 해야 할 듯
        }

        Size = totalBounds.size;
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.position.x + Size.x / 2 < target.transform.position.x - target.PlayerSize.x / 2)
        {
            Vector3 direction = (target.ParentObject.transform.position - ParentObject.transform.position).normalized;

            ParentObject.transform.Translate(direction * Time.deltaTime * speed);
            //transform.position += direction * Time.deltaTime * speed;
            enemyAnimtor.SetBool("1_Move", true);
        }
        else
        {
            enemyAnimtor.SetBool("1_Move", false);

            // 애니메이션 중첩문제 해결
            if (!isAttacking)
            {
                StartCoroutine(AttackRoutine());
            }
        }
    }

    

    IEnumerator AttackRoutine()
    {
        isAttacking = true;
        enemyAnimtor.SetTrigger("2_Attack");

        // 애니메이션 길이 동안 대기 (예: 1.2초)
        yield return new WaitForSeconds(0.25f);

        isAttacking = false;
    }
}
