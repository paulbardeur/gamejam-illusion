using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Levitation : MonoBehaviour
{

    bool isUpping = true;
    int index = 0;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        const float speed = 0.002f;

        if (isUpping && index < 50) {
            transform.position = new Vector3(transform.position.x, transform.position.y + speed, transform.position.z);
            index += 1;
            return;
        }

        if (isUpping) {
            isUpping = false;
        }

        if (index > 0) {
            transform.position = new Vector3(transform.position.x, transform.position.y - speed, transform.position.z);
            index -= 1;
            return;
        }

        isUpping = true;
    }
}
