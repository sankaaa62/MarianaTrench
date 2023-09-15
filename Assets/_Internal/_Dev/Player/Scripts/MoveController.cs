using UnityEngine;

public class MoveController : MonoBehaviour
{
    [SerializeField] private Transform _anchor;
    [SerializeField] private float _maxSpeed;
    [SerializeField] private float _maxControlDeviation;
    [SerializeField] private AnimationCurve _deviationCurve;
    [SerializeField] private float _inertia;

    private Camera _camera;
    private Vector2 _anchorViewportPosition;
    private Vector2 _mouseViewportPosition;
    private float _controlDeviation;
    private Vector2 _heading;
    private Vector2 _direction;
    private float _distance;

    private void Awake()
    {
        _camera = Camera.main;
    }

    private void Start()
    {
        OffCollision();
    }

    private void FixedUpdate()
    {
        Move();
    }
    
    private void OffCollision()
    {
        var colliders = transform.GetComponentsInChildren<Collider2D>();
        for (int i = 0; i < colliders.Length; i++)
        {
            for (int j = 0; j < colliders.Length; j++)
            {
                Physics2D.IgnoreCollision(colliders[i], colliders[j]);
            }
        }
    }

    private void Move()
    {
        if (Input.GetMouseButton(0))
            CalculateMoveParams();
        else
            SlowDownParams();
            
        //_anchor.Translate(_direction * _distance * Time.fixedDeltaTime);
        _anchor.position = _anchor.position + (Vector3)(_direction * _distance * Time.fixedDeltaTime);
    }

    private void CalculateMoveParams()
    {
        _anchorViewportPosition = _camera.WorldToViewportPoint(_anchor.position);
        _mouseViewportPosition = _camera.ScreenToViewportPoint(Input.mousePosition);

        _heading = _mouseViewportPosition - _anchorViewportPosition;
        
        _controlDeviation = Mathf.Clamp(_heading.magnitude, 0f, _maxControlDeviation);
        _distance = _maxSpeed * _deviationCurve.Evaluate(_controlDeviation / _maxControlDeviation);
        _direction = _heading.normalized;
    }

    private void SlowDownParams()
    {
        _distance = Mathf.Lerp(_distance, 0f, (1 - _inertia) * Time.deltaTime);
    }
}
