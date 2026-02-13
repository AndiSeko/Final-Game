using ASUL.InitialScripts;
using ASUL.InputSystem;
using ASUL.NPC.PlayerDetection;
using UnityEngine;

public class DieplomePlayerSceneInitializer : PlayerSceneInitialyzer
{
    [SerializeField] private PlayerVisibilityState playerVisibilityState = PlayerVisibilityState.Default;
    [SerializeField] private CSetPreset.Preset preset = CSetPreset.Preset.Play;

    private protected override void Start()
    {
        base.Start();
        PlayerVisibilityManager.PlayerVisibilityState = playerVisibilityState;
        CSetPreset.SetByValue(preset);
    }
}
