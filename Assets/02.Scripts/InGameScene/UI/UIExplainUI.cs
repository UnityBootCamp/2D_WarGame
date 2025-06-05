using UnityEngine;
using UnityEngine.UI;

public class UIExplainUI : MonoBehaviour
{
    // Fields
    [SerializeField] Image[] _explainPanels;
    int _panelIndex;

    
    // Methods
    public void Init()
    {
        gameObject.SetActive(true);

        for (int i = 1; i < _explainPanels.Length; i++)
        {
            _explainPanels[i].gameObject.SetActive(false);
        }

        _explainPanels[_panelIndex].gameObject.SetActive(true);
    }
   
    public void MoveToNextPanel()
    {
        if (_panelIndex >= _explainPanels.Length-1)
        {
            OnSkip();
            return;
        }

        _explainPanels[_panelIndex].gameObject.SetActive(false);
        _panelIndex++;
        _explainPanels[_panelIndex].gameObject.SetActive(true);   
    }

    public void OnSkip()
    {
        _explainPanels[_panelIndex].gameObject.SetActive(false);
        GameManager.Instance.GameStart();
        gameObject.SetActive(false);
    }
}
