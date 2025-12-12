using ASUL.NPC.PlayerDetection;
using UnityEngine;

public class PlayerSceneInitialyzer : MonoBehaviour
{
    [SerializeField] private PlayerVisibilityState playerVisibilityState = PlayerVisibilityState.Default;
    [SerializeField] private CSetPreset.Preset preset = CSetPreset.Preset.Play;

    private void Start()
    {
        PlayerVisibilityManager.PlayerVisibilityState = playerVisibilityState;
        CSetPreset.SetByValue(preset);
    }
}
