using Agents;
using CombatSystem;
using DG.Tweening;
using GGMLib.AnimationSystem;
using Players.SKillSystem;
using System.Collections;
using System.Reflection;
using UnityEngine;
using UnityEngine.UIElements;
using static UnityEditor.Experimental.GraphView.GraphView;

namespace Players.SkillSystem
{
    public class PlayerSwordCombo : AbstractPlayerSkill
    {
        [SerializeField] private AnimParamSO[] comboClips;
        [SerializeField] private AnimationCurve[] comboCurves; //�����̴� ���� ����
        [SerializeField] private float[] comboDurations; //�޺� ���ӽð�
        [SerializeField] private float comboWindow = 0.4f; //�޺��� �̾����� �ð�

        private AgentTrigger _agentTrigger;

        public float AttackSpeed { get; private set; }
        public int ComboCounter { get; private set; } = 0;

        public override void InitializeSkill(ISkillModule skillModule)
        {
            base.InitializeSkill(skillModule);
            _agentTrigger = _player.GetModule<AgentTrigger>();
            Debug.Assert(_agentTrigger != null, "Sword combo ������ �ִϸ��̼� Ʈ���Ű� �ʿ��մϴ�.");
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
            _movement.CanManualMove = false;//�ڵ� ���� ���� ����
            while (IsUsing)
            {
                float percent = currentDuration / comboDuration; //0~1 ������ ���� ����ȭ���ش�.
                currentDuration += Time.deltaTime;
                float force = comboCurve.Evaluate(percent);
                _movement.SetMovementVelocity(forward * force);
                yield return null;
            }
            _movement.CanManualMove = true; //���� ���۸��� ����.
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