using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;

namespace ASUL.EventSystem
{
    [System.Serializable]
    public class Event : UnityEvent<MonoBehaviour, System.Action<IEnumerator>>
    {

    }

    public class EventManager : ASULStaticClass
    {
        private class EventRunner : Runner { }

        #region Запуск последовательности шагов и событий
        
        public static void LaunchStepByStep(params Func<IEnumerator>[] steps)
        {
            InitRunner<EventRunner>();
            runner.StartCoroutine(ILaunchStepByStep(steps));
        }

        private static IEnumerator ILaunchStepByStep(Func<IEnumerator>[] steps)
        {
            foreach (var step in steps)
            {
                if (step != null)
                    yield return step();
            }
        }


        public static void LaunchStepByStep(float stepDelay, params Func<IEnumerator>[] steps)
        {
            InitRunner<EventRunner>();
            runner.StartCoroutine(ILaunchStepByStepDelay(stepDelay, steps));
        }

        private static IEnumerator ILaunchStepByStepDelay(float stepDelay, Func<IEnumerator>[] steps)
        {
            foreach (var step in steps)
            {
                if (step != null)
                    yield return step();
                if (stepDelay > 0f)
                    yield return new WaitForSeconds(stepDelay);
            }
        }
        
        public static void LaunchStepByStep(params ASUPEvent[] events)
        {
            InitRunner<EventRunner>();
            runner.StartCoroutine(ILaunchStepByStep(runner, events));
        }

        private static IEnumerator ILaunchStepByStep(MonoBehaviour runner, ASUPEvent[] events)
        {
            foreach (var e in events)
            {
                IEnumerator routine = null;

                e?.Invoke(runner, (ienum) => routine = ienum);

                if (routine != null)
                    yield return runner.StartCoroutine(routine);
            }
        }
        
        public static void LaunchEvent(params EventStep[] steps)
        {
            InitRunner<EventRunner>();
            runner.StartCoroutine(RunEvent(runner, steps));
        }

        private static IEnumerator RunEvent(MonoBehaviour runner, EventStep[] steps)
        {
            foreach (var step in steps)
            {
                if (step != null)
                    yield return step.Execute(runner);
            }
        }

        #endregion
    }
}
