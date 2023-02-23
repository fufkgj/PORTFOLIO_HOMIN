using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyGizmos : MonoBehaviour
{
    public  enum Type { NORMAL,SPAWNPOINT}
    public Type type = Type.NORMAL;
    public const string SpawnPointFile = "Enemy_1";

    public Color _color = Color.green;
    public float _radius = 0.5f;

    private void OnDrawGizmos() // 기즈모 색상 입히는 콜백함수 
    {
        if (type == Type.NORMAL)
        {
            Gizmos.color = _color;
            Gizmos.DrawSphere(transform.position, _radius);
        }
        else
        {
            Gizmos.color = _color;
            Gizmos.DrawIcon(transform.position + (Vector3.up * 1.0f), SpawnPointFile, true);
            Gizmos.DrawWireSphere(transform.position, _radius);
        }

    }
}
