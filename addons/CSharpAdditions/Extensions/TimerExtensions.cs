namespace Additions;

using Godot;
using System;
using System.Threading.Tasks;

public static class TimerExtensions
{
    public async static Task ToTimeout(this Timer from)
    {
        await from.ToSignal(from, Timer.SignalName.Timeout);
    }
}
