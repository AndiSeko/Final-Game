using UnityEngine;

namespace ASUL.DNS.Error
{
    public class DialogueErrorData
    {

        public Color Color { get; set; }

        public DialogueErrorData()
        {
            GenerateRandomColor();
        }

        private void GenerateRandomColor()
        {
            Color = Color.red;
        }
    }
}