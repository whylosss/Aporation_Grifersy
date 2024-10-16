using UnityEngine;

public class MineTank : MonoBehaviour
{
    [SerializeField] private float _range = 15f;
    [SerializeField] private ParticleSystem _effect;
    [SerializeField] private AudioClip _explode;

    private GameObject _mainCharacter;
    private AudioSource _audioSource;
    private void Start()
    {
        _mainCharacter = GameObject.FindGameObjectWithTag("Player");
        _audioSource = _mainCharacter.GetComponent<AudioSource>();
        SetMine.Explode += Explode;
    }

    private void Explode()
    {
        _audioSource.PlayOneShot(_explode);
        Collider[] hits = Physics.OverlapSphere(transform.position, _range);
        foreach (Collider hit in hits)
        {
            if(hit.transform.TryGetComponent(out ITargetHealth health))
                health.Destroy();
        }
        Instantiate(_effect, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }

    private void OnDestroy()
    {
        SetMine.Explode -= Explode;
    }

}
