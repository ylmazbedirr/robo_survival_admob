/* 
//using UnityEditor.Toolbars;
using UnityEngine;
using UnityEngine.InputSystem;


[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{
    private Vector2 _input;
    private CharacterController _characterController;
    private Vector3 _direction;

    [SerializeField] private float smoothTime = 0.05f;
    [SerializeField] private float speed = 10f;
    private float _currentVelocity;


    private void Awake()
    {
        _characterController = GetComponent<CharacterController>();



    }


    private void Update()
    {

        if (_direction == Vector3.zero) return;

        float targetAngle = Mathf.Atan2(_direction.x, _direction.z) * Mathf.Rad2Deg;
        float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref _currentVelocity, smoothTime);

        transform.rotation = Quaternion.Euler(0f, angle, 0f);
        _characterController.Move(_direction * speed * Time.deltaTime);

        // 🔒 Pozisyon sınırla
        Vector3 clampedPosition = transform.position;
        clampedPosition.x = Mathf.Clamp(clampedPosition.x, -18.5f, 18.5f);
        clampedPosition.z = Mathf.Clamp(clampedPosition.z, -18.2f, 19.2f);
        transform.position = clampedPosition;


    }


    public void Move(InputAction.CallbackContext context)
    {
        _input = context.ReadValue<Vector2>();
        _direction = new Vector3(_input.x, 0f, _input.y);
    }


}

*/




/*

// File: Assets/Scripts/PlayerController.cs
using UnityEngine;
using UnityEngine.InputSystem;

// Joystick Pack'teki FixedJoystick'i kullanıyoruz
// using JoystickPackNamespace; // yoksa gerekmez

[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{
    private Vector2 _input;
    private CharacterController _characterController;
    private Vector3 _direction;

    [Header("Movement")]
    [SerializeField] private float smoothTime = 0.05f;
    [SerializeField] private float speed = 10f;
    private float _currentVelocity;

    [Header("Mobile (Optional)")]
    [SerializeField] private FixedJoystick mobileJoystick; // Joystick Pack objesini sürükle
    [SerializeField] private float joystickDeadzone = 0.12f; // çok küçük titremeyi yut

    private void Awake()
    {
        _characterController = GetComponent<CharacterController>();
    }

    private void Update()
    {
        // Eğer ekranda FixedJoystick varsa ve anlamlı input veriyorsa onu kullan
        if (mobileJoystick != null)
        {
            var j = new Vector2(mobileJoystick.Horizontal, mobileJoystick.Vertical);
            if (j.sqrMagnitude > joystickDeadzone * joystickDeadzone)
            {
                _input = Vector2.ClampMagnitude(j, 1f);
            }
            else
            {
                // joystick boşsa, klavye/IM gui'den gelen son _input olduğu gibi kalır
            }
        }

        _direction = new Vector3(_input.x, 0f, _input.y);
        if (_direction == Vector3.zero) return;

        float targetAngle = Mathf.Atan2(_direction.x, _direction.z) * Mathf.Rad2Deg;
        float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref _currentVelocity, smoothTime);

        transform.rotation = Quaternion.Euler(0f, angle, 0f);
        _characterController.Move(_direction * speed * Time.deltaTime);

        // Alanı sınırlama (senin mevcut değerlerin)
        Vector3 clamped = transform.position;
        clamped.x = Mathf.Clamp(clamped.x, -18.5f, 18.5f);
        clamped.z = Mathf.Clamp(clamped.z, -18.2f, 19.2f);
        transform.position = clamped;
    }

    // New Input System'den (WASD, gamepad) gelir
    public void Move(InputAction.CallbackContext context)
    {
        _input = context.ReadValue<Vector2>();
        // mobileJoystick varsa ve aktif input veriyorsa Update'te override eder
    }
}


*/



// Fixed Joyistik Version

/*


// File: Assets/Scripts/PlayerController.cs
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{
    private Vector2 _input;
    private CharacterController _characterController;
    private Vector3 _direction;

    [Header("Movement")]
    [SerializeField] private float smoothTime = 0.05f;
    [SerializeField] private float speed = 5f;
    private float _currentVelocity;

    [Header("Mobile (Optional)")]
    [SerializeField] private FixedJoystick mobileJoystick; // sahnedeki joystick'i sürükle
    [SerializeField] private float joystickDeadzone = 0.12f;

    private bool _joystickActiveLast; // neden: bırakınca sıfırlamak

    private void Awake()
    {
        _characterController = GetComponent<CharacterController>();
    }

    private void Update()
    {
        // 1) Joystick varsa onu oku
        if (mobileJoystick != null)
        {
            Vector2 j = new Vector2(mobileJoystick.Horizontal, mobileJoystick.Vertical);

            float dz2 = joystickDeadzone * joystickDeadzone;
            if (j.sqrMagnitude > dz2)
            {
                _input = Vector2.ClampMagnitude(j, 1f);
                _joystickActiveLast = true;
            }
            else
            {
                // Joystick bırakıldıysa input'u net sıfırla
                if (_joystickActiveLast)
                {
                    _input = Vector2.zero;
                    _joystickActiveLast = false;
                }
                // değilse, klavyeden gelen son değer kalabilir
            }
        }

        _direction = new Vector3(_input.x, 0f, _input.y);
        if (_direction == Vector3.zero) return;

        float targetAngle = Mathf.Atan2(_direction.x, _direction.z) * Mathf.Rad2Deg;
        float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref _currentVelocity, smoothTime);

        transform.rotation = Quaternion.Euler(0f, angle, 0f);
        _characterController.Move(_direction * speed * Time.deltaTime);

        // sınırlar
        Vector3 clamped = transform.position;
        clamped.x = Mathf.Clamp(clamped.x, -18.5f, 18.5f);
        clamped.z = Mathf.Clamp(clamped.z, -18.2f, 19.2f);
        transform.position = clamped;
    }

    // New Input System (WASD/gamepad)
    public void Move(InputAction.CallbackContext context)
    {
        if (context.canceled)
        {
            _input = Vector2.zero;        // parmak/tuş bırakılınca dur
            return;
        }

        // klavye/gamepad değeri; joystick aktifse Update'te override edilir
        _input = context.ReadValue<Vector2>();
    }
}



*/


/*

// Dynamic Joyistik Version


using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    private Vector2 _input;
    private CharacterController _characterController;
    private Vector3 _direction;

    [Header("Movement")]
    [SerializeField] private float smoothTime = 0.05f;
    [SerializeField] private float speed = 5f;
    private float _currentVelocity;

    [Header("Mobile Joystick (Joystick Pack)")]
    [SerializeField] private Joystick dynamicJoystick;   // Canvas'taki Dynamic Joystick'i sürükle
    [Range(0f, 0.4f)][SerializeField] private float mobileDeadzone = 0.12f;

    private void Awake()
    {
        _characterController = GetComponent<CharacterController>();
       // QualitySettings.vSyncCount = 0;           // VSync kapalı (aksi halde cihazın pacing’i yönetir)
      //  Application.targetFrameRate = 60;          // Stabil 60 FPS
        // Application.targetFrameRate = (int)Screen.currentResolution.refreshRateRatio.value;

    }

    private void Update()
    {
        // JOYSTICK VARSA: her kare joystick'i KESİN olarak kaynak al
        if (dynamicJoystick != null)
        {
            Vector2 j = new Vector2(dynamicJoystick.Horizontal, dynamicJoystick.Vertical);
            if (j.magnitude < mobileDeadzone) j = Vector2.zero;
            _input = Vector2.ClampMagnitude(j, 1f);        // <- bırakınca _input = (0,0)
        }

        _direction = new Vector3(_input.x, 0f, _input.y);
        if (_direction == Vector3.zero) return;

        float targetAngle = Mathf.Atan2(_direction.x, _direction.z) * Mathf.Rad2Deg;
        float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref _currentVelocity, smoothTime);

        transform.rotation = Quaternion.Euler(0f, angle, 0f);
        _characterController.Move(_direction * speed * Time.deltaTime);

        // sınırlar
        Vector3 p = transform.position;
        p.x = Mathf.Clamp(p.x, -18.5f, 18.5f);
        p.z = Mathf.Clamp(p.z, -18.2f, 19.2f);
        transform.position = p;
    }

    // Yeni Input Sistemi (WASD). Joystick bağlıysa bunu YOK SAY.
    public void Move(InputAction.CallbackContext context)
    {
        if (dynamicJoystick != null) return;           // <- kritik
        if (context.canceled) { _input = Vector2.zero; return; }
        _input = context.ReadValue<Vector2>();
    }
}

*/


using UnityEngine;
using UnityEngine.InputSystem;


public class PlayerController : MonoBehaviour
{
    private Vector2 _input;
    private CharacterController _characterController;
    private Vector3 _direction;


    [Header("Movement")]
    [SerializeField] private float smoothTime = 0.05f;
    [SerializeField] private float speed = 9f;
    private float _currentVelocity;


    [Header("Mobile Joystick (Joystick Pack)")]
    [SerializeField] private Joystick dynamicJoystick;
    [Range(0f, 0.4f)][SerializeField] private float mobileDeadzone = 0.12f;


    private void Awake()
    {
        _characterController = GetComponent<CharacterController>();
    }

    private void Update()
    {
        if (dynamicJoystick != null)
        {
            Vector2 j = new Vector2(dynamicJoystick.Horizontal, dynamicJoystick.Vertical);
            if (j.magnitude < mobileDeadzone) j = Vector2.zero;
            _input = Vector2.ClampMagnitude(j, 1f);
        }


        _direction = new Vector3(_input.x, 0f, _input.y);
        if (_direction == Vector3.zero) return;


        float targetAngle = Mathf.Atan2(_direction.x, _direction.z) * Mathf.Rad2Deg;
        float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref _currentVelocity, smoothTime);
        transform.rotation = Quaternion.Euler(0f, angle, 0f);
        _characterController.Move(_direction * speed * Time.deltaTime);


        Vector3 p = transform.position;
        // p.x = Mathf.Clamp(p.x, -18.5f, 18.5f);
        //  p.z = Mathf.Clamp(p.z, -18.2f, 19.2f);

        p.x = Mathf.Clamp(p.x, -19.3f, 22.60f);
        p.z = Mathf.Clamp(p.z, -25f, 16.50f);

        transform.position = p;
    }


    public void Move(InputAction.CallbackContext context)
    {
        if (dynamicJoystick != null) return;
        if (context.canceled) { _input = Vector2.zero; return; }
        _input = context.ReadValue<Vector2>();
    }
}



