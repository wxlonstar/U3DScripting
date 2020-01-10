using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class GoButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler {
    bool ButtonDown = false;
    float buttonHoldingTimer = 0f;
    [SerializeField]
    float minHoldTime = 20f;

    public UnityEvent onHolding;

    public CamController camraController;

    void Update() {
        if(ButtonDown) {
            buttonHoldingTimer += Time.deltaTime;
            if(buttonHoldingTimer <= minHoldTime) {
                if(onHolding != null) {
                    onHolding.Invoke();
                }
            }
        }else {
            ResetButton();
        }
    }

    public void OnPointerDown(PointerEventData eventData) {
        ButtonDown = true;
        //Debug.Log("Holding");
    }

    public void OnPointerUp(PointerEventData eventData) {
        ResetButton();
        //Debug.Log("Released");
    }

    void ResetButton() {
        ButtonDown = false;
        buttonHoldingTimer = 0;
    }
}
