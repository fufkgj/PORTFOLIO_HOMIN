using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletCtrl : MonoBehaviour
{
    public float Speed = 3500f;

    private Transform tr;
    private Rigidbody rb;
    private TrailRenderer trail;

    public GameObject Spark;

    void Awake()
    {
        tr = GetComponent<Transform>();
        rb = GetComponent<Rigidbody>();
        trail = GetComponent<TrailRenderer>();
    }
    void OnEnable()
    {
        rb.AddForce(transform.forward * Speed); //오브젝트가 켜지면 앞방향으로 힘을 가해줌
        Invoke("BulletActive", 5.0f); // 5초후 BulletActive 실행
    }
    void OnDisable()
    {
        trail.Clear(); // trail 제거
        tr.position = Vector3.zero; //오브젝트 위치 초기화
        tr.rotation = Quaternion.identity; // 오브젝트 회전 초기화
        rb.Sleep();
    }
    void BulletActive()
    {
        this.gameObject.SetActive(false);
    }
    private void Update()
    {
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Tile" || other.gameObject.tag == "Stage") //오브젝트가  Tile이나 Stage에 충돌 할 경우 
        {
            Vector3 pos = transform.position;
            Quaternion rot = transform.rotation;
            Instantiate(Spark, pos, rot); // 충돌 위치에 Spark 오브젝트 생성
            BulletActive();
        }
        if (other.gameObject.tag == "Enemy") 
        {
            BulletActive(); 
        }
    }
}
