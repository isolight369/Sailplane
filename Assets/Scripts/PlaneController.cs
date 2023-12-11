using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlaneController : MonoBehaviour
{
    [Header("Plane Stats")]
    [Tooltip("How much throttle ramps up and down")]
    public float throttleIncrement = 0.1f;
    [Tooltip("Maximum engine thrust when at 100% throttle")]
    public float maxThrust = 200f;
    [Tooltip("How responsive the plane is when rolling, pitching, and yawing")]
    public float responsiveness = 10f;
    [Tooltip("How much lift force this plane generates as it gains speed")]
    public float lift = 135f;

    private float _throttle;    //Percentage of maximum engine thrust currently being used
    private float _roll;        //Tilting left to right
    private float _pitch;       //Tilting front to back
    private float _yaw;         //"Turning" left to right

    private float _responseModifier //Value used to tweak responsiveness based on plane's mass
    {
        get
        {
            return (rb.mass / 10f) * responsiveness;
        }
    }

    Rigidbody rb;
    [SerializeField] TextMeshProUGUI _hud;
    [SerializeField] Transform _propeller;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void HandleInputs()
    {
        //Set rotational values from our axis inputs
        _roll = Input.GetAxis("Roll");
        _pitch = Input.GetAxis("Pitch");
        _yaw = Input.GetAxis("Yaw");

        //Handle throttle value being sure to clamp it between 0 and 100
        if (Input.GetKey(KeyCode.Space)) _throttle += throttleIncrement;
        else if (Input.GetKey(KeyCode.LeftControl)) _throttle -= throttleIncrement;
        _throttle = Mathf.Clamp(_throttle, 0f, 100f);
    }

    private void Update()
    {
        HandleInputs();
        UpdateHUD();

        _propeller.Rotate(Vector3.right * _throttle);
    }

    private void FixedUpdate()
    {
        //Apply forces to our plane
        rb.AddForce(transform.forward * maxThrust * _throttle);
        rb.AddTorque(transform.up * _yaw * _responseModifier);
        rb.AddTorque(transform.right * _pitch * _responseModifier);
        rb.AddTorque(-transform.forward * _roll * _responseModifier);

        rb.AddForce(Vector3.up * rb.velocity.magnitude * lift);
    }

    private void UpdateHUD()
    {
        _hud.text = "Throttle: " + _throttle.ToString("F0") + "%\n";
        _hud.text += "Airspeed: " + (rb.velocity.magnitude * 3.6f).ToString("F0") + "km/h\n";
        _hud.text += "Altitude: " + transform.position.y.ToString("F0") + "m";
    }
}
