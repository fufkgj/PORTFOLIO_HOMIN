using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//마우스로 캐릭터의 시선을 조작하는 스크립트

public class CameraMove : MonoBehaviour 
{
    public float rotSpeed = 200f;

    [SerializeField]
    private float mx;
    [SerializeField]
    private float my;

    public float max = 20;
    private PlayerHealth health;
    private PlayerMove Move;
    void Start()
    {
        health = GetComponent<PlayerHealth>();
        Move = GetComponent<PlayerMove>();
    }
    // Update is called once per frame
    void Update()
    {
        if (!health.isDie && !Move.isWait)
        {
            float h = Input.GetAxis("Mouse X");
            float v = Input.GetAxis("Mouse Y");

            mx += h * rotSpeed * Time.deltaTime;
            my += v * rotSpeed * Time.deltaTime;
            if (my >= max)   //카메라 회전 제한
                my = max;
            else if (my <= -max)
                my = -max;
            transform.eulerAngles = new Vector3(-my, mx, 0);
        }
    }
}
