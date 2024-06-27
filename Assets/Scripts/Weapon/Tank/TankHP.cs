using Game.SeniorEventBus;
using Game.SeniorEventBus.Signals;
using UnityEngine;

public class TankHP : MonoBehaviour, ITankHealth
{
    [SerializeField] private ParticleSystem _effect;
    [SerializeField] private Transform _pos;

    private EventBus _eventBus;
    private Rigidbody _rb;

    private void Start()
    {
        _eventBus = ServiceLocator.Current.Get<EventBus>();

        _rb = GetComponent<Rigidbody>();
    }
    public void Destroy()
    {
        Instantiate(_effect, _pos.position, Quaternion.identity);
        Vector3 dir = new Vector3(1, 2, 3);
        _rb.AddForce(dir * 2f, ForceMode.Impulse);
        _eventBus.Invoke(new DestroyTank());
    }
}
