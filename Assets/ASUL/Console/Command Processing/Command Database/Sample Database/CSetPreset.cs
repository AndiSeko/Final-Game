using ASUL.Console;
using UnityEngine;

public class CSetPreset : MonoBehaviour
{
    public static void Play()
    {
        CSetAvailable.Interact(true);
        CSetAvailable.Walk(true);
        CSetAvailable.Look(true);
        CSetAvailable.Run(false);
        CSetAvailable.Jump(false);
    }
    
    public static void Full()
    {
        CSetAvailable.Interact(true);
        CSetAvailable.Walk(true);
        CSetAvailable.Look(true);
        CSetAvailable.Run(true);
        CSetAvailable.Jump(true);
    }

    public static void Sprint()
    {
        CSetAvailable.Interact(true);
        CSetAvailable.Walk(true);
        CSetAvailable.Look(true);
        CSetAvailable.Run(true);
        CSetAvailable.Jump(false);
    }

    public static void Jump()
    {
        CSetAvailable.Interact(true);
        CSetAvailable.Walk(true);
        CSetAvailable.Look(true);
        CSetAvailable.Run(false);
        CSetAvailable.Jump(true);
    }

    public static void Look()
    {
        CSetAvailable.Interact(false);
        CSetAvailable.Walk(false);
        CSetAvailable.Look(true);
        CSetAvailable.Run(false);
        CSetAvailable.Jump(false);
    }

    public static void Cutscene()
    {
        CSetAvailable.Interact(false);
        CSetAvailable.Walk(false);
        CSetAvailable.Look(false);
        CSetAvailable.Run(false);
        CSetAvailable.Jump(false);
    }
}
