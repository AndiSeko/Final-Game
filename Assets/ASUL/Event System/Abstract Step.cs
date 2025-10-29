using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;

namespace ASUL.EventSystem
{
    [System.Serializable]
    public class ASUPEvent : UnityEvent<MonoBehaviour, Action<IEnumerator>> { }

    [System.Serializable]
    public class EventStep
    {
        [Tooltip("Событие, которое будет вызвано")]
        public ASUPEvent unityEvent;

        [Tooltip("Задержка после выполнения шага (в секундах)")]
        public float delayAfter = 0f;

        [Tooltip("Вложенные шаги, которые выполнятся после этого шага")]
        public EventStep[] nestedSteps;

        public IEnumerator Execute(MonoBehaviour runner)
        {
            IEnumerator routine = null;
            unityEvent?.Invoke(runner, (ienum) => routine = ienum);

            if (routine != null)
                yield return runner.StartCoroutine(routine);

            if (nestedSteps != null)
            {
                foreach (var step in nestedSteps)
                    if (step != null)
                        yield return step.Execute(runner);
            }

            if (delayAfter > 0f)
                yield return new WaitForSeconds(delayAfter);
        }
    }
}