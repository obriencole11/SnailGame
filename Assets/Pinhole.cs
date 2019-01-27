using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pinhole : MonoBehaviour
{

    public Transform Camera;

    Vector3 startPosition;
    public float openTime = 2.0f;
    public bool easeOut = true;

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
        float easedSize = EasingFunction.EaseInOutCubic(0.0f, 1.0f, timer) * 10.0f;
        transform.localScale = new Vector3(easedSize, transform.localScale.y, easedSize);

        Vector3 playerPosition = GameManager.Instance.playerObject.transform.position;
        Vector3 cameraPosition = Camera.position;

        Vector3 cameraToPlayer = playerPosition - cameraPosition;
        // Vector3 cameraToHole = cameraPosition - startPosition;

        // Vector3 upVector = Vector3.Dot(transform.up, cameraToPlayer) * transform.up;
        // Vector3 rightVector = Vector3.Dot(transform.right, cameraToPlayer) * transform.right;

        Vector3 upVector = Vector3.ProjectOnPlane(cameraToPlayer, transform.up) * 0.2f;
        // Vector3 rightVector = Vector3.Project(cameraToPlayer, transform.right);
        
        transform.position = startPosition + upVector;
    }
}
