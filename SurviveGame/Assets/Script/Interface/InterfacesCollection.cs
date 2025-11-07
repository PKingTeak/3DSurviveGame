using Attacker;
using NUnit.Framework.Interfaces;
using System.Collections;
using System.IO;
using System.Net.Sockets;
using Unity.VisualScripting;
using UnityEngine;


namespace Attacker
{


    public interface IAttacker // 공격하는 유닛
    {
        bool canAttack { get; }
        void Attack(Unit target, int damage);

        
        // 제너릭으로 오브젝트도 데미지가 생긴다면 리펙토링 하면된다.

    }

    public class MeleeAttacker : MonoBehaviour , IAttacker
    {
        [SerializeField] float attackRange = 1.5f;
        [SerializeField] float coolDown = 0.8f;
        [SerializeField] float maxAngle = 180f;
        float lastTime;


        
        public bool canAttack => Time.time >= lastTime + coolDown;

        void Attack(Unit target, int damage)
        {
            if (!canAttack || target == null || !IsEffectiveAgainst(target.transform))
            {
                return; 
            }

            //애니메이션 추가 후 여기서 트리거 판정 예정


            //범위 판정도 해야하고 

            
        }

        public bool CanAttack(Transform target)
        {
            return IsEffectiveAgainst(target);

        }

        public bool CanAttack(Unit target) //오버로드
        {
            return target != null && IsEffectiveAgainst(target.transform);
        }


        public bool TryAttack(Unit target, int damage)
        {
            if (target == null) return false;
            if (!canAttack) return false;

            Attack(target, damage);
            return true;
        }

        void IAttacker.Attack(Unit target, int damage)
        {
            Attack(target, damage);
        }

        private bool IsEffectiveAgainst(Transform target)
        {
            Vector3 selfPos = transform.position;
            Vector3 to = target.position - selfPos;
            to.y = 0;

            if (to.sqrMagnitude > attackRange * attackRange)
            {
                return false;
            }

            //각도 
            if (maxAngle < 179.9f)
            {
                float ang = Vector3.Angle(transform.forward, to);
                if (ang > maxAngle * 0.5f)
                {
                    return false;
                }
            }
            return true;    
        }

#if UNITY_EDITOR
        private void OnDrawGizmosSelected()
        {
            Gizmos.color = new Color(1f, .5f, 0f, .8f);
            Gizmos.DrawWireSphere(transform.position, attackRange);
        }
#endif
    }
}

namespace Mover
{
    public interface IMover
    {
        float Speed { get; set; }
        void Move(Vector3 direction);
        void Stop();
    }

    public class Mover : MonoBehaviour, IMover
    {
        [SerializeField] float speed { get; set; }
        float IMover.Speed { get => speed; set => speed = value; }

        public void Move(Vector3 direction)
        {
            this.transform.position += direction * speed  *  Time.deltaTime;

        }


        public void Stop()
        {
            throw new System.NotImplementedException();
        }
    }

    public class RbMover : MonoBehaviour, IMover
    {
        [SerializeField] private float speed;
        public float Speed { get => speed; set => speed  = value; }

        private Rigidbody rigid;

        private void Awake()
        {
            rigid = GetComponent<Rigidbody>();
            rigid.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;
        }

        public void Move(Vector3 direction)
        {
            var dir = direction.sqrMagnitude > 1e-6f ? direction.normalized : Vector3.Normalize(direction);

            var v = rigid.linearVelocity;
            v.x = dir.x* speed;
            v.z = dir.z * speed;
            rigid.linearVelocity = v;
        }

        public void Stop()
        {
            var v = rigid.linearVelocity;
            v.x = 0f; v.z = 0f;
            rigid.linearVelocity = v; //속도 0으로 초기화
        }
    }
}


namespace Brain
{
    public interface IBrain
    {
        void Think();

    }

    
}


public interface IEnemy
{
    public enum enemyType
    {
        Chaser = 0,
        Ranger = 1,
        Defenser = 2,
    }
}





