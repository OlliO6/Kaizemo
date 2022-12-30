using System;
using Godot;

public partial class InputManager : Node
{
    public enum ActionDirection { Up, Down, Left, Right }

    const float JumpBufferTime = 0.1f;

    public static InputManager Instance { get; private set; }

    public static event Action JumpPressed, JumpReleased, PausePressed;
    public static event Action<ActionDirection> ActionPressed;

    public static bool IsJumpBuffered => Instance.jumpBufferTimer.TimeLeft != 0 && !Instance.jumpBufferTimer.IsStopped();
    public static bool IsHoldingJump => Input.IsActionPressed(InputAction.Jump);

    private Timer jumpBufferTimer;

    public override void _EnterTree() => Instance = this;
    public override void _ExitTree() => Instance = null;

    public override void _Ready()
    {
        ProcessMode = ProcessModeEnum.Always;

        jumpBufferTimer = new()
        {
            OneShot = true,
            WaitTime = JumpBufferTime
        };
        AddChild(jumpBufferTimer);

        JumpPressed += StartJumpBuffer;
    }

    private void StartJumpBuffer() => jumpBufferTimer.Start();

    public static void UseJumpBuffer() => Instance.jumpBufferTimer.Stop();

    public static float GetPlayerHorizontalInput() => Input.GetAxis(InputAction.MoveLeft, InputAction.MoveRight);

    public override void _UnhandledInput(InputEvent @event)
    {
        if (@event.IsEcho())
            return;

        if (@event.IsAction(InputAction.Jump))
        {
            if (@event.IsPressed())
            {
                JumpPressed?.Invoke();
                return;
            }
            JumpReleased?.Invoke();
            return;
        }

        if (@event.IsActionPressed(InputAction.ActionUp))
        {
            ActionPressed?.Invoke(ActionDirection.Up);
            return;
        }

        if (@event.IsActionPressed(InputAction.ActionDown))
        {
            ActionPressed?.Invoke(ActionDirection.Down);
            return;
        }

        if (@event.IsActionPressed(InputAction.ActionLeft))
        {
            ActionPressed?.Invoke(ActionDirection.Left);
            return;
        }

        if (@event.IsActionPressed(InputAction.ActionRight))
        {
            ActionPressed?.Invoke(ActionDirection.Right);
            return;
        }

        if (@event is InputEventKey key && !key.Pressed && key.Keycode == Key.F11)
        {
            ToggleFullscreen();
            return;
        }
    }

    private void ToggleFullscreen()
    {
        if (DisplayServer.WindowGetMode() is DisplayServer.WindowMode.Fullscreen or DisplayServer.WindowMode.ExclusiveFullscreen)
        {
            DisplayServer.WindowSetMode(DisplayServer.WindowMode.Windowed);
            return;
        }

        DisplayServer.WindowSetMode(DisplayServer.WindowMode.Fullscreen);
    }
}
