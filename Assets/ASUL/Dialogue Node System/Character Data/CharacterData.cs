using UnityEngine;

namespace ASUL.DNS.Data
{
    [CreateAssetMenu(fileName = "Character", menuName = "ASUL/Dialogue Character")]
    public class CharacterData : ScriptableObject
    {
        public string Name = "noname";
        public Color NameColor = Color.white;
        public Sprite Icon = null;
    }
}