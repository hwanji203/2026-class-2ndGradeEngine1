using System.Collections;
using Unity.AI.Navigation;
using UnityEngine;
using UnityEngine.AI;

namespace _01.Scripts
{
    public class NavMeshClimbTest : MonoBehaviour
    {
        [SerializeField] private int targetOffMeshLayer = 4;
        [SerializeField] private float jumpSpeed = 10.0f;
        [SerializeField] private float yOffset = 0.5f;

        private NavMeshAgent _navAgent;

        private void Start()
        {
            _navAgent = GetComponent<NavMeshAgent>();
            StartCoroutine(ClimbRoutine());
        }

        private IEnumerator ClimbRoutine()
        {
            while (true)
            {
                yield return new WaitUntil(CanClimbState);
                yield return StartCoroutine(JumpProcess());
            }
        }

        private bool CanClimbState()
        {
            if (!_navAgent.isOnOffMeshLink) return false;

            NavMeshLink linkOwner = _navAgent.currentOffMeshLinkData.owner as NavMeshLink;
            return linkOwner != null && linkOwner.area == targetOffMeshLayer;
        }

        private IEnumerator JumpProcess()
        {
            _navAgent.isStopped = true;

            OffMeshLinkData linkData = _navAgent.currentOffMeshLinkData;
            Vector3 start = transform.position;
            Vector3 end = linkData.endPos;

            float jumpTime = Mathf.Max(0.3f, Vector3.Distance(start, end) / jumpSpeed);

            // 올라갈 때: 먼저 Y 올리고 → XZ 이동
            // 내려갈 때: 먼저 XZ 이동하고 → Y 내리기
            bool isClimbingUp = end.y > start.y;
            if (isClimbingUp)
            {
                yield return StartCoroutine(LerpY(start.y, end.y, jumpTime));
                yield return StartCoroutine(MoveAlongAxis(start, end, jumpTime));
            }
            else
            {
                yield return StartCoroutine(MoveAlongAxis(start, end, jumpTime));
                yield return StartCoroutine(LerpY(start.y, end.y, jumpTime));
            }

            _navAgent.CompleteOffMeshLink();
            _navAgent.isStopped = false;
        }

        // XZ축만 이동 (Y는 현재 위치 유지)
        private IEnumerator MoveAlongAxis(Vector3 start, Vector3 end, float duration)
        {
            float elapsed = 0f;
            while (elapsed < duration)
            {
                elapsed += Time.deltaTime;
                float percent = elapsed / duration;

                Vector3 next = Vector3.Lerp(start, end, percent);
                transform.position = new Vector3(next.x, transform.position.y, next.z);
                yield return null;
            }
        }

        // Y축만 이동 (XZ는 현재 위치 유지)
        private IEnumerator LerpY(float startY, float endY, float duration)
        {
            float elapsed = 0f;
            while (elapsed < duration)
            {
                elapsed += Time.deltaTime;
                float percent = elapsed / duration;

                float y = Mathf.Lerp(startY, endY, percent);
                transform.position = new Vector3(transform.position.x, y + yOffset, transform.position.z);
                yield return null;
            }
        }
    }
}