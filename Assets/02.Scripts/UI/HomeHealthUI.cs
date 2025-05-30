using UnityEngine;
using UnityEngine.UI;

public class HomeHealthUI : MonoBehaviour
{
    Slider _playerHomeHealthSlider;
    Slider _enemyHomeHealthSlider;

    private void Awake()
    {
        _playerHomeHealthSlider = transform.GetChild(0).GetComponent<Slider>();
        _enemyHomeHealthSlider = transform.GetChild(1).GetComponent<Slider>();
    }

    public void SetPlayerHomeHealth(float value)
    {
        _playerHomeHealthSlider.value = value;
    }
    public void SetEnemyHomeHealth(float value)
    {
        _enemyHomeHealthSlider.value = value;
    }
}