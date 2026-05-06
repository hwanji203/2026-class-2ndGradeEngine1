using System.Collections;
using System.Net.NetworkInformation;
using Agents;
using CombatSystem;
using CoreSystem;
using GGMLib.AnimationSystem;
using UnityEngine;

namespace Players.SkillSystem
{
    public class PlayerSwordCombo : AbstractPlayerSkill
    {
        [SerializeField] private AnimParamSO[] comboClips;
        [SerializeField] private AnimationCurve[] comboCurves; //움직이는 양을 조절
        [SerializeField] private float[] comboDurations; //콤보 지속시간
        [SerializeField] private AssetNameSO[] comboEffects;
        
        [SerializeField] private float comboWindow = 0.4f; //콤보가 이어지는 시간
        
        private AgentTrigger _agentTrigger;
        private VfxModule _vfxModule;
        
        public float AttackSpeed { get; private set; }
        public int ComboCounter { get; private set; } = 0;

        public override void InitializeSkill(ISkillModule skillModule)
        {
            base.InitializeSkill(skillModule);
            _agentTrigger = _player.GetModule<AgentTrigger>();
            Debug.Assert(_agentTrigger != null, "Sword combo 공격은 애니메이션 트리거가 필요합니다.");
            _vfxModule = _player.GetModule<VfxModule>();
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
            _agentTrigger.OnAnimationEnd -= HandleAnimationEnd;
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