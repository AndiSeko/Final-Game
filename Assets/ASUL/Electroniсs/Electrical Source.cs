using UnityEngine;

namespace ASUL.Electronics
{
    public class ElectricalSource : MonoBehaviour
    {
        [field: SerializeField] public bool IsWorking { get; set; } = true;
    }

}