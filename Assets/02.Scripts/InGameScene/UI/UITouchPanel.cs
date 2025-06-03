using UnityEngine;
using UnityEngine.EventSystems;

public class UITouchPanel : MonoBehaviour, IPointerClickHandler
{
    public void OnPointerClick(PointerEventData eventData)
    {
        if(eventData != null)
        {
            GameManager.Instance.ExplainPanel.MoveToNextPanel();
        }

    }

}
