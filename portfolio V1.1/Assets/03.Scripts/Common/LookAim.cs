using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAim : MonoBehaviour
{
    public Transform target;
    private Transform tr;
    void Start()
    {
        tr = GetComponent<Transform>();
    }

    // Update is called once per frame
    void Update()
    {
        tr.LookAt(target.position);
    }
}
