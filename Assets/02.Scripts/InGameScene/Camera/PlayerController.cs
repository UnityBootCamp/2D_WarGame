
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{

    const int CAMERA_BOUND = 16;
    const int CAMERA_MOVE_SPEED = 10;
    Vector3 _moveDir;
    bool _isPause;

    
    private void Update()
    {

        if (GameManager.Instance.IsGameOver)
            return;

        Vector3 newPos = transform.position + _moveDir * CAMERA_MOVE_SPEED * Time.deltaTime;

        float newPosX = Mathf.Clamp(newPos.x, -CAMERA_BOUND, CAMERA_BOUND);
        float newPosY = Mathf.Clamp(newPos.y, -CAMERA_BOUND, CAMERA_BOUND);

        transform.position = new Vector3(newPosX, newPosY, -10f);


    }

    public void OnMove(InputAction.CallbackContext context)
    {
        _moveDir = context.ReadValue<Vector2>();
    }

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
