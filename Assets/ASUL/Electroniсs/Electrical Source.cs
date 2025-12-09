using UnityEngine;

namespace ASUL.Electronics
{
    public class ElectricalSource : MonoBehaviour
    {
        public bool IsWorking { get => _isWorking; set { _isWorking = value; ToggleActive(value); } }

        public bool _isWorking = true;

        public virtual void ToggleActive(bool active)
        {

        }
    }

}