using Game.SeniorEventBus;
using Game.SeniorEventBus.Signals;
using UnityEngine;

public class TargetHealth : MonoBehaviour, ITargetHealth
{
    [SerializeField] private float _health = 100;
    [SerializeField] private ParticleSystem[] _lowHpEffect;
    [SerializeField] private Material _destoyedMaterial;

    private AirDefence _airDefence;

    private MeshRenderer[] _meshRenderer;
    private EventBus _eventBus;

    private bool _gotDamage = false;
    private bool _isDead = false;

    private void Start()
    {
        _eventBus = ServiceLocator.Current.Get<EventBus>();
        _eventBus.Invoke(new AddObj(gameObject));

        _meshRenderer = GetComponentsInChildren<MeshRenderer>();
        _airDefence = GetComponent<AirDefence>();

        foreach (ParticleSystem effect in _lowHpEffect)
            effect.Stop();
    }

    public void GetDamage(float damage)
    {
        _health -= damage;

        if (!_gotDamage)
        {
            foreach (ParticleSystem effect in _lowHpEffect)
                effect.Play();
            _gotDamage = true;
        }

        if (_health <= 0 && !_isDead)
            Die();
    }

    public void Die()
    {
        foreach (MeshRenderer mesh in _meshRenderer)
            mesh.material = _destoyedMaterial;

        if(_airDefence != null)
            _airDefence.StopAllCoroutines();

        _isDead = true;

        gameObject.tag = "Untagged";
        _eventBus.Invoke(new RemoveObj(gameObject));
    }
}
