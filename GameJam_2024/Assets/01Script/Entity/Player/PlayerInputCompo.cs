using System;
using UnityEngine;

public class PlayerInputCompo : MonoBehaviour
{
    public event Action OnJumpKeyEvent;

    public event Action OnMouseDownEvent;
    public event Action OnMouseUpEvent;

    public Vector2 InputDirection { get; private set; }
    private bool _isMouseDown = false;

    private Player _player;

    public void Initialize(Entity entity)
    {
        _player = entity as Player;
    }

    private void Update()
    {
        MouseInput();

        if (_isMouseDown) return;
        MovementInput();
        JumpInput();
    }

    private void JumpInput()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            OnJumpKeyEvent?.Invoke();
        }
    }

    private void MouseInput()
    {
        if (Input.GetMouseButtonDown(0))
        {
            OnMouseDownEvent?.Invoke();
            _isMouseDown = true;
        }

        if (Input.GetMouseButtonUp(0))
        {
            OnMouseUpEvent?.Invoke();
            _isMouseDown = false;
        }
    }

    private void MovementInput()
    {
        float x = Input.GetAxis("Horizontal");

        InputDirection = new Vector2(x, 0);
    }
}
