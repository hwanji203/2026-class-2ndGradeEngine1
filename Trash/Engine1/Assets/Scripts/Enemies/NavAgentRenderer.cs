using System;
using Agents;
using GGMLib.AnimationSystem;
using GGMLib.ModuleSystem;
using UnityEngine;
using UnityEngine.AI;

namespace Enemies
{
    public class NavAgentRenderer : AgentRenderer, IAfterInitModule
    {
        [SerializeField] private AnimParamSO speedParam;

        [Header("Navigation Agent control")]
        [SerializeField] private bool updateRotation;
        [SerializeField] private bool updatePosition;

        private INavMovement _navMovement;
        private NavMeshAgent _navAgent;
        private Vector2 _velocity;
        private Vector2 _smoothDeltaPosition;

        public override void Initialize(ModuleOwner owner)
        {
            base.Initialize(owner);
            _navMovement = owner.GetModule<INavMovement>();
            Debug.Assert(_navMovement != null, "NavAgentRenderer는 INavMovement가 필요합니다.");
        }

        public void AfterInit()
        {
            _navAgent = _navMovement.NavMeshAgent; //이건 AfterInit에서 해야해.
            _navAgent.updatePosition = updatePosition;
            _navAgent.updateRotation = updateRotation;
        }

        private void OnAnimatorMove()
        {
            if (_navAgent == null) return;

            Vector3 rootPosition = Animator.rootPosition;
            rootPosition.y = _navAgent.nextPosition.y;

            _owner.transform.position = rootPosition;
            _navAgent.nextPosition = rootPosition;
        }

        private void SynchronizeAnimatorAndNavAgent()
        {
            if (_navAgent == null) return;

            //월드 좌표의 델타좌표.
            Vector3 worldDeltaPosition = _navAgent.nextPosition - _owner.transform.position;
            worldDeltaPosition.y = 0; //이건 계산하지 않을거다.

            float dx = Vector3.Dot(_owner.transform.right, worldDeltaPosition);
            float dy = Vector3.Dot(_owner.transform.forward, worldDeltaPosition);

            Vector2 localDelta = new Vector2(dx, dy);
            float smooth = Mathf.Min(1, Time.deltaTime / 0.1f);

            _smoothDeltaPosition = Vector2.Lerp(_smoothDeltaPosition, localDelta, smooth);
            _velocity = _smoothDeltaPosition / Time.deltaTime;

            if (_navAgent.remainingDistance <= _navAgent.stoppingDistance)
            {
                _velocity = Vector2.Lerp(Vector2.zero, _velocity, _navAgent.remainingDistance / _navAgent.stoppingDistance);
            }

            Animator.SetFloat(speedParam.ParamHash, _velocity.magnitude);
            //모델의 위치가 너무 벗어난경우 처리
            float deltaMagnitude = worldDeltaPosition.magnitude;
            if (deltaMagnitude > _navAgent.radius * 0.5f)
            {
                _owner.transform.position = Vector3.Lerp(Animator.rootPosition, _navAgent.nextPosition, smooth);
            }
        }

    }
}