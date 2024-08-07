using UnityEngine;
using System.Collections;
using Game.SeniorEventBus;
using Game.SeniorEventBus.Signals;

namespace Game.Weapon
{
    [RequireComponent(typeof(AudioSource))]
    public class WeaponAk : AbstractWeapon, IService
    {
        private AudioSource _audioSource;

        [Header("Effect")]
        [SerializeField] private Transform _spawnPosition;
        [SerializeField] private ParticleSystem _shootingEffect;
        [SerializeField] private ParticleSystem _bulletEffect;
        [SerializeField] private ParticleSystem _enemyEffect;


        [Header("Clips")]
        [SerializeField] private AudioClip _shootSound;
        [SerializeField] private AudioClip _noBulletsSound;

        [SerializeField] private int _maxBullets = 30;
        [SerializeField] private float _interval = 0.12f;

        private bool _canShoot = true;

        private EventBus _eventBus;

        public override void Init()
        {
            _mainCamera = Camera.main;

            _eventBus = ServiceLocator.Current.Get<EventBus>();
            _eventBus.Subscribe<BuyAkBullets>(AddBullets, 1);
            _eventBus.Subscribe<SetAimCamera>(GetCamera, 1);

            _aimCamera = GetComponentInChildren<Camera>();

            _audioSource = GetComponent<AudioSource>();

            _mainCamera.enabled = true;
            _aimCamera.enabled = false;
        }

        private void OnEnable()
        {
            _mainCamera.enabled = true;
            _shootingEffect.Stop();
            _eventBus.Invoke(new SetCurrentBullets(true));
            _eventBus.Invoke(new SetTotalBullets(true));
            _eventBus.Invoke(new UpdateCurrentBullets(Bullets));
            _eventBus.Invoke(new UpdateTotalBullets(TotalBullets));
            _eventBus.Invoke(new SetImage(0, true));
        }

        private void Update()
        {
            if (Input.GetButtonDown("Fire1") )
            {
                if (_mainCamera.enabled)
                    StartCoroutine(Shoot(_mainCamera));

                if (_aimCamera.enabled)
                    StartCoroutine(Shoot(_aimCamera));
            }

            if (Input.GetMouseButtonUp(0))
            {
                StopAllCoroutines();
            }

            if (Input.GetKeyDown(KeyCode.R) && Bullets < _maxBullets && TotalBullets > 0)
            {
                _eventBus.Invoke(new AkReloadAnim());
                _canShoot = false;
                Invoke("Reaload", 1.5f);
            }
        }

        protected override IEnumerator Shoot(Camera _cam)
        {
            if (Bullets > 0 && _canShoot)
            {
                _eventBus.Invoke(new AkShootAnim());

                RaycastHit hit;
                if (Physics.Raycast(_cam.transform.position, _cam.transform.forward, out hit, _range))
                {

                    if (hit.collider.TryGetComponent(out IEnemyHealth enemy))
                    {
                        enemy.GetDamage(_damage);
                        Instantiate(_enemyEffect, hit.point, Quaternion.identity);
                    }

                    else
                        Instantiate(_bulletEffect, hit.point, Quaternion.identity);
                }

                _audioSource.PlayOneShot(_shootSound);
                _shootingEffect.Play();
                Bullets--;

                _eventBus.Invoke(new UpdateCurrentBullets(Bullets));
                _eventBus.Invoke(new CheckList(transform.position, _callRange));

                //_eventBus.Invoke(new ShakeCamera(0.1f, 0.12f));

                yield return new WaitForSeconds(_interval);

                StartCoroutine(Shoot(_cam));
            }

            else
            {
                _audioSource.PlayOneShot(_noBulletsSound);
                yield return null;
            }
        }

        private void AddBullets(BuyAkBullets bullets)
        {
            TotalBullets += bullets.Amount;
            _eventBus.Invoke(new UpdateTotalBullets(TotalBullets));
        }

        private void Reaload()
        {
            if (TotalBullets >= _maxBullets)
            {
                TotalBullets -= _maxBullets;
                Bullets = _maxBullets;
                _canShoot = true;
            }

            else if (TotalBullets < _maxBullets)
            {
                Bullets = TotalBullets;
                TotalBullets = 0;
                _canShoot = true;
            }

            _eventBus.Invoke(new UpdateTotalBullets(TotalBullets));
            _eventBus.Invoke(new UpdateCurrentBullets(Bullets));
        }

        private void GetCamera(SetAimCamera AimCamera)
        {
            _aimCamera = AimCamera.AimCamera;
        }

        private void OnDisable()
        {
            _eventBus.Invoke(new SetCurrentBullets(false));
            _eventBus.Invoke(new SetTotalBullets(false));
            _eventBus.Invoke(new SetImage(3, false));
        }

        private void OnDestroy()
        {
            _eventBus.Unsubscribe<BuyAkBullets>(AddBullets);
            _eventBus.Unsubscribe<SetAimCamera>(GetCamera);
        }
    }
}
