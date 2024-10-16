using Game.Player;
using System.Collections;
using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
    [SerializeField] private float _attackRange = 3f;
    [SerializeField] private float _damage = 40f;

    private bool _started = false;

    private Animator _animator;
    private GameObject _mainCharacter;

    private IEnemyHealth _health;

    private void Start()
    {
        _health = GetComponent<IEnemyHealth>();
        _animator = GetComponent<Animator>();
        _mainCharacter = GameObject.FindGameObjectWithTag("Player");
    }

    private void Update()
    {
        if(!_mainCharacter.Equals(null) && Vector3.Distance(transform.position, _mainCharacter.transform.position) <= _attackRange)
        {
            if(!_started)
            {
                StartCoroutine(Attack());
                _started = true;
            }
        }

        else if (!_mainCharacter.Equals(null) && Vector3.Distance(transform.position, _mainCharacter.transform.position) > _attackRange)
        {
            if (_started)
            {
                StopAllCoroutines();
                _started = false;
            }
        }
    }

    private IEnumerator Attack()
    {
        _animator.SetTrigger("Attack");

        Invoke("SendDamage", 0.5f);

        yield return new WaitForSeconds(1.5f);
    }

    private void SendDamage()
    {
        if (!_health.IsDead)
        {
            Collider[] hits = Physics.OverlapSphere(transform.position, _attackRange);

            foreach (Collider hit in hits)
            {
                if (hit.transform.TryGetComponent(out IPlayerHealth health))
                    health.GetDamage(_damage / 2);
            }
        }
    }
}
