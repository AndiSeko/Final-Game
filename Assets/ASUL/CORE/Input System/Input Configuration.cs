using UnityEngine;

namespace ASUL.InputSystem 
{
    [CreateAssetMenu(fileName = "InputConfiguration", menuName = "ASUL/InputConfiguration")]
    public class InputConfiguration : ScriptableObject
    {
        public string DefaultInputMap = "Player";
        public CursorLockMode DefaultCursorLockMode = CursorLockMode.None;
    }
}