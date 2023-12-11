using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    
    [Tooltip("An array of transform representing camera positions")]
    [SerializeField] Transform[] _povs;
    [Tooltip("The speed at which the camera follows the plane")]
    [SerializeField] float _speed;

    private int _index = 1;     //Current POV
    private Vector3 _target;    //camera position

    private void Update()
    {
        //Numbers 1-4 represent different povs 
        if (Input.GetKeyDown(KeyCode.Alpha1)) _index = 0;
        else if (Input.GetKeyDown(KeyCode.Alpha2)) _index = 1;
        else if (Input.GetKeyDown(KeyCode.Alpha3)) _index = 2;
        else if (Input.GetKeyDown(KeyCode.Alpha4)) _index = 3;
        else if (Input.GetKeyDown(KeyCode.Alpha5)) _index = 4;
        else if (Input.GetKeyDown(KeyCode.Alpha6)) _index = 5;

        //Set our target to the relevant POV
        _target = _povs[_index].position;
    }

    private void FixedUpdate ()
    {
        //Move camera to desired position/orientation. Must be in FixedUpdate to avoid camera jitters
        transform.position = Vector3.MoveTowards(transform.position, _target, Time.deltaTime * _speed);
        transform.forward = _povs[_index].forward;
    }
}
