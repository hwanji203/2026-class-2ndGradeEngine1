using System.Collections;
using System.Linq;
using Unity.AI.Navigation;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Splines;

namespace _01.Scripts
{
    public class NavSplineJumpTest : MonoBehaviour
    {
        [SerializeField] private int targetOffMeshLayer = 5;
        [SerializeField] private float jumpSpeed = 10.0f;

        private NavMeshAgent _navAgent;

        private void Start()
        {
            _navAgent = GetComponent<NavMeshAgent>(); //이거 꼭 써!!!!
            StartCoroutine(StartCoroutine());
        }

        private IEnumerator StartCoroutine()
        {
            while (true)
            {
                yield return new WaitUntil(CanJumpState);
                yield return StartCoroutine(JumpProcess());
            }
        }

        private bool CanJumpState()
        {
            if (!_navAgent.isOnOffMeshLink) return false;

            OffMeshLinkData linkData = _navAgent.currentOffMeshLinkData;

            NavMeshLink linkOwner = linkData.owner as NavMeshLink;

            return linkOwner != null && linkOwner.area == targetOffMeshLayer;
        }

        private IEnumerator JumpProcess()
        {
            _navAgent.isStopped = true; //이걸 키면 강제 멈춤. 경로가 리셋된건 아냐.

            OffMeshLinkData linkData = _navAgent.currentOffMeshLinkData;
            Vector3 start = transform.position;
            Vector3 end = linkData.endPos;

            //최소 점프 시간인 0.3을 배치하고, 나머지는 거리기반으로 해서. 둘 중 큰 값을 가져온다.
            float jumpTime = Mathf.Max(0.3f, Vector3.Distance(start, end) / jumpSpeed);
            float currentTime = 0;
            float percent = 0;

            //로컬 좌표를 월드로 변환한다.
            SplineContainer splineContainer = linkData.owner.GetComponent<SplineContainer>();
            Vector3 first = splineContainer.Spline.Knots.First().Position;
            Vector3 last = splineContainer.Spline.Knots.Last().Position;

            first += splineContainer.transform.position;
            last += splineContainer.transform.position;

            //만약 시작점의 위치가 마지막점보다 멀다면 나는 마지막 점에 있는거다.
            bool isReversed = Vector3.Distance(first, transform.position) > Vector3.Distance(last, transform.position);
            
            
            while (percent < 1)
            {
                currentTime += Time.deltaTime;
                percent = currentTime / jumpTime;

                Vector3 position = splineContainer.EvaluatePosition(isReversed ? 1 - percent : percent);
                transform.position = position + new Vector3(0, 0.5f);
                yield return null;
            }

            _navAgent.CompleteOffMeshLink(); //오프 메시 링크를 건넜음을 에이전트에게 알려준다.

            _navAgent.isStopped = false;

        }
    }
}