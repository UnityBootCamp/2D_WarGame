using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    // Fields
    const int CAMERA_BOUND = 16;            // ī�޶� �̵� �Ѱ� ��
    const int CAMERA_MOVE_SPEED = 10;       // ī�޶� �̵� �ӵ�
    bool _isPause;                          // ���� ������ �Ͻ������� ���������� �����ϴ� bool
    Vector3 _moveDir;                       // ī�޶� �̵� ����

    
    //UnityLifeCycle
    private void Update()
    {
        // ������ ����� ���¶�� ���� x
        if (GameManager.Instance.IsGameOver)
            return;

        Vector3 newPos = transform.position + _moveDir * CAMERA_MOVE_SPEED * Time.deltaTime;    // �̹� �����ӿ� ���� ���ο� ��ǥ

        float newPosX = Mathf.Clamp(newPos.x, -CAMERA_BOUND, CAMERA_BOUND);                     // ��踦 �Ѿ�� �ʵ��� ���ο� ��ǥ�� Clamp
        float newPosY = Mathf.Clamp(newPos.y, -CAMERA_BOUND, CAMERA_BOUND);

        transform.position = new Vector3(newPosX, newPosY, -10f);                               // ī�޶� �̵�


    }


    // Methods
    // Unity InputSystem
    // �̵�
    public void OnMove(InputAction.CallbackContext context)
    {
        _moveDir = context.ReadValue<Vector2>();
    }

    // �Ͻ�����
    public void OnPause(InputAction.CallbackContext context)
    {
        // InputAction �� Ű �ѹ� ������ 3�� (Started, Performed, Canceled �� �� �����. ���� �޼��带 ������ Ÿ�̹��� ��Ȯ�� �ؾ��� �ʿ� ����)
        if (!context.performed) 
            return;


        if (_isPause ==false)
        {
            GameManager.Instance.Pause();
            _isPause = true;
            return; 
        }

        if(_isPause && GameManager.Instance.OptionaPanel.OppendPanel.Count > 0)
        {
            GameManager.Instance.OptionaPanel.ClosePanel();
            return;
        }

        GameManager.Instance.Restart();
        _isPause = false;
    }

}
