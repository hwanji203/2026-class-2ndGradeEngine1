using System.Collections;
using Agents;
using CombatSystem;
using CoreSystem;
using CoreSystem.Events;
using GGMLib.AnimationSystem;
using GGMLib.ObjectPool.Runtime;
using UnityEngine;

namespace Players.SkillSystem
{
    public class PlayerSwordCombo : AbstractPlayerSkill
    {
        [SerializeField] private PoolItemSO slashVfxItem;
        [SerializeField] private PoolItemSO impactVfxItem;
        
        [SerializeField] private AnimParamSO[] comboClips;
        [SerializeField] private AnimationCurve[] comboCurves; //움직이는 양을 조절
        [SerializeField] private float[] comboDurations; //콤보 지속시간
        [SerializeField] private AssetNameSO[] comboEffects;
        
        [SerializeField] private float comboWindow = 0.4f; //콤보가 이어지는 시간
        
        private AgentTrigger _agentTrigger;
        private VfxModule _vfxModule;
        private AbstractDamageCaster _damageCaster;
        
        public float AttackSpeed { get; private set; }
        public int ComboCounter { get; private set; } = 0;

        public override void InitializeSkill(ISkillModule skillModule)
        {
            base.InitializeSkill(skillModule);
            _agentTrigger = _player.GetModule<AgentTrigger>();
            Debug.Assert(_agentTrigger != null, "Sword combo 공격은 애니메이션 트리거가 필요합니다.");
            _vfxModule = _player.GetModule<VfxModule>();
            
            _damageCaster = GetComponentInChildren<AbstractDamageCaster>();
            Debug.Assert(_damageCaster != null, $"데미지 캐스터가 있어야 정상적으로 데미지를 줄 수 있습니다. : {gameObject}");
            _damageCaster.InitCaster(skillModule.Owner); //해당 오너로 캐스터를 초기화(오너를 넣어줘야 차후에 딜러가 누군지 알 수 있습니다.
        }

        public override bool CanUseSkill(GameObject target = null)
        {
            return NormalizedCooldown >= 1f && !IsUsing;
        }

        public override void UseSkill(GameObject target = null)
        {
            base.UseSkill(target);
            bool comboCounterOver = ComboCounter >= comboClips.Length;
            bool comboWindowExhaust = Time.time >= _lastUsingTime + comboWindow;
            if (comboCounterOver || comboWindowExhaust)
            {
                ComboCounter = 0;
            }
            _vfxModule?.PlayVfx(comboEffects[ComboCounter].AssetHash);
            _renderer.PlayClip(comboClips[ComboCounter].ParamHash, 0f, 0.05f);

            Vector3 mousePosition = _player.PlayerInput.GetWorldMousePosition();
            mousePosition.y = _player.transform.position.y;
            Vector3 direction = (mousePosition - _player.transform.position).normalized;
            _movement.RotateTo(direction);
            
            StartCoroutine(SwordComboCoroutine());
        }

        private IEnumerator SwordComboCoroutine()
        {
            _agentTrigger.OnDamageCast += HandleDamageHandle;
            _agentTrigger.OnAnimationEnd += HandleAnimationEnd;
            
            AnimationCurve comboCurve = comboCurves[ComboCounter];
            float comboDuration = comboDurations[ComboCounter];
            float currentDuration = 0;
            Vector3 forward = _player.transform.forward;
            _movement.CanManualMove = false;//자동 조작 모드로 변경
            while (IsUsing)
            {
                float percent = currentDuration / comboDuration; //0~1 값으로 값을 정규화해준다.
                currentDuration += Time.deltaTime;
                float force = comboCurve.Evaluate(percent);
                _movement.SetMovementVelocity(forward * force);
                yield return null;
            }
            _movement.CanManualMove = true; //수동 조작모드로 변경.
            _agentTrigger.OnDamageCast -= HandleDamageHandle;
            _agentTrigger.OnAnimationEnd -= HandleAnimationEnd;
        }

        private void HandleDamageHandle()
        {
            Vector3 position = _damageCaster.transform.position;
            bool isHit = _damageCaster.CastDamage(position, transform.forward, SkillData);
            if (isHit)
            {
                var slash = CreateEvents.ShowPoolingVfx.InitData(
                    slashVfxItem, _damageCaster.LastHitPosition, Quaternion.identity);
                _skillModule.CreateChannel.RaiseEvent(slash);
                var impact = CreateEvents.ShowPoolingVfx.InitData(
                    impactVfxItem, _damageCaster.LastHitPosition, Quaternion.identity);
                _skillModule.CreateChannel.RaiseEvent(impact);
            }
        }

        private void HandleAnimationEnd() => StopSkill();

        public override void StopSkill()
        {
            ComboCounter++;
            // _agentTrigger.OnAnimationEnd -= HandleAnimationEnd;
            base.StopSkill();
        }
    }
}