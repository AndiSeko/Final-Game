using System.Collections;
using UnityEngine;

namespace ASUL.Electronics
{
    public class ElectricTransmitterTimed : ElectricTransmitter
    {

        [field: SerializeField] public float Time = 1f;

        public override void Press()
        {
            IsWorking = true;

            WaitAndDisconnect();
        }

        private IEnumerator WaitAndDisconnect()
        {
            yield return new WaitForSeconds(Time);
            IsWorking = false;
        }
        
    }
}
