using UnityEngine;

public class Balancer : MonoBehaviour
{
    [SerializeField] private PlayerAnimationController _animationController;
    [SerializeField] private float _targetLocalRotation;
    [SerializeField] private float _force = 300;
    [Space]
    [SerializeField] private Transform _targetTransform;
    [SerializeField] private bool _isFollow;

    private float _targetRotation;
    private Rigidbody2D _rigidbody;
    private Rigidbody2D _parent;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        _parent = transform.parent.GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        CalculateTargetRotation();
        BalanceRotation();
    }

    private void BalanceRotation()
    {
        _rigidbody.MoveRotation(Mathf.LerpAngle(
            _rigidbody.rotation, 
            _targetRotation, 
            _force * _animationController.ForceCoeff * Time.deltaTime));
    }

    private void CalculateTargetRotation()
    {
        if (!_isFollow)
            _targetRotation = _parent.rotation + _targetLocalRotation;
        else
            _targetRotation = _parent.rotation + _targetTransform.localRotation.eulerAngles.z;
    }
}
