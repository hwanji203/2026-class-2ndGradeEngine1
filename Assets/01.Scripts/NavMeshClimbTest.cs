

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
            _navAgent = GetComponent<NavMeshAgent>(); //이거 꼭 써!!!!
            StartCoroutine(StartCoroutine());
        }

        private IEnumerator StartCoroutine()
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

            if (end.y > start.y)
            {
                while (percent < 1)
                {
                    currentTime += Time.deltaTime;
                    percent = currentTime / jumpTime;
                
                    float y = Mathf.Lerp(start.y, end.y, percent); //선형 보간.
                    //여기서 y값을 수정해주는 로직이 필요하다.
                    transform.position = new Vector3(transform.position.x, y + yOffset, transform.position.z);
                    yield return null;
                }
                currentTime = 0;
                percent = 0;
                while (percent < 1)
                {
                    currentTime += Time.deltaTime;
                    percent = currentTime / jumpTime;
                
                    Vector3 a = Vector3.Lerp(start, end, percent); //선형 보간.
                    //여기서 y값을 수정해주는 로직이 필요하다.
                    transform.position = new Vector3(a.x, transform.position.y, a.z);
                    yield return null;
                }
            }
            else
            {
                while (percent < 1)
                {
                    currentTime += Time.deltaTime;
                    percent = currentTime / jumpTime;
                
                    Vector3 a = Vector3.Lerp(start, end, percent); //선형 보간.
                    //여기서 y값을 수정해주는 로직이 필요하다.
                    transform.position = new Vector3(a.x, transform.position.y, a.z);
                    yield return null;
                }
                currentTime = 0;
                percent = 0;
                while (percent < 1)
                {
                    currentTime += Time.deltaTime;
                    percent = currentTime / jumpTime;
                
                    float y = Mathf.Lerp(start.y, end.y, percent); //선형 보간.
                    //여기서 y값을 수정해주는 로직이 필요하다.
                    transform.position = new Vector3(transform.position.x, y + yOffset, transform.position.z);
                    yield return null;
                }
            }

            _navAgent.CompleteOffMeshLink(); //오프 메시 링크를 건넜음을 에이전트에게 알려준다.
            
            _navAgent.isStopped = false;
        }
    }
}