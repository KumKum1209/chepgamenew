using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camerafollow : MonoBehaviour
{
    public Transform target;
    public float speed = 10f;
    public Vector3 offset;
    // Start is called before the first frame update
    void Start()
    {
        target = FindObjectOfType<PlayerControler>().transform;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        transform.position = Vector3.Lerp(transform.position,target.position + offset,Time.deltaTime * speed);
    }
}
