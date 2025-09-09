// File: Assets/Scripts/PlayerController.cs
using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// Safe movement controller: guards against NaN/Infinity during ad/retry flows.
/// WHY: Ad/show or retry can leave inputs/velocities invalid; we sanitize every frame.
/// </summary>
public class PlayerController : MonoBehaviour
{
    private Vector2 _input;
    private CharacterController _characterController;
    private Vector3 _direction;

    [Header("Movement")]
    [SerializeField]
    private float smoothTime = 0.05f;

    [SerializeField]
    private float speed = 10f;
    private float _currentVelocity;

    [Header("Mobile Joystick (Joystick Pack)")]
    [SerializeField]
    private Joystick dynamicJoystick;

    [Range(0f, 0.4f)]
    [SerializeField]
    private float mobileDeadzone = 0.12f;

    [Header("Runtime State")]
    [SerializeField]
    private bool isDead = false; // WHY: hard-stop update when dead

    private void Awake()
    {
        _characterController = GetComponent<CharacterController>();
    }

    private static bool Finite(float f) => !(float.IsNaN(f) || float.IsInfinity(f));

    private static bool Finite(Vector2 v) => Finite(v.x) && Finite(v.y);

    private static bool Finite(Vector3 v) => Finite(v.x) && Finite(v.y) && Finite(v.z);

    private void Update()
    {
        if (isDead)
            return; // WHY: prevent any motion/rotation when dead

        // sanitize stale values (once NaN, always NaN unless reset)
        if (!Finite(_currentVelocity))
            _currentVelocity = 0f;
        if (!Finite(_input))
            _input = Vector2.zero;

        // mobile joystick input path
        if (dynamicJoystick != null)
        {
            Vector2 j = new Vector2(dynamicJoystick.Horizontal, dynamicJoystick.Vertical);
            if (!Finite(j))
                j = Vector2.zero; // WHY: some joystick impls return NaN when disabled
            if (j.magnitude < mobileDeadzone)
                j = Vector2.zero;
            _input = Vector2.ClampMagnitude(j, 1f);
        }

        // build direction
        _direction = new Vector3(_input.x, 0f, _input.y);
        if (!Finite(_direction) || _direction == Vector3.zero)
            return;

        // angle math
        float targetAngle = Mathf.Atan2(_direction.x, _direction.z) * Mathf.Rad2Deg;
        if (!Finite(targetAngle))
            return;

        float angle = Mathf.SmoothDampAngle(
            transform.eulerAngles.y,
            targetAngle,
            ref _currentVelocity,
            smoothTime
        );
        if (!Finite(angle))
        {
            _currentVelocity = 0f;
            return;
        }

        transform.rotation = Quaternion.Euler(0f, angle, 0f);

        // move
        Vector3 move = _direction * speed * Time.deltaTime;
        if (!Finite(move))
            return;
        _characterController.Move(move);

        // bounds
        Vector3 p = transform.position;
        if (!Finite(p))
            return;
        p.x = Mathf.Clamp(p.x, -19.4f, 22.60f);
        p.z = Mathf.Clamp(p.z, -25f, 16.50f);
        transform.position = p;
    }

    public void Move(InputAction.CallbackContext context)
    {
        if (dynamicJoystick != null)
            return; // WHY: mobile path owns _input
        if (context.canceled)
        {
            _input = Vector2.zero;
            return;
        }
        var v = context.ReadValue<Vector2>();
        _input = Finite(v) ? v : Vector2.zero; // WHY: sanitize broken input streams
    }

    public void Die()
    {
        isDead = true;
        _input = Vector2.zero;
        _currentVelocity = 0f;
    }

    public void Revive()
    {
        isDead = false;
        _input = Vector2.zero;
        _currentVelocity = 0f;
    }
}
