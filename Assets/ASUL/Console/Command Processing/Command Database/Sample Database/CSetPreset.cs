using ASUL.Console;
using ASUL.InputSystem;
using UnityEngine;

public class CSetPreset : MonoBehaviour
{
    public static Preset CurrentPreset = Preset.Play;

    public static void Play()
    {
        CSetAvailable.Interact(true);
        CSetAvailable.Walk(true);
        CSetAvailable.Look(true);
        CSetAvailable.Run(false);
        CSetAvailable.Jump(false);
        
        CurrentPreset = Preset.Play;

        RevertToPlay();
    }
    
    public static void Full()
    {
        CSetAvailable.Interact(true);
        CSetAvailable.Walk(true);
        CSetAvailable.Look(true);
        CSetAvailable.Run(true);
        CSetAvailable.Jump(true);

        CurrentPreset = Preset.Full;

        RevertToPlay();
    }

    public static void Sprint()
    {
        CSetAvailable.Interact(true);
        CSetAvailable.Walk(true);
        CSetAvailable.Look(true);
        CSetAvailable.Run(true);
        CSetAvailable.Jump(false);

        CurrentPreset = Preset.Sprint;

        RevertToPlay();
    }

    public static void Jump()
    {
        CSetAvailable.Interact(true);
        CSetAvailable.Walk(true);
        CSetAvailable.Look(true);
        CSetAvailable.Run(false);
        CSetAvailable.Jump(true);

        CurrentPreset = Preset.Jump;

        RevertToPlay();
    }

    public static void Look()
    {
        CSetAvailable.Interact(false);
        CSetAvailable.Walk(false);
        CSetAvailable.Look(true);
        CSetAvailable.Run(false);
        CSetAvailable.Jump(false);

        CurrentPreset = Preset.Look;

        RevertToPlay();
    }

    public static void Cutscene()
    {
        CSetAvailable.Interact(false);
        CSetAvailable.Walk(false);
        CSetAvailable.Look(false);
        CSetAvailable.Run(false);
        CSetAvailable.Jump(false);

        CurrentPreset = Preset.Cutscene;

        if (InputManager.Instance.CurrentState != "Cutscene")
        {
            InputManager.Instance.ChangeInputMap("Cutscene");
        }
    }

    private static void RevertToPlay()
    {
        if (InputManager.Instance.CurrentState == "Cutscene")
        {
            InputManager.Instance.ChangeInputMap("Player");
        }
    }

    public static void SetByValue(Preset preset)
    {
        switch (preset)
        {
            case Preset.Cutscene:
                Cutscene(); break;
            case Preset.Sprint:
                Sprint(); break;
            case Preset.Play:
                Play(); break;
            case Preset.Full:
                Full(); break;
            case Preset.Jump:
                Jump(); break;
            case Preset.Look:
                Look(); break;
        }
    }

    public enum Preset
    {
        Play,
        Full,
        Sprint,
        Jump,
        Look,
        Cutscene
    }
}