using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestSetFloat : MonoBehaviour {
    private Animator animator = null;
    public Transform target;
    public float speed = 2f;

    private float tick = 0;
    private float rate = 1.5f;
    private float current = 0f;
    // Start is called before the first frame update
    void Start() {
        animator = GameObject.FindGameObjectWithTag("MyMan").GetComponent<Animator>();
        Debug.Log(Mathf.Atan2(0, 1) * Mathf.Rad2Deg);
    }

    // Update is called once per frame
    void Update() {
        float h = Input.GetAxis("Horizontal");
        
        float v = Input.GetAxis("Vertical");
        float step = Mathf.PingPong(v, 1);
        //Debug.Log(step);
        
        //target.Translate(0, 0, v * speed * Time.deltaTime);

        transform.Rotate(0, h * speed * Time.deltaTime * 90, 0);

       if(v != 0) {
            if (current == 1) {
                rate *= -1 * 0.6f;
            }

            current = JustOnce(current, rate);

            //current = JustOnce(current, rate);
            animator.SetFloat("DirectionX", current);
            Debug.Log(current);

        }else {
            rate = 1.5f;
            current = 0;
        }
        

    }

    float JustOnce(float currentNum, float rate) {
        
        currentNum = Mathf.MoveTowards(currentNum, 1, rate * Time.deltaTime);
        return Mathf.Clamp01(currentNum);
    }

    

    
}
