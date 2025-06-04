using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    // Fields
    const int CAMERA_BOUND = 16;            // 카메라 이동 한계 값
    const int CAMERA_MOVE_SPEED = 10;       // 카메라 이동 속도
    bool _isPause;                          // 현재 게임이 일시정지한 상태인지를 저장하는 bool
    Vector3 _moveDir;                       // 카메라 이동 방향

    
    //UnityLifeCycle
    private void Update()
    {
        // 게임이 종료된 상태라면 실행 x
        if (GameManager.Instance.IsGameOver)
            return;

        Vector3 newPos = transform.position + _moveDir * CAMERA_MOVE_SPEED * Time.deltaTime;    // 이번 프레임에 계산된 새로운 좌표

        float newPosX = Mathf.Clamp(newPos.x, -CAMERA_BOUND, CAMERA_BOUND);                     // 경계를 넘어가지 않도록 새로운 좌표를 Clamp
        float newPosY = Mathf.Clamp(newPos.y, -CAMERA_BOUND, CAMERA_BOUND);

        transform.position = new Vector3(newPosX, newPosY, -10f);                               // 카메라 이동


    }


    // Methods
    // Unity InputSystem
    // 이동
    public void OnMove(InputAction.CallbackContext context)
    {
        _moveDir = context.ReadValue<Vector2>();
    }

    // 일시정지
    public void OnPause(InputAction.CallbackContext context)
    {
        // InputAction 은 키 한번 누를때 3번 (Started, Performed, Canceled 세 번 실행됨. 따라서 메서드를 실행할 타이밍을 명확히 해야할 필요 있음)
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
