namespace StateMachines;

using Godot;
using System;
using System.Collections.Generic;

public partial class State : RefCounted, IState
{
    [Signal]
    public delegate void EnteredEventHandler();
    [Signal]
    public delegate void ExitedEventHandler();
    [Signal]
    public delegate void ProcessEventHandler(double delta);
    [Signal]
    public delegate void PhysicsProcessEventHandler(double delta);

    public Enum Id { get; }
    public List<Enum> Tags { get; set; } = new();
    public StateMachine StateMachine { get; }

    public State(Enum identifier, StateMachine stateMachine)
    {
        Id = identifier;
        StateMachine = stateMachine;
    }

    public void Enter()
    {
        GetEmitter<SignalEmitter.Entered>().Emit();

        StateMachine._InternalProcess += EmitProcess;
        StateMachine._InternalPhysicsProcess += EmitPhysicsProcess;
    }

    public void Exit()
    {
        GetEmitter<SignalEmitter.Exited>().Emit();

        StateMachine._InternalProcess -= EmitProcess;
        StateMachine._InternalPhysicsProcess -= EmitPhysicsProcess;
    }

    private void EmitProcess(double delta) => GetEmitter<SignalEmitter.Process>().Emit(delta);

    private void EmitPhysicsProcess(double delta) => GetEmitter<SignalEmitter.PhysicsProcess>().Emit(delta);

    public IState WithEnterCallback(Action callback, bool defer, bool onlyCallWhenActive = false)
    {
        if (!defer)
        {
            Entered += callback.Invoke;
            return this;
        }

        if (onlyCallWhenActive)
        {
            Connect(SignalName.Entered, Callable.From(() =>
            {
                if (StateMachine.CurrentState == this)
                    callback();
            }), (uint)ConnectFlags.Deferred);
            return this;
        }

        Connect(SignalName.Entered, Callable.From(callback), (uint)ConnectFlags.Deferred);
        return this;
    }

    public IState WithExitCallback(Action callback, bool defer, bool onlyCallWhenActive = false)
    {
        if (!defer)
        {
            Exited += callback.Invoke;
            return this;
        }

        if (onlyCallWhenActive)
        {
            Connect(SignalName.Exited, Callable.From(() =>
            {
                if (StateMachine.CurrentState == this)
                    callback();
            }), (uint)ConnectFlags.Deferred);
            return this;
        }

        Connect(SignalName.Exited, Callable.From(callback), (uint)ConnectFlags.Deferred);
        return this;
    }

    public IState WithProcessCallback(ProcessCallback callback, bool defer, bool onlyCallWhenActive = true)
    {
        if (!defer)
        {
            if (onlyCallWhenActive)
            {
                Connect(SignalName.Process, Callable.From((Action<double>)((delta) =>
                {
                    if (StateMachine.CurrentState == this)
                        callback(delta);
                })));
                return this;
            }

            Process += callback.Invoke;
            return this;
        }

        if (onlyCallWhenActive)
        {
            Connect(SignalName.Process, Callable.From((Action<double>)((delta) =>
            {
                if (StateMachine.CurrentState == this)
                    callback(delta);
            })), (uint)ConnectFlags.Deferred);
            return this;
        }

        Connect(SignalName.Process, Callable.From((Action<double>)callback.Invoke), (uint)ConnectFlags.Deferred);
        return this;
    }

    public IState WithPhysicsProcessCallback(ProcessCallback callback, bool defer, bool onlyCallWhenActive = true)
    {
        if (!defer)
        {
            if (onlyCallWhenActive)
            {
                Connect(SignalName.PhysicsProcess, Callable.From((Action<double>)((delta) =>
                {
                    if (StateMachine.CurrentState == this)
                        callback(delta);
                })));
                return this;
            }

            PhysicsProcess += callback.Invoke;
            return this;
        }

        if (onlyCallWhenActive)
        {
            Connect(SignalName.PhysicsProcess, Callable.From((Action<double>)((delta) =>
            {
                if (StateMachine.CurrentState == this)
                    callback(delta);
            })), (uint)ConnectFlags.Deferred);
            return this;
        }

        Connect(SignalName.PhysicsProcess, Callable.From((Action<double>)callback.Invoke), (uint)ConnectFlags.Deferred);
        return this;
    }

    public override string ToString() => Id.ToString();
}
