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


        // �������� ���ϸ鼭 ������ bound �� ��Ұų�, ������ ���ϸ鼭 ���� bound�� ������� ���� ����
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
