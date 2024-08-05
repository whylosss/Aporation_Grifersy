using Game.SeniorEventBus.Signals;
using Game.SeniorEventBus;
using UnityEngine;

public class Striker : MonoBehaviour
{
    [Header("Sound")]
    [SerializeField] private AudioSource _audioSourceDrive;
    [SerializeField] private AudioSource _audioSourceIdle;
    [SerializeField] private AudioSource _audioSourceStart;
    private AudioListener _audioListener;

    [Header("Drive")]
    [SerializeField] private Transform _transformFR;
    [SerializeField] private Transform _transformFMR;
    [SerializeField] private Transform _transformBMR;
    [SerializeField] private Transform _transformBR;
    [SerializeField] private Transform _transformFL;
    [SerializeField] private Transform _transformFML;
    [SerializeField] private Transform _transformBML;
    [SerializeField] private Transform _transformBL;

    [SerializeField] private WheelCollider _colliderFR;
    [SerializeField] private WheelCollider _colliderFMR;
    [SerializeField] private WheelCollider _colliderBMR;
    [SerializeField] private WheelCollider _colliderBR;
    [SerializeField] private WheelCollider _colliderFL;
    [SerializeField] private WheelCollider _colliderFML;
    [SerializeField] private WheelCollider _colliderBML;
    [SerializeField] private WheelCollider _colliderBL;

    [Header("Stats")]
    [SerializeField] private float _forwardSpeed = 750f;
    [SerializeField] private float _maxAngle = 30f;
    [SerializeField] private float _brakeTorque = 5000f;
    [SerializeField] private float _antiRoll = 5000f;

    private EventBus _eventBus;
    [SerializeField] private Rigidbody _rb;

    private bool _crabMode = false;
    private bool _frontWheelDriveMode = true;
    private bool _breakeMode = false;

    public void Init()
    {
        //_eventBus = ServiceLocator.Current.Get<EventBus>();
        //_camera.enabled = false;
        //this.enabled = false;
        //_rb = GetComponent<Rigidbody>();
        //_audioListener = GetComponentInChildren<AudioListener>();
        //_audioListener.enabled = false;
    }

    private void FixedUpdate()
    {
        Move();
        AntiRollBar(_colliderFR, _colliderFL);
        AntiRollBar(_colliderFMR, _colliderFML);
        AntiRollBar(_colliderBMR, _colliderBML);
        AntiRollBar(_colliderBR, _colliderBL);
    }


    protected void Move()
    {
        _colliderFR.motorTorque = Input.GetAxis("Vertical") * -_forwardSpeed;
        _colliderFL.motorTorque = Input.GetAxis("Vertical") * -_forwardSpeed;
        _colliderFMR.motorTorque = Input.GetAxis("Vertical") * -_forwardSpeed;
        _colliderFML.motorTorque = Input.GetAxis("Vertical") * -_forwardSpeed;
        _colliderBR.motorTorque = Input.GetAxis("Vertical") * -_forwardSpeed;
        _colliderBL.motorTorque = Input.GetAxis("Vertical") * -_forwardSpeed;
        _colliderBMR.motorTorque = Input.GetAxis("Vertical") * -_forwardSpeed;
        _colliderBML.motorTorque = Input.GetAxis("Vertical") * -_forwardSpeed;


        if (Input.GetKey(KeyCode.Space))
        {
            _colliderFR.brakeTorque = 5000f;
            _colliderFMR.brakeTorque = 5000f;
            _colliderBMR.brakeTorque = 5000f;
            _colliderBR.brakeTorque = 5000f;

            _colliderFL.brakeTorque = 5000f;
            _colliderFML.brakeTorque = 5000f;
            _colliderBML.brakeTorque = 5000f;
            _colliderBL.brakeTorque = 5000f;
        }
        else
        {
            _colliderFR.brakeTorque = 0f;
            _colliderFMR.brakeTorque = 0f;
            _colliderBMR.brakeTorque = 0f;
            _colliderBR.brakeTorque = 0f;

            _colliderFL.brakeTorque = 0f;
            _colliderFML.brakeTorque = 0f;
            _colliderBML.brakeTorque = 0f;
            _colliderBL.brakeTorque = 0f;
        }


        if (!_crabMode)
        {
            if (_frontWheelDriveMode)
            {
                _colliderFR.steerAngle = _maxAngle * Input.GetAxis("Horizontal");
                _colliderFL.steerAngle = _maxAngle * Input.GetAxis("Horizontal");
                _colliderFMR.steerAngle = _maxAngle * Input.GetAxis("Horizontal");
                _colliderFML.steerAngle = _maxAngle * Input.GetAxis("Horizontal");
            }

            if(!_frontWheelDriveMode)
            {
                _colliderBR.steerAngle = _maxAngle * Input.GetAxis("Horizontal");
                _colliderBL.steerAngle = _maxAngle * Input.GetAxis("Horizontal");
                _colliderBMR.steerAngle = _maxAngle * Input.GetAxis("Horizontal");
                _colliderBML.steerAngle = _maxAngle * Input.GetAxis("Horizontal");
            }
        }

        if(_breakeMode)
        {
            _colliderFR.steerAngle = _maxAngle * Input.GetAxis("Horizontal");
            _colliderFL.steerAngle = _maxAngle * Input.GetAxis("Horizontal");
            _colliderFMR.steerAngle = _maxAngle * Input.GetAxis("Horizontal");
            _colliderFML.steerAngle = _maxAngle * Input.GetAxis("Horizontal");
            _colliderBR.steerAngle = -_maxAngle * Input.GetAxis("Horizontal");
            _colliderBL.steerAngle = -_maxAngle * Input.GetAxis("Horizontal");
            _colliderBMR.steerAngle = -_maxAngle * Input.GetAxis("Horizontal");
            _colliderBML.steerAngle = -_maxAngle * Input.GetAxis("Horizontal");
        }

        if(_crabMode)
        {
            _colliderFR.steerAngle = _maxAngle * Input.GetAxis("Horizontal");
            _colliderFL.steerAngle = _maxAngle * Input.GetAxis("Horizontal");
            _colliderFMR.steerAngle = _maxAngle * Input.GetAxis("Horizontal");
            _colliderFML.steerAngle = _maxAngle * Input.GetAxis("Horizontal");
            _colliderBR.steerAngle = _maxAngle * Input.GetAxis("Horizontal");
            _colliderBL.steerAngle = _maxAngle * Input.GetAxis("Horizontal");
            _colliderBMR.steerAngle = _maxAngle * Input.GetAxis("Horizontal");
            _colliderBML.steerAngle = _maxAngle * Input.GetAxis("Horizontal");
        }



        RotateWheel(_colliderFR, _transformFR);
        RotateWheel(_colliderFMR, _transformFMR);
        RotateWheel(_colliderBMR, _transformBMR);
        RotateWheel(_colliderBR, _transformBR);
        RotateWheel(_colliderFL, _transformFL);
        RotateWheel(_colliderFML, _transformFML);
        RotateWheel(_colliderBML, _transformBML);
        RotateWheel(_colliderBL, _transformBL);
    }
    private void AntiRollBar(WheelCollider wheelL, WheelCollider wheelR)
    {
        WheelHit hit;
        float travelL = 1.0f;
        float travelR = 1.0f;

        bool groundedL = wheelL.GetGroundHit(out hit);
        if (groundedL)
            travelL = (-wheelL.transform.InverseTransformPoint(hit.point).y - wheelL.radius) / wheelL.suspensionDistance;

        bool groundedR = wheelR.GetGroundHit(out hit);
        if (groundedR)
            travelR = (-wheelR.transform.InverseTransformPoint(hit.point).y - wheelR.radius) / wheelR.suspensionDistance;

        float antiRollForce = (travelL - travelR) * _antiRoll;

        if (groundedL)
            GetComponent<Rigidbody>().AddForceAtPosition(wheelL.transform.up * -antiRollForce, wheelL.transform.position);

        if (groundedR)
            GetComponent<Rigidbody>().AddForceAtPosition(wheelR.transform.up * antiRollForce, wheelR.transform.position);
    }

    private void RotateWheel(WheelCollider collider, Transform transform)
    {
        Vector3 position;
        Quaternion rotation;

        collider.GetWorldPose(out position, out rotation);

        transform.rotation = rotation;
        transform.position = position;
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.C))
        {
            if (_crabMode)
            {
                _crabMode = false;
                _forwardSpeed = _forwardSpeed * 3f;
            }

            else
            {
                _crabMode = true;
                _forwardSpeed = _forwardSpeed / 3f;
            }
        }

        if (Input.GetKeyDown(KeyCode.X))
        {
            if (_frontWheelDriveMode)
                _frontWheelDriveMode = false;

            else
                _frontWheelDriveMode = true;
        }

        if(Input.GetKeyDown(KeyCode.Z))
        {
            if (_breakeMode)
            {
                _breakeMode = false;
            }

            else
            {
                _breakeMode = true;
            }
        }

        //if (_rb.velocity.magnitude >= 2f)
        //{
        //    _audioSourceDrive.volume = _rb.velocity.magnitude / 2;
        //    if (!_audioSourceDrive.isPlaying)
        //    {
        //        _audioSourceDrive.Play();
        //        _audioSourceIdle.Stop();
        //    }
        //}

        //if (_rb.velocity.magnitude < 2f)
        //{
        //    if (!_audioSourceIdle.isPlaying)
        //    {
        //        _audioSourceIdle.Play();
        //        _audioSourceDrive.Stop();
        //    }
        //}
    }
}

