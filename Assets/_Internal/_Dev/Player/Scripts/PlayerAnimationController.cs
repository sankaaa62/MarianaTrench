using System;
using UnityEngine;

public class PlayerAnimationController : MonoBehaviour
{
    [SerializeField] private Animator _animator;
    [SerializeField] private float _runSpeedThreshold;
    [SerializeField] private Transform _body;

    private readonly string _animatorTakeDamageTriggerKey = "TakeDamage";
    private readonly string _animatorRunBoolKey = "IsRun";
    private readonly string _animatorIsDeathBoolKey = "IsDeath";
    private readonly string _animatorMoveSpeedFloatKey = "MoveSpeed";

    private float _forceCoeff;
    private float _speedCoeff;
    private Vector2 _oldPos;
    private Vector2 _deltaPos;

    public float ForceCoeff => _forceCoeff;

    private void Start()
    {
        _oldPos = _body.position;
    }

    private void FixedUpdate()
    {
        CalculateSpeedCoeff();
        
        _forceCoeff = Mathf.Clamp(_speedCoeff * 0.6f, 0.2f, 3f);
        
        if (_speedCoeff > _runSpeedThreshold)
            Run(_speedCoeff);
        else
            Stop();
    }

    private void CalculateSpeedCoeff()
    {
        //Debug.Log(_body.velocity.magnitude);
        //_speedCoeff = Mathf.Clamp01(_body.velocity.magnitude);
        //Debug.Log((_oldPos - (Vector2)_body.position).magnitude * Time.fixedDeltaTime * 1000);
        _speedCoeff = (_oldPos - (Vector2)_body.position).magnitude * Time.fixedDeltaTime * 1000;
        _oldPos = _body.position;
    }
    
    public bool IsRun()
    {
        return _animator.GetBool(_animatorRunBoolKey);
    }

    public void TakeDamage()
    {
        _animator.SetTrigger(_animatorTakeDamageTriggerKey);
    }
    
    public void Die()
    {
        _animator.SetBool(_animatorIsDeathBoolKey, true);
    }

    private void SetMoveSpeed(float speed)
    {
        _animator.SetFloat(_animatorMoveSpeedFloatKey, speed);
    }
    
    private void Run(float speed)
    {
        SetMoveSpeed(speed);
        
        if (!_animator.GetBool(_animatorRunBoolKey))
            _animator.SetBool(_animatorRunBoolKey, true);
    }

    private void Stop()
    {
        if (_animator.GetBool(_animatorRunBoolKey))
            _animator.SetBool(_animatorRunBoolKey, false);
    }
}
