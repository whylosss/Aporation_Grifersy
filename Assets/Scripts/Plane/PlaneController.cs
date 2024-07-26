using UnityEngine;
<<<<<<< HEAD
<<<<<<< HEAD
using UnityEngine.UI;
public class PlaneController : MonoBehaviour , IService
=======
public class PlaneController : MonoBehaviour 
>>>>>>> parent of a7405a9b (dwad)
=======
public class PlaneController : MonoBehaviour 
>>>>>>> parent of a7405a9b (dwad)
{
    public float FlySpeed;
    [SerializeField] private float _maxSpeed = 120f;
    [SerializeField] private float _minSpeed = 70f;
    [SerializeField] private float _startSpeed = 50f;

    public float Force = 0.08f;

    [SerializeField] private float _pitchAngle = 70f;
    [SerializeField] private float _rollAngle = 45f;

    [SerializeField] private Transform _leftFlap;
    [SerializeField] private Transform _rightFlap;

    [SerializeField] private Transform _leftWing;
    [SerializeField] private Transform _rightWing;
    [SerializeField] private float _wingSpeed = 2f;

    [SerializeField] private float _flapSpeed = 15f;
    [SerializeField] private float _flapAngle = 20f;

    [SerializeField] private Text _forceText;
    [SerializeField] private Text _speedText;

    private float _pitchSmoothness;
    private float _rollSmoothness;

    private float _rollSpeed;
    private float _pitchSpeed;

    private float _horizontalMovement;
    private float _amount = 120;

    public bool IsClosed = false;
    public bool IsStarted = false;
    public bool IsSet = false;
    public bool CanFly = false;

    public Rigidbody Rb;
    private Animator _animator;

    private void Awake()
    {
        Rb = GetComponent<Rigidbody>();
        _animator = GetComponent<Animator>();
        FlySpeed = 0f;
        Rb.useGravity = true;
        IsClosed = false;
        CanFly = false;
        IsSet = false;
        IsStarted = false;
    }

    private void FixedUpdate()
    {
        if (FlySpeed >= _startSpeed)
        {
            if(!IsSet)
            {
                Rb.useGravity = false;
                Force = 0.5f;
                IsSet = true;
                IsStarted = true;
                CanFly = true;
            }
        }

        if (CanFly)
            Movement();

        if (Input.GetKey(KeyCode.Space) && FlySpeed < _maxSpeed)
            FlySpeed += Force;


        if (Input.GetKey(KeyCode.Space) && FlySpeed > 80)
        {
            if(_leftWing.localRotation.y >= -45 && _rightWing.localRotation.y <= 45)
            {
                Quaternion leftWingTargetRotation = Quaternion.Euler(-180, -45, 0);
                Quaternion rightWingTargetRotation = Quaternion.Euler(0, 45, 0);

                _leftWing.localRotation = Quaternion.Lerp(_leftWing.localRotation, leftWingTargetRotation, Time.deltaTime * _wingSpeed);
                _rightWing.localRotation = Quaternion.Lerp(_rightWing.localRotation, rightWingTargetRotation, Time.deltaTime * _wingSpeed);
            }
        }

        if (Input.GetKey(KeyCode.LeftShift))
        {
            if (_leftWing.localRotation.y <= -12 && _rightWing.localRotation.y >= 12)
            {
                Quaternion leftWingTargetRotation = Quaternion.Euler(-180, -12, 0);
                Quaternion rightWingTargetRotation = Quaternion.Euler(0, 12, 0);

                _leftWing.localRotation = Quaternion.Lerp(_leftWing.localRotation, leftWingTargetRotation, Time.deltaTime * _wingSpeed);
                _rightWing.localRotation = Quaternion.Lerp(_rightWing.localRotation, rightWingTargetRotation, Time.deltaTime * _wingSpeed);
            }
        }


        if (Input.GetKey(KeyCode.LeftShift) && FlySpeed > _minSpeed + 1f)
            FlySpeed -= Force;

        transform.Translate(Vector3.forward * FlySpeed * Time.deltaTime);

    }

    private void Update()
    {
        int forceUi = (int)FlySpeed / 3;
        int speedUi = (int)FlySpeed * 4;
        _forceText.text = forceUi.ToString() + " %";
        _speedText.text = speedUi.ToString() + " km/h";

        if (FlySpeed <= 70f)
        {
            _pitchSmoothness = FlySpeed / 20f;
            _rollSmoothness = FlySpeed / 20f;
            _rollSpeed = _rollSmoothness / 10f;
            _pitchSpeed = _pitchSmoothness / 20f;
        }

        else if (FlySpeed > 70f)
        {
            _pitchSmoothness = 3.5f;
            _rollSmoothness = 3.5f;
            _rollSpeed = 0.35f;
            _pitchSpeed = 0.175f;
        }

        if (Input.GetKeyDown(KeyCode.C))
        {
            if (!IsClosed && FlySpeed >= 60f)
            {
                _animator.SetBool("CloseWheels", true);
                IsClosed = true;   
            }

            else
            {
                _animator.SetBool("CloseWheels", false);
                IsClosed = false;
            }
        }

        if (Input.GetKey(KeyCode.W))
        {
            Quaternion leftFlapTargetRotation = Quaternion.Euler(-_flapAngle, 0, 0);
            Quaternion rightFlapTargetRotation = Quaternion.Euler(-200, 0, 0);

            _leftFlap.localRotation = Quaternion.Lerp(_leftFlap.localRotation, leftFlapTargetRotation, Time.deltaTime * _flapSpeed);
            _rightFlap.localRotation = Quaternion.Lerp(_rightFlap.localRotation, rightFlapTargetRotation, Time.deltaTime * _flapSpeed);
        }

        if (Input.GetKey(KeyCode.S))
        {
            Quaternion leftFlapTargetRotation = Quaternion.Euler(_flapAngle, 0, 0);
            Quaternion rightFlapTargetRotation = Quaternion.Euler(-160, 0, 0);

            _leftFlap.localRotation = Quaternion.Lerp(_leftFlap.localRotation, leftFlapTargetRotation, Time.deltaTime * _flapSpeed);
            _rightFlap.localRotation = Quaternion.Lerp(_rightFlap.localRotation, rightFlapTargetRotation, Time.deltaTime * _flapSpeed);
        }

        if (!Input.GetKey(KeyCode.S) && !Input.GetKey(KeyCode.W))
        {
            Quaternion leftFlapTargetRotation = Quaternion.Euler(0, 0, 0);
            Quaternion  rightFlapTargetRotation = Quaternion.Euler(-180, 0, 0);

            _leftFlap.localRotation = Quaternion.Lerp(_leftFlap.localRotation, leftFlapTargetRotation, Time.deltaTime * _flapSpeed);
            _rightFlap.localRotation = Quaternion.Lerp(_rightFlap.localRotation, rightFlapTargetRotation, Time.deltaTime * _flapSpeed);
        }
    }

    private void Movement()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        _horizontalMovement += horizontal * _amount * _rollSpeed * Time.deltaTime;

        float targetVerticalMovement = Mathf.LerpAngle(0, _pitchAngle, Mathf.Abs(vertical)) * Mathf.Sign(vertical);
        float verticalMovement = Mathf.LerpAngle(transform.localRotation.eulerAngles.x, targetVerticalMovement, Time.deltaTime * _pitchSmoothness * _pitchSpeed);

        float targetRoll = Mathf.LerpAngle(0, _rollAngle, Mathf.Abs(horizontal)) * -Mathf.Sign(horizontal);
        float roll = Mathf.LerpAngle(transform.localRotation.eulerAngles.z, targetRoll, Time.deltaTime * _rollSmoothness);

        transform.localRotation = Quaternion.Euler(verticalMovement, _horizontalMovement, roll);
    }
}