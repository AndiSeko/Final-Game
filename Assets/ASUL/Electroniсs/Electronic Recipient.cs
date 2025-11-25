using System.Collections;
using UnityEngine;

namespace ASUL.Electronics
{
    public class ElectronicRecipient : ElectricTransmitter
    {
        public override void CalculateWirework()
        {
            if (!IsIndependent)
            {
                bool totalResult = false;

                if (logicalType == LogicalType.OR)
                {
                    totalResult = false;

                    foreach (var source in ElectricalSources)
                    {
                        totalResult |= source.IsWorking;
                    }
                }

                if (logicalType == LogicalType.AND)
                {
                    totalResult = true;

                    foreach (var source in ElectricalSources)
                    {
                        totalResult &= source.IsWorking;
                    }
                }

                IsWorking = totalResult;
            }
        }
    }
}
