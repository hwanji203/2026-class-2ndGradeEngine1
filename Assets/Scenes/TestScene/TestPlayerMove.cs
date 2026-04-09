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

    private void Awake()
    {
        _controller = GetComponent<CharacterController>();
    }

    private void Update()
    {
        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            _targetPos = _input.GetWorldMousePosition();
            _targetPos.y = transform.position.y;
            if (box != null)
            {
                Destroy(box);
                box = null;
            }
            box = Instantiate(boxPrefab, _targetPos, Quaternion.identity);
            transform.LookAt(_targetPos);
            OnShoot();
        }
        if (box == null)
            return;
        if (box != null)
        {
            transform.LookAt(_targetPos);
        }
        
        Vector3 playerPos = transform.position;
        Vector3 _velocity = transform.forward;
        _velocity *= speed * Time.deltaTime;
        if ((_targetPos - playerPos).magnitude >= _velocity.magnitude)
        {
            _controller.Move(_velocity);
        }
        else if (box != null)
        {
            transform.position = _targetPos;
            Destroy(box);
            box = null;
        }
    }

    private void OnShoot()
    {
        Vector3 _direction = (_input.GetWorldMousePosition() - transform.position).normalized;
        GetComponent<PlayerRayCast>().RayCast(new(transform.position,_direction));
        Vector3 vector = UnityEngine.Random.onUnitSphere;
        GetComponent<CinemachineImpulseSource>().GenerateImpulse(vector);
        GetComponentInChildren<ParticleSystem>().Play();
        Bullet bullet = Instantiate(boxPrefab, transform.position, Quaternion.identity).GetComponent<Bullet>();
        bullet.direction = (_targetPos - transform.position).normalized;
    }
}
