namespace StateMachines;

using Godot;
using System;
using System.Collections.Generic;
using System.Linq;

public partial class StateMachine : Node
{
    [Signal]
    public delegate void StateChangedEventHandler();
    [Signal]
    public delegate void ProcessAppliedEventHandler(double delta);
    [Signal]
    public delegate void PhysicsProcessAppliedEventHandler(double delta);

    internal event ProcessCallback _InternalProcess, _InternalPhysicsProcess;

    public List<IState> states = new();

    private State _currentState;
    private State _previousState;

    private Dictionary<Enum, List<Action>> _tagEnterCallbacks;
    private Dictionary<Enum, List<Action>> _tagExitCallbacks;
    private Dictionary<Enum, List<ProcessCallback>> _tagProcessCallbacks;
    private Dictionary<Enum, List<ProcessCallback>> _tagPhysicsProcessCallbacks;

    public IState CurrentState => _currentState;
    public IState PreviousState => _previousState;

    public Dictionary<Enum, List<Action>> TagEnterCallbacks => _tagEnterCallbacks ??= new();
    public Dictionary<Enum, List<Action>> TagExitCallbacks => _tagExitCallbacks ??= new();
    public Dictionary<Enum, List<ProcessCallback>> TagProcessCallbacks => _tagProcessCallbacks ??= new();
    public Dictionary<Enum, List<ProcessCallback>> TagPhysicsProcessCallbacks => _tagPhysicsProcessCallbacks ??= new();

    public IState AddState(Enum stateId)
    {
        State state = new State(stateId, this);
        states.Add(state);
        return state;
    }

    public IState GetState(Enum stateId) => states.FirstOrDefault((state) => state.Id.Equals(stateId));

    public List<IState> GetStatesWithTag(Enum tag) => states.Where((state) => state.HasTag(tag)).ToList();

    public StateMachine WithEnterTagCallback(Enum tag, Action callback, bool defer = false, bool onlyCallWhenActive = false)
    {
        if (!TagEnterCallbacks.ContainsKey(tag))
            TagEnterCallbacks.Add(tag, new());

        if (!defer)
        {
            TagEnterCallbacks[tag].Add(callback);
            return this;
        }

        if (onlyCallWhenActive)
        {
            TagEnterCallbacks[tag].Add(() => Callable.From(() =>
            {
                if (CurrentState.HasTag(tag))
                    callback();
            }).CallDeferred());
            return this;
        }

        TagEnterCallbacks[tag].Add(() => Callable.From(callback.Invoke).CallDeferred());
        return this;
    }

    public StateMachine WithExitTagCallback(Enum tag, Action callback, bool defer = false, bool onlyCallWhenActive = false)
    {
        if (!TagExitCallbacks.ContainsKey(tag))
            TagExitCallbacks.Add(tag, new());

        if (!defer)
        {
            TagExitCallbacks[tag].Add(callback);
            return this;
        }

        if (onlyCallWhenActive)
        {
            TagExitCallbacks[tag].Add(() => Callable.From(() =>
            {
                if (CurrentState.HasTag(tag))
                    callback();
            }).CallDeferred());
            return this;
        }

        TagExitCallbacks[tag].Add(() => Callable.From(callback.Invoke).CallDeferred());
        return this;
    }

    public StateMachine WithProcessTagCallback(Enum tag, ProcessCallback callback, bool defer = false, bool onlyCallWhenActive = true)
    {
        if (!TagProcessCallbacks.ContainsKey(tag))
            TagProcessCallbacks.Add(tag, new());

        if (!defer)
        {
            TagProcessCallbacks[tag].Add(callback);
            return this;
        }

        if (onlyCallWhenActive)
        {
            TagProcessCallbacks[tag].Add((delta) => Callable.From((Action<double>)((_) =>
            {
                if (CurrentState.HasTag(tag))
                    callback(delta);
            })).CallDeferred(delta));
            return this;
        }

        TagProcessCallbacks[tag].Add((delta) => Callable.From((Action<double>)callback.Invoke).CallDeferred(delta));
        return this;
    }

    public StateMachine WithPhysicsProcessTagCallback(Enum tag, ProcessCallback callback, bool defer = false, bool onlyCallWhenActive = true)
    {
        if (!TagPhysicsProcessCallbacks.ContainsKey(tag))
            TagPhysicsProcessCallbacks.Add(tag, new());

        if (!defer)
        {
            TagPhysicsProcessCallbacks[tag].Add(callback);
            return this;
        }

        if (onlyCallWhenActive)
        {
            TagPhysicsProcessCallbacks[tag].Add((delta) => Callable.From((Action<double>)((_) =>
            {
                if (CurrentState.HasTag(tag))
                    callback(delta);
            })).CallDeferred(delta));
            return this;
        }

        TagPhysicsProcessCallbacks[tag].Add((delta) => Callable.From((Action<double>)callback.Invoke).CallDeferred(delta));
        return this;
    }

    public void SwitchStateIf(Enum stateId, bool condition)
    {
        if (condition)
            SwitchToState(stateId);
    }

    public void SwitchToState(Enum stateId) => SwitchToState(GetState(stateId));

    public void SwitchToState(IState state) => SwitchToState((State)state);

    private void SwitchToState(State newState)
    {
        _previousState = _currentState;
        _currentState = newState;

        _previousState?.Exit();
        _currentState?.Enter();

        // Invoke and connect tag callbacks

        if (CurrentState != null)
            foreach (var tag in CurrentState.Tags)
            {
                if (PreviousState != null && PreviousState.HasTag(tag))
                    continue;

                if (_tagEnterCallbacks?.ContainsKey(tag) ?? false)
                    foreach (var enterCallback in TagEnterCallbacks[tag])
                        enterCallback.Invoke();

                if (_tagProcessCallbacks?.ContainsKey(tag) ?? false)
                    foreach (var processCallback in TagProcessCallbacks[tag])
                        _InternalProcess += processCallback;

                if (_tagPhysicsProcessCallbacks?.ContainsKey(tag) ?? false)
                    foreach (var physicsprocessCallback in TagPhysicsProcessCallbacks[tag])
                        _InternalPhysicsProcess += physicsprocessCallback;
            }

        if (PreviousState != null)
            foreach (var tag in PreviousState.Tags)
            {
                if (CurrentState != null && CurrentState.HasTag(tag))
                    continue;

                if (_tagExitCallbacks?.ContainsKey(tag) ?? false)
                    foreach (var exitCallback in TagExitCallbacks[tag])
                        exitCallback.Invoke();

                if (_tagProcessCallbacks?.ContainsKey(tag) ?? false)
                    foreach (var processCallback in TagProcessCallbacks[tag])
                        _InternalProcess -= processCallback;

                if (_tagPhysicsProcessCallbacks?.ContainsKey(tag) ?? false)
                    foreach (var physicsprocessCallback in TagPhysicsProcessCallbacks[tag])
                        _InternalPhysicsProcess -= physicsprocessCallback;
            }

        GetEmitter<SignalEmitter.StateChanged>().Emit();
    }

    public override void _Process(double delta)
    {
        _InternalProcess?.Invoke(delta);
        GetEmitter<SignalEmitter.ProcessApplied>().Emit(delta);
    }

    public override void _PhysicsProcess(double delta)
    {
        _InternalPhysicsProcess?.Invoke(delta);
        GetEmitter<SignalEmitter.PhysicsProcessApplied>().Emit(delta);
    }

    public string GetCurrentStateAsString(bool withTags)
    {
        if (CurrentState == null)
            return "null";

        string result = CurrentState.ToString();

        if (withTags)
        {
            result += " [";

            for (int i = 0; i < CurrentState.Tags.Count; i++)
            {
                if (i != 0)
                    result += ", ";

                result += CurrentState.Tags[i].ToString();
            }

            result += "]";
        }

        return result;
    }
}
