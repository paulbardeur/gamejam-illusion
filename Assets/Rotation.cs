using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotation : MonoBehaviour
{

    public float rotationSpeed = 100f;

    void Start()
    {
        
    }

    void Update()
    {
        transform.rotation = Quaternion.Euler(45f, transform.eulerAngles.y + rotationSpeed * Time.deltaTime, 0f);
    }
}
