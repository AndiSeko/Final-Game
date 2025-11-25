using UnityEngine;

namespace ASUL.Electronics
{
    public class ElectricTransmitter : ElectricalSource
    {
        [field: SerializeField] public bool IsIndependent { get; set; } = false;

        public ElectricalSource[] ElectricalSources;

        public LogicalType logicalType;

        private void Update()
        {
             CalculateWirework();
        }

        public virtual void Press()
        {
            IsWorking = !IsWorking;
        }

        public virtual void CalculateWirework()
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

                IsWorking &= totalResult;
            }
        }
        

        public enum LogicalType
        {
            OR,
            AND
        }
    }
}
