using UnityEngine;
using UnityEngine.UI;

public class HomeHealthUI : MonoBehaviour
{
    // Fields
    Slider _playerHomeHealthSlider;
    Slider _enemyHomeHealthSlider;


    // UnityLifeCycle
    private void Awake()
    {
        _playerHomeHealthSlider = transform.GetChild(0).GetComponent<Slider>();
        _enemyHomeHealthSlider = transform.GetChild(1).GetComponent<Slider>();
    }

    
    //Methods
    public void SetPlayerHomeHealth(float value)
    {
        _playerHomeHealthSlider.value = value;
    }

    public void SetEnemyHomeHealth(float value)
    {
        _enemyHomeHealthSlider.value = value;
    }
}