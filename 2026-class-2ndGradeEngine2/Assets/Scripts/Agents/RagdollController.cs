using System.Collections.Generic;
using Enemies;
using GGMLib.ModuleSystem;
using Unity.VisualScripting;
using UnityEngine;

namespace Agents
{
    public class RagdollController : MonoBehaviour, IModule
    {
        [SerializeField] private float defaultForce = 500f;
        
        private Rigidbody[] _ragdollRigidbodies;
        private Collider[] _ragdollColliders;
        private IRenderer _renderer;
        private INavMovement _navMovement;
        
        public bool IsRagdollActive { get; private set; }
        
        public void Initialize(ModuleOwner owner)
        {
            _renderer = owner.GetModule<IRenderer>();
            _navMovement = owner.GetModule<INavMovement>();
            
            _ragdollRigidbodies = owner.GetComponentsInChildren<Rigidbody>();
            
            //래그돌 본에 붙은 콜라이더만 수집해야한다.
            List<Collider> colliderList= new List<Collider>();
            foreach (Rigidbody rb in _ragdollRigidbodies)
            {
                colliderList.AddRange(rb.GetComponents<Collider>());
                //래그돌은 Rigidbody하나에 여러개 콜라이더가 있기도 해.
            }
            
            _ragdollColliders = colliderList.ToArray();

            SetRagdollActive(false);
        }

        private void SetRagdollActive(bool  isActive)
        {
            IsRagdollActive = isActive;
            foreach(Rigidbody rb in _ragdollRigidbodies)
                rb.isKinematic = !isActive;
            foreach (Collider col in _ragdollColliders)
                col.enabled = isActive;
        }

        //이건 최적화를 위해 포기할 수도 있다.
        private Rigidbody GetClosestRigidbody(Vector3 point)
        {
            Rigidbody closest = null;
            float minSqrDist = float.MaxValue;

            foreach (Rigidbody rb in _ragdollRigidbodies)
            {
                float sqrDist = (rb.position - point).sqrMagnitude;
                if (sqrDist < minSqrDist)
                {
                    minSqrDist = sqrDist;
                    closest = rb;
                }
            }

            return closest;
        }

        public void EnableRagdoll(Vector3 hitPoint, Vector3 hitNormal, float force = -1f)
        {
            if (IsRagdollActive) return;
            
            if (_renderer != null && _renderer.Animator.enabled) _renderer.Animator.enabled = false;
            if (_navMovement != null) _navMovement.NavMeshAgent.enabled = false;

            SetRagdollActive(true);

            float appliedForce = force < 0f ? defaultForce : force;
            Rigidbody targetRb = GetClosestRigidbody(hitPoint);

            if (targetRb != null)
            {
                targetRb.AddForce(-hitNormal * appliedForce, ForceMode.Impulse);
            }
        }
    }
}