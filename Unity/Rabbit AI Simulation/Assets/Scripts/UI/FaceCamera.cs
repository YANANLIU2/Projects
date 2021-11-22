using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FaceCamera : MonoBehaviour
{
    void Update()
    {
        Vector3 camera_pos = Camera.main.transform.position;
        Vector3 v = camera_pos - transform.position;
        v.x = v.z = 0.0f;
        transform.LookAt(camera_pos - v);
        transform.Rotate(0, 180, 0);
    }
}
