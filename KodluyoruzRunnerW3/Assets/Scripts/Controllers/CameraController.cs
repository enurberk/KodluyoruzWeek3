using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private Transform _target;
    private Vector3 _offset;

    private void Start()
    {
        _offset = _target.position - transform.position;
    }
    private void LateUpdate()
    {
        float desireAngle = _target.eulerAngles.y;
        //Quaternion rotation = Quaternion.Euler(0, desireAngle, 0);
        //transform.position = _target.position - (rotation * _offset);
        transform.position = _target.position - (_offset);
        transform.LookAt(_target);
    }
}
