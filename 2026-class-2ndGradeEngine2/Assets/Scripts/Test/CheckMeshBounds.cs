using UnityEngine;

namespace Test
{
    public class MeshBoundChecker : MonoBehaviour
    {
        [ContextMenu("Check mesh bound")]
        private void CheckMeshBound()
        {
            if (TryGetComponent<SkinnedMeshRenderer>(out var skinnedMeshRenderer))
            {
                Bounds local = skinnedMeshRenderer.localBounds;
                Bounds raw = skinnedMeshRenderer.sharedMesh.bounds;
                
                Debug.Log($"[MeshBound] OS(셰이더 기준) - Y Min : {local.min.y:F4}, Y Max : {local.max.y:F4}, Height : {local.size.y:F4}");
                Debug.Log($"[MeshBound] Raw mesh(원본 기준) - Y Min : {raw.min.y:F4}, Y Max : {raw.max.y:F4}, Height : {raw.size.y:F4}");
                return;
            }
        }
    }
}