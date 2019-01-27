using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bounce : MonoBehaviour
{
    Vector3 startPosition;
    public float speed = 0.5f;
    public float height = 0.1f;

    void Start()
    {
        startPosition = transform.localPosition;
    }

    // Update is called once per frame
    void Update()
    {
        transform.localPosition = new Vector3 (
            transform.localPosition.x,
            startPosition.y + Mathf.Sin(Time.time / speed) * height,
            transform.localPosition.z
        );
    }
}
