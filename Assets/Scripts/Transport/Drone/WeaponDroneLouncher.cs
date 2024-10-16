using Game.SeniorEventBus.Signals;
using Game.SeniorEventBus;
using UnityEngine;
using UnityEngine.Video;

public class WeaponDroneLouncher : AbstractDroneLouncher, IService
{
    [SerializeField] private GameObject _lostConectionPanel;
    [SerializeField] private VideoPlayer _video;

    private int DroneLounched = 0;
    private void Start()
    {
        _eventBus = ServiceLocator.Current.Get<EventBus>();
        _mainCharacter = gameObject;
        _eventBus.Invoke(new UpdateDrone(DronesAmount));
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.T) && DronesAmount > 0)
        {
            Vector3 pos = transform.position;

            if (DroneLounched < 2)
            {
                KamikadzeDrone newDrone = Instantiate(_kamikadze, _lounchPos.position, _lounchPos.rotation);

                newDrone.MainCharacter = _mainCharacter;
                newDrone.GamePanel = _panel;
                newDrone.DroneSpawn = pos;
                newDrone.TextDistance = _distanceText;
                newDrone.BatteryText = _batteryText;
                newDrone.MaxDistance = _maxDistance;
                newDrone.LostConnectionPanel = _lostConectionPanel;
                newDrone.Video = _video;
                newDrone.CharacterPos = new Vector3(transform.position.x, transform.position.y - 0.77f, transform.position.z);

                _nameText.text = _droneName1.ToString();
            }

            else
            {
                FPVDrone newDrone = Instantiate(_fpv, _lounchPos.position, _lounchPos.rotation);

                newDrone.MainCharacter = _mainCharacter;
                newDrone.GamePanel = _panel;
                newDrone.DroneSpawn = pos;
                newDrone.TextDistance = _distanceText;
                newDrone.BatteryText = _batteryText;
                newDrone.MaxDistance = _maxDistance;
                newDrone.CharacterPos = new Vector3(transform.position.x, transform.position.y - 0.77f, transform.position.z);

                _nameText.text = _droneName2.ToString();
            }

            DronesAmount--;
            DroneLounched++;
            _eventBus.Invoke(new UpdateDrone(DronesAmount));
        }
       
    }
}
