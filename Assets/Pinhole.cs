using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pinhole : MonoBehaviour
{

    Vector3 startPosition;
    public float openTime = 2.0f;
    public bool easeOut = true;
    public float maxSize = 25.0f;

    float timer = 0.0f;

    void Awake() {
        startPosition = transform.position;
    }
    
    void Update() {

        if (easeOut) {

            if (timer < 1.0f) {
                timer += Time.deltaTime / openTime;
            }
        } else {
            
            if (timer > 0.0f) {
                timer -= Time.deltaTime / openTime;
            }
        }
        float easedSize = EasingFunction.EaseInOutCubic(0.0f, 1.0f, timer) * maxSize;
        transform.localScale = new Vector3(easedSize, easedSize, easedSize);
         
    }
}
