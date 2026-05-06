using System;
using System.Collections.Generic;
using System.Linq;
using Players;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using Random = UnityEngine.Random;

namespace _01.Scripts
{
    public class RoadGridSystem : MonoBehaviour
    {
        [SerializeField] private PlayerInputSO playerInput;
        [SerializeField] private GameObject roadPrefab;
        [SerializeField] private GameObject agentPrefab;
        [SerializeField] private CinemachineCamera followCam;
        [SerializeField] private bool canCombineMesh = true;
        
        private GameObject _agent;
        private Grid _mainGrid;
        private MeshFilter _meshFilter;

        public UnityEvent<bool> OnConstructionModeChange;
        public UnityEvent OnUpdateRoadMap;

        private bool _isConstruction;
        private HashSet<Vector3Int> _buildPositions = new();

        
        public bool IsConstruction
        {
            get => _isConstruction;
            set
            {
                _isConstruction = value;
                OnConstructionModeChange?.Invoke(value);
            }
        }

        private void Awake()
        {
            _mainGrid = GetComponent<Grid>();
            _meshFilter = GetComponent<MeshFilter>();
            _meshFilter.mesh = new Mesh(); //워닝 안뜨게 빈거
            playerInput.OnAttackKeyPressed += HandleMouseClick;
        }

        private void Start()
        {
            OnConstructionModeChange?.Invoke(IsConstruction);
        }

        private void OnDestroy()
        {
            playerInput.OnAttackKeyPressed -= HandleMouseClick;
        }

        private void HandleMouseClick()
        {
            if (IsConstruction == false) return;

            Vector3 worldPos = playerInput.GetWorldMousePosition();
            Vector3Int gridPos = _mainGrid.WorldToCell(worldPos);

            if (_buildPositions.Add(gridPos))
            {
                Vector3 center = _mainGrid.GetCellCenterWorld(gridPos);
                GameObject road = Instantiate(roadPrefab, center, Quaternion.identity);
                road.transform.SetParent(transform);

                if (canCombineMesh)
                    CombineMesh();
                
                OnUpdateRoadMap?.Invoke();
            }
        }

        private void CombineMesh()
        {
            MeshFilter[] meshFilters = _meshFilter.GetComponentsInChildren<MeshFilter>();
            CombineInstance[] combine = new CombineInstance[meshFilters.Length];
            
            int vertexCount = 0;
            for (int i = 0; i < meshFilters.Length; i++)
            {
                if (meshFilters[i].sharedMesh == null) continue;
                
                combine[i].mesh = meshFilters[i].sharedMesh;
                combine[i].transform = meshFilters[i].transform.localToWorldMatrix;
                meshFilters[i].gameObject.SetActive(false);
                
                vertexCount += meshFilters[i].sharedMesh.vertexCount;
            }
            
            _meshFilter.mesh = new Mesh(); //안전하게 이전 메시 폐기 후 다시 만들기
            if (vertexCount > 65535)
            {
                _meshFilter.mesh.indexFormat = UnityEngine.Rendering.IndexFormat.UInt32;
            }
            
            _meshFilter.mesh.CombineMeshes(combine); //모아둔 메시 수집 후 다시 컴바인
            gameObject.SetActive(true); //자기자신도 포함되어있으므로, 다시 켜기
        }

        public void GeneratePlayer()
        {
            int randomIndex = Random.Range(0, _buildPositions.Count);
            Vector3 startPos = _buildPositions.ElementAt(randomIndex);

            if (_agent != null)
            {
                Destroy(_agent);
            }
            _agent = Instantiate(agentPrefab, startPos, Quaternion.identity);
            followCam.Follow = _agent.transform;
        }

        private void Update()
        {
            if (Keyboard.current.tabKey.wasPressedThisFrame)
            {
                IsConstruction = !IsConstruction;
            }
        }
    }
}
