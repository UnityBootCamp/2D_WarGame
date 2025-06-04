using System;
using System.Collections;
using UnityEngine;


public class LongRangeAttack : MonoBehaviour
{
    // Fields
    [SerializeField] GameObject[] _projectiles;

    // 화살 관련 변수
    float _gravity = -9.8f;
    float _launchAngle = Mathf.PI/10f;
    Vector2 _arrowVelocity;
    Vector2 _enemyArrowVelocity;

    // 메테오 관련 변수
    float _meteorPosY;

    
    // UnityLifeCycle
    void Awake()
    {
        _meteorPosY = 5;

        _arrowVelocity = new Vector2(Mathf.Cos(_launchAngle), Mathf.Sin(_launchAngle));
        _enemyArrowVelocity = new Vector2(-Mathf.Cos(_launchAngle), Mathf.Sin(_launchAngle));

        foreach (ProjectileType projectile in Enum.GetValues(typeof(ProjectileType)))
        {
            PoolManager.Instance.CreatePool(projectile.ToString(), () => Instantiate(_projectiles[(int)projectile]));
        }
    }


    // Methods
    public void ShootArrow(float attackDelay, Vector3 unitPos, Vector3 oppositeUnitPos)
    {
        StartCoroutine(C_ArrowCoroutine(attackDelay, unitPos, oppositeUnitPos));
    }

    public void SummonMeteor(float attackDelay, Vector3 oppositeUnitPos)
    {
        StartCoroutine(C_MeteorCoroutine(attackDelay, oppositeUnitPos));
        
    }

    IEnumerator C_MeteorCoroutine(float attackDelay, Vector3 oppositeUnitPos)
    {

        GameObject meteor = PoolManager.Instance.Get(ProjectileType.Meteor.ToString());
        meteor.transform.position = oppositeUnitPos + Vector3.up * _meteorPosY;

        SoundManager.Instance.SummonMeteor();

        while (meteor.transform.position.y >= oppositeUnitPos.y+1)
        {
            meteor.transform.position += Vector3.down * Time.deltaTime * (_meteorPosY / (attackDelay/2));

            yield return null;
        }

        PoolManager.Instance.Release(ProjectileType.Meteor.ToString(), meteor);
    }

    IEnumerator C_ArrowCoroutine(float attackDelay, Vector3 unitPos, Vector3 oppositeUnitPos)
    {

        yield return new WaitForSeconds(attackDelay / 3);


        if (unitPos.x < oppositeUnitPos.x)
        {
            SoundManager.Instance.PlayerUnitShotArrow();
            Vector2 arrowVelocity = _arrowVelocity * 20 / attackDelay;


            GameObject arrow = PoolManager.Instance.Get(ProjectileType.Arrow.ToString());

            if (arrow == null)
                yield break;

            arrow.transform.position = unitPos + Vector3.up;

            while (arrow.transform.position.y > -3.1f && arrow.transform.position.x <= oppositeUnitPos.x)
            {
                arrowVelocity.y += _gravity * Time.deltaTime;
                arrow.transform.position += (Vector3)(arrowVelocity * Time.deltaTime);

                float angle = Mathf.Atan2(arrowVelocity.y, arrowVelocity.x) * Mathf.Rad2Deg;
                arrow.transform.rotation = Quaternion.Euler(0, 0, angle);

                yield return null;
            }
            PoolManager.Instance.Release(ProjectileType.Arrow.ToString(), arrow);
        }

        else if (unitPos.x >= oppositeUnitPos.x)
        {
            SoundManager.Instance.EnemyUnitShotArrow();
            Vector2 arrowVelocity = _enemyArrowVelocity * 20 / attackDelay;


            GameObject arrow = PoolManager.Instance.Get(ProjectileType.EnemyArrow.ToString());

            if (arrow == null)
                yield break;

            arrow.transform.position = unitPos + Vector3.up;

            while (arrow.transform.position.y > -3.1f && arrow.transform.position.x >= oppositeUnitPos.x)
            {
                arrowVelocity.y += _gravity * Time.deltaTime;
                arrow.transform.position += (Vector3)(arrowVelocity * Time.deltaTime);

                float angle = Mathf.Atan2(arrowVelocity.y, arrowVelocity.x) * Mathf.Rad2Deg;
                arrow.transform.rotation = Quaternion.Euler(0, 0, angle);

                yield return null;
            }
            PoolManager.Instance.Release(ProjectileType.EnemyArrow.ToString(), arrow);
        }

    }

}
