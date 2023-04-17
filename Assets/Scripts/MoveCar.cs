using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveCar : MonoBehaviour
{
    float moveSpeed = 2f;
    GameObject heart;
    void Start()
    {
        heart = GameObject.Find("World/Car(Clone)/Heart");
    }
    void Update()
    {
        gameObject.transform.position += new Vector3(0f, 0f, moveSpeed) * Time.deltaTime;
        heart.transform.Rotate(new Vector3(0f, 0f, 90f) * Time.deltaTime);
    }
}
