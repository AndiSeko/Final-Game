using UnityEngine;

namespace ASUL.EventSystem
{
    public class ASULStaticClass
    {
        protected class Runner : MonoBehaviour { }

        protected static Runner runner;
        
        protected static void InitRunner<T>() where T : Runner
        {
            if (runner == null)
            {
                var go = new GameObject(typeof(T).Name);
                Object.DontDestroyOnLoad(go);
                runner = go.AddComponent<T>();
            }
        }
    }
}