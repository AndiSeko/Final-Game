using UnityEngine;

namespace ASUL.Console
{
    public abstract class CommandDatabase : MonoBehaviour
    {
        public abstract void FindAndPerform(Command command);
    }
}