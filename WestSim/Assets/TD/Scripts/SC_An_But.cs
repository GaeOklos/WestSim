using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class SC_An_But : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public Animator animatorButton;

    [SerializeField]
    private bool _isButChangerScene = true;

    private bool _isHovered = false;


    
    public void ClickOnBut() {
        if (_isButChangerScene)
            animatorButton.SetTrigger("Trigger_Click");
    }

    

    private void ChangeToHovered() {
        animatorButton.SetTrigger("Trigger_Hovered");
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (_isHovered == false) {
            animatorButton.SetTrigger("Trigger_Hovered");
            _isHovered = !_isHovered;
        }

    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (_isHovered) {
            animatorButton.SetTrigger("Trigger_NoHovered");
            _isHovered = !_isHovered;
        }

    }
}
