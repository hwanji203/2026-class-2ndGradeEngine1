using Players;
using Scenes.TestScene;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;

public class TestPlayerMove : MonoBehaviour
{
    [SerializeField] private PlayerInputSO _input;
    [SerializeField] private float speed = 10;
    [SerializeField] private GameObject boxPrefab;
    private Vector3 _targetPos = Vector3.zero;
    private CharacterController _controller;

    private GameObject box;
    private Vector3 _movementDirection;
    
    private void Awake()
    {
        _controller = GetComponent<CharacterController>();
        _input.OnMovementChanged += Move;
    }

    private void Move(Vector2 obj)
    {
        _movementDirection = Quaternion.Euler(0, -45, 0) * new Vector3(obj.x, 0, obj.y);
    }

    private void Update()
    {
        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            OnShoot();
        }
        
        Vector3 _direction = (_input.GetWorldMousePosition() - transform.position).normalized;
        GetComponent<PlayerRayCast>().RayCast(new(transform.position,_direction));
        
        _targetPos = _input.GetWorldMousePosition();
        _targetPos.y = transform.position.y;
        transform.rotation = Quaternion.LookRotation((_targetPos -  transform.position).normalized);
        
        _controller.Move(_movementDirection * (speed * Time.deltaTime));
    }

    private void OnShoot()
    {
        Vector3 vector = UnityEngine.Random.onUnitSphere;
        GetComponent<CinemachineImpulseSource>().GenerateImpulse(vector);
        GetComponentInChildren<ParticleSystem>().Play();
        Bullet bullet = Instantiate(boxPrefab, transform.position, Quaternion.identity).GetComponent<Bullet>();
        bullet.direction = (_targetPos - transform.position).normalized;
    }
}
