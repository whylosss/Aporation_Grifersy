using UnityEngine;
using Game.SeniorEventBus;

public class MiniEnter : MonoBehaviour
{
    [SerializeField] private PlaneController _planeController;
    [SerializeField] private PlaneHealth _planeHealth;
    [SerializeField] private MissileLouncher _missileLouncher;
    [SerializeField] private StopDetector _stopDetector;
    [SerializeField] private GameUI _gameUI;

    private EnemyListController _enemyListController;
    private EventBus _eventBus;

    private void Awake()
    {
        _enemyListController = new EnemyListController();
        _eventBus = new EventBus();

        Register();
        Init();
    }


    private void Init()
    {
        _planeController.Init();
        _stopDetector.Init();
        _missileLouncher.Init();
        _planeHealth.Init();
        _gameUI.Init();
        _enemyListController.Init();
    }
    private void Register()
    {
        ServiceLocator.Initialize();

        ServiceLocator.Current.Register(_eventBus);
        ServiceLocator.Current.Register(_enemyListController);
        ServiceLocator.Current.Register<PlaneController>(_planeController);
        ServiceLocator.Current.Register<StopDetector>(_stopDetector);
    }
}
