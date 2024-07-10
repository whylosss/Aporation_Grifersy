using UnityEngine;
using UnityEngine.AI;

namespace Game.Enemy
{
    public abstract class AbstractEnemy : MonoBehaviour
    {
        [SerializeField] protected bool _isMission5 = false;
        [SerializeField] protected float _range = 20f;

        protected Animator _animator;

        protected GameObject[] _points;
        protected GameObject _mainCharacter;


        [HideInInspector] public NavMeshAgent Agent;
        [HideInInspector] public bool IsDetected = false;
        [HideInInspector] public bool IsDead = false;

        public abstract void Chill();
        public abstract void EnemyDetected();
    }
}
