using System;
using Agents;
using Agents.StatSystem;
using GGMLib.AnimationSystem;
using GGMLib.ModuleSystem;
using UnityEngine;
using UnityEngine.AI;

namespace Enemies
{
    public class NavAgentRenderer : AgentRenderer, IAfterInitModule
    {
        [Header("각종 데이터 에셋")] 
        [SerializeField] private AnimParamSO speedParam;

        [SerializeField] private StatSO moveSpeedStat;
        [SerializeField] private AnimParamSO speedMultiplyParam;
        
        [Header("Navigation Agent control")]
        [SerializeField] private bool updateRotation;
        [SerializeField] private bool updatePosition;
        
        [Header("Force rotation settings")]
        [SerializeField] private bool forceRotation;
        [SerializeField] private float forceRotationSpeed;

        public bool IsUpdateRotationByAnimator
        {
            get => !updateRotation; //이게 켜져있으면 navAgent가 처리함
            set
            {
                updateRotation = !value;
                if (_navAgent != null)
                {
                    _navAgent.updateRotation = updateRotation; //갱신한다
                }
            }
        }
        
        private INavMovement _navMovement;
        private NavMeshAgent _navAgent;
        private IStatModule _statModule;
        private Vector2 _velocity;
        private Vector2 _smoothDeltaPosition;
        private float _moveMultiplier = 1f;

        public override void Initialize(ModuleOwner owner)
        {
            base.Initialize(owner);
            _navMovement = owner.GetModule<INavMovement>();
            Debug.Assert(_navMovement != null, $"{owner.gameObject.name}: NavMovement module is required for NavAgentRenderer");
            _statModule = owner.GetModule<IStatModule>();
            Debug.Assert(_statModule != null, $"{owner.gameObject.name}: StatModule is required for NavAgentRenderer");
        }

        public void AfterInit()
        {
            _navAgent = _navMovement.NavMeshAgent;
            _navAgent.updatePosition = updatePosition;
            _navAgent.updateRotation = updateRotation;
            
            _moveMultiplier = _statModule.SubscribeStat(moveSpeedStat.AssetIndex, HandleMoveSpeedChange, 0.5f) * 0.1f;
            Animator.SetFloat(speedMultiplyParam.ParamHash, _moveMultiplier);
        }
        
        private void OnDestroy()
        {
            _statModule?.UnSubscribeStat(moveSpeedStat.AssetIndex, HandleMoveSpeedChange);
        }

        private void HandleMoveSpeedChange(StatSO stat, float currentValue, float prevValue)
        {
            _moveMultiplier = currentValue * 0.1f;
            Animator.SetFloat(speedMultiplyParam.ParamHash, _moveMultiplier);
        }

        private void OnAnimatorMove()
        {
            if (_navMovement == null) return;
            
            Vector3 rootPosition = Animator.rootPosition;
            rootPosition.y = _navAgent.nextPosition.y;
            
            if (NavMesh.SamplePosition(rootPosition, out NavMeshHit hit, 0.4f, NavMesh.AllAreas))
            {
                _owner.transform.position = rootPosition;
                _navAgent.nextPosition = hit.position;
            }
            
            if (IsUpdateRotationByAnimator)
                _owner.transform.rotation = Animator.rootRotation;
        }

        private void Update()
        {
            SynchronizeAnimationWithNavMeshAgent();
            ForceRotationControl();
        }

        private void SynchronizeAnimationWithNavMeshAgent()
        {
            if (_navAgent == null || !_navAgent.isOnNavMesh) return;
            
            Vector3 worldDeltaPosition = _navAgent.nextPosition - _owner.transform.position;
            worldDeltaPosition.y = 0f;
            
            float dx = Vector3.Dot(_owner.transform.right, worldDeltaPosition);
            float dy = Vector3.Dot(_owner.transform.forward, worldDeltaPosition);
            
            Vector2 localDelta = new Vector2(dx, dy);
            float smooth = Mathf.Min(1, Time.deltaTime / 0.1f);
            
            _smoothDeltaPosition = Vector2.Lerp(_smoothDeltaPosition, localDelta, smooth);
            float dt = Mathf.Max(Time.deltaTime, 0.0001f);
            _velocity = _smoothDeltaPosition / dt;

            if (_navAgent.remainingDistance <= _navAgent.stoppingDistance)
            {
                if (_navAgent.stoppingDistance > 0.0001f)
                {
                    _velocity = Vector2.Lerp(
                        Vector2.zero,
                        _velocity,
                        Mathf.Clamp01(_navAgent.remainingDistance / _navAgent.stoppingDistance));
                }
                else
                {
                    _velocity = Vector2.zero;
                }
            }
            
            Animator.SetFloat(speedParam.ParamHash, _velocity.magnitude);
            float deltaMagnitude = worldDeltaPosition.magnitude;
            if (deltaMagnitude > _navAgent.radius * 0.5f)
            {
                _owner.transform.position = Vector3.Lerp(Animator.rootPosition, _navAgent.nextPosition, smooth);
            }
        }
        
        private void ForceRotationControl()
        {
            if (!forceRotation || _navAgent == null || !_navAgent.isOnNavMesh || _navMovement.IsArrived) return;

            // steeringTarget는 상황에 따라 현재 위치와 매우 가까울 수 있어 desiredVelocity로 fallback.
            Vector3 desiredDirection = _navAgent.steeringTarget - _owner.transform.position;
            desiredDirection.y = 0f;
            if (desiredDirection.sqrMagnitude < 0.0001f)
            {
                desiredDirection = _navAgent.desiredVelocity;
                desiredDirection.y = 0f;
            }
            if (desiredDirection.sqrMagnitude < 0.0001f) return;

            Quaternion desiredRotation = Quaternion.LookRotation(desiredDirection, Vector3.up);
            _owner.transform.rotation = Quaternion.RotateTowards
                (_owner.transform.rotation, desiredRotation, forceRotationSpeed * Time.deltaTime);
        }
    }
}