using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class LongHoldButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler {
    private bool pointerDown;
    private float pointerDownTimer;
    [SerializeField]
    private float requiredHoldTime = 20f;

    public UnityEvent onLongClick;

    
    public void OnPointerDown(PointerEventData eventData) {
        pointerDown = true;
        Debug.Log("OnPointerDown");
    }
    

    public void OnPointerUp(PointerEventData eventData) {
        Reset();
        Debug.Log("OnPointerUp");
    }

    private void UpdateV1() {
        if(pointerDown) {
            pointerDownTimer += Time.deltaTime;
            if(pointerDownTimer >= requiredHoldTime) {
                if(onLongClick != null) {
                    onLongClick.Invoke();
                }
                Reset();
            }else {
                onLongClick.Invoke();
            }
        }
    }

    private void Update() {
        if(pointerDown) {
            pointerDownTimer += Time.deltaTime;
            if (pointerDownTimer <= requiredHoldTime) {
                if(onLongClick != null) {
                    onLongClick.Invoke();
                }
            }
        }else {
            Reset();
        }
        //Reset();
    }

    private void Reset() {
        pointerDown = false;
        pointerDownTimer = 0;
       
    }
}
