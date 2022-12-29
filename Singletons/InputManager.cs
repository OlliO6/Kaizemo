using System;
using Godot;

public partial class InputManager : Node
{
    const float JumpBufferTime = 0.125f;

    public static InputManager Instance { get; private set; }

    public static event Action JumpPressed, JumpReleased, PausePressed;

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
