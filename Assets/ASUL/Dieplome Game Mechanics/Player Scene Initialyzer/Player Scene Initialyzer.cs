using ASUL.InputSystem;
using ASUL.NPC.PlayerDetection;
using UnityEngine;

public class PlayerSceneInitialyzer : MonoBehaviour
{
    [SerializeField] private PlayerVisibilityState playerVisibilityState = PlayerVisibilityState.Default;
    [SerializeField] private CSetPreset.Preset preset = CSetPreset.Preset.Play;

    private void Start()
    {
        InputManager.Instance.ChangeInputMap("Player", CursorLockMode.Locked);
        PlayerVisibilityManager.PlayerVisibilityState = playerVisibilityState;
        CSetPreset.SetByValue(preset);
    }
}
