using UnityEngine;
using Game.SeniorEventBus;
using Game.SeniorEventBus.Signals;

public class Scoping : MonoBehaviour
{
    private Camera _mainCamera;

    [HideInInspector] public Camera AimCamera;

    private EventBus _eventBus;

    private void Start()
    {
        _eventBus = ServiceLocator.Current.Get<EventBus>();

        _mainCamera = Camera.main;

        _mainCamera.enabled = true;
    }
    void Update()
    {
        if (Input.GetMouseButton(1))
        {
            _mainCamera.enabled = false;
            AimCamera = GetComponentInChildren<Camera>();
            AimCamera.enabled = true;
            _eventBus.Invoke(new SetAimCamera(AimCamera));
            _eventBus.Invoke(new ChangeSens(70));
        }
        else
        {
            _mainCamera.enabled = true;
            AimCamera = GetComponentInChildren<Camera>();
            AimCamera.enabled = false;
            _eventBus.Invoke(new ChangeSens(300));
        }
    }
}
