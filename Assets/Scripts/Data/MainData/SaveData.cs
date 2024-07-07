using UnityEngine;
using Game.Player;
using Game.Weapon;

namespace Game.Data
{
    public class SaveData : MonoBehaviour, IService
    {
        private PlayerHealth _playerHealth;
        private Movement _playerMove;
        private Helicopter _helicopter;
        private HelicopterStatesController _helicopterStatesController;
        private Car _car;
        private WeaponAk _weaponAk;
        private RPG _rpg;
        private ScopeLevels _scopeLevels;
        private CoinSystem _coinSystem;
        private BaseStates _baseStates;
        private GrenadeThrower _grenadeThrower;
        private VolumeController _volume;
        public void Init()
        {
            _playerHealth = ServiceLocator.Current.Get<PlayerHealth>();
            _playerMove = ServiceLocator.Current.Get<Movement>();
            _helicopter = ServiceLocator.Current.Get<Helicopter>();
            _helicopterStatesController = ServiceLocator.Current.Get<HelicopterStatesController>();
            _car = ServiceLocator.Current.Get<Car>();
            _weaponAk = ServiceLocator.Current.Get<WeaponAk>();
            _rpg = ServiceLocator.Current.Get<RPG>();
            _scopeLevels = ServiceLocator.Current.Get<ScopeLevels>();
            _coinSystem = ServiceLocator.Current.Get<CoinSystem>();
            _baseStates = ServiceLocator.Current.Get<BaseStates>();
            _grenadeThrower = ServiceLocator.Current.Get<GrenadeThrower>();
            _volume = ServiceLocator.Current.Get<VolumeController>();
            ConstSystem.CanSave = true;
        }

        private void Update()
        {
            if(Input.GetKeyDown(KeyCode.Escape))
                SaveInfo();
        }

        public void SaveInfo()
        {
            if(ConstSystem.CanSave)
                SaveSystem.SavePlayerData(_playerHealth, _playerMove, _volume, _grenadeThrower, _scopeLevels, _helicopter, _helicopterStatesController, _car, _weaponAk, _rpg, _coinSystem, _baseStates); 
        }

        public void CanSave(bool state)
        {
            ConstSystem.CanSave = state;
        }
    }
}
