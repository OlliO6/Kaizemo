namespace StateMachines;

using Godot;
using System;
using System.Collections.Generic;

public interface IState
{
    event State.EnteredEventHandler Entered;
    event State.ExitedEventHandler Exited;
    event State.ProcessEventHandler Process;
    event State.PhysicsProcessEventHandler PhysicsProcess;

    Enum Id { get; }
    List<Enum> Tags { get; set; }
    StateMachine StateMachine { get; }

    IState WithTag(Enum tag)
    {
        Tags.Add(tag);
        return this;
    }

    IState WithEnterCallback(Action callback, bool defer = false, bool onlyCallWhenActive = false);

    IState WithExitCallback(Action callback, bool defer = false, bool onlyCallWhenActive = false);

    IState WithProcessCallback(ProcessCallback callback, bool defer = false, bool onlyCallWhenActive = true);

    IState WithPhysicsProcessCallback(ProcessCallback callback, bool defer = false, bool onlyCallWhenActive = true);

    bool HasTag(Enum tag) => Tags.Contains(tag);

    Variant GetMeta(StringName name, Variant @default = default);

    bool HasMeta(StringName name);

    void SetMeta(StringName name, Variant value);

    void RemoveMeta(StringName name);
}
