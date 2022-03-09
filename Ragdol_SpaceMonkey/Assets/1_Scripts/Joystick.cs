using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Joystick: MonoBehaviour
{
    public Movement movement;
    private Vector2 _direction;
    private Vector2 startTouchPosition;

    private void Start()
    {
        movement = GetComponent<Movement>();
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
            startTouchPosition = Input.mousePosition;
        if (Input.GetMouseButton(0))
            _direction = Vector2.ClampMagnitude((Vector2)Input.mousePosition - startTouchPosition, 1);
        if (Input.GetMouseButtonUp(0))
            _direction = Vector3.zero;


        if (movement != null)
            movement.SetDirection(new Vector3 (_direction.x, 0, _direction.y));
    }
}
