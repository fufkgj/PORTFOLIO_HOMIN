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
        rb.AddForce(transform.forward * Speed);
        Invoke("BulletActive", 5.0f);
    }
    void OnDisable()
    {
        trail.Clear();
        tr.position = Vector3.zero;
        tr.rotation = Quaternion.identity;
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
        if (other.gameObject.tag == "Tile" || other.gameObject.tag == "Stage")
        {
            Vector3 pos = transform.position;
            Quaternion rot = transform.rotation;
            Instantiate(Spark, pos, rot);
            BulletActive();
        }
        if (other.gameObject.tag == "Enemy")
        {
            BulletActive();
        }
    }
}
