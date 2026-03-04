using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour {
    public Vector3 distance = new Vector3(0, 8, -18);
    private Camera cam;
    public Vector3 offset = new Vector3(0, 5f, 0);
    public float speed = 3f;

    void Start () {
        cam = Camera.main;
        Vector3 pos = transform.position;
        Vector3 forward = transform.forward;
        Vector3 initPos = pos - 30 * forward + Vector3.up * 10;
        cam.transform.position = initPos; 
    }

    void LateUpdate () {
        Vector3 pos = transform.position;
        Vector3 forward = transform.forward;
        Vector3 targetPos = pos + forward * distance.z;
        targetPos.y += distance.y;
        Vector3 cameraPos = cam.transform.position;
        cameraPos = Vector3.Lerp(cameraPos, targetPos, Time.deltaTime * speed);
        cam.transform.position = cameraPos;
        cam.transform.LookAt(pos + offset);
    }
}