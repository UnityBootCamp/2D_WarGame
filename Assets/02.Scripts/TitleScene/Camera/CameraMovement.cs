using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    Vector2 _direction;
    const int CAMERA_BOUND = 14;
    bool _isCamRightWay;

    private void Awake()
    {
        _direction = Vector2.right;
        _isCamRightWay = true;
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(_direction * Time.deltaTime);


        // 오른쪽을 향하면서 오른쪽 bound 에 닿았거나, 왼쪽을 향하면서 왼쪽 bound에 닿았으면 방향 변경
        if ((transform.position.x >= CAMERA_BOUND && _isCamRightWay == true)
            ||( transform.position.x <= -CAMERA_BOUND && _isCamRightWay == false))
        {
            _direction = ChangeDirection(_direction);
        }

    }

    Vector2 ChangeDirection(Vector2 direction)
    {
        _isCamRightWay = !_isCamRightWay;
        return -direction;
    }
}
