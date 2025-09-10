using UnityEngine;
using UnityEngine.InputSystem;


public class Wheels : MonoBehaviour
{
    [SerializeField] private float donmeHizi = 360f;


    void Update()
    {
        bool isMoving = Keyboard.current != null && (
        Keyboard.current.wKey.isPressed || Keyboard.current.sKey.isPressed ||
        Keyboard.current.aKey.isPressed || Keyboard.current.dKey.isPressed);


        if (isMoving)
        {
            transform.Rotate(Vector3.forward * donmeHizi * Time.deltaTime, Space.Self);
        }
    }
}
