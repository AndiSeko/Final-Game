using ASUL.Console;
using UnityEngine;

public class CSetPreset : MonoBehaviour
{
    public static void Play()
    {
        CSetAwaible.Interact(true);
        CSetAwaible.Walk(true);
        CSetAwaible.Look(true);
        CSetAwaible.Run(false);
        CSetAwaible.Jump(false);
    }
    
    public static void Full()
    {
        CSetAwaible.Interact(true);
        CSetAwaible.Walk(true);
        CSetAwaible.Look(true);
        CSetAwaible.Run(true);
        CSetAwaible.Jump(true);
    }

    public static void Sprint()
    {
        CSetAwaible.Interact(true);
        CSetAwaible.Walk(true);
        CSetAwaible.Look(true);
        CSetAwaible.Run(true);
        CSetAwaible.Jump(false);
    }

    public static void Jump()
    {
        CSetAwaible.Interact(true);
        CSetAwaible.Walk(true);
        CSetAwaible.Look(true);
        CSetAwaible.Run(false);
        CSetAwaible.Jump(true);
    }

    public static void Look()
    {
        CSetAwaible.Interact(false);
        CSetAwaible.Walk(false);
        CSetAwaible.Look(true);
        CSetAwaible.Run(false);
        CSetAwaible.Jump(false);
    }

    public static void Cutscene()
    {
        CSetAwaible.Interact(false);
        CSetAwaible.Walk(false);
        CSetAwaible.Look(false);
        CSetAwaible.Run(false);
        CSetAwaible.Jump(false);
    }
}
