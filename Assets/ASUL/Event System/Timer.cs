using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;

namespace ASUL.EventSystem
{
    public class Timer : ASULStaticClass
    {
        private class TimerRunner : Runner { }

        #region Запуск набора действий после определённой задержки
        
        public static void WaitAndLaunchAll(float time, params Action[] actions)
        {
            InitRunner<TimerRunner>();
            runner.StartCoroutine(IWaitAndLaunchAll(time, actions));
        }

        private static IEnumerator IWaitAndLaunchAll(float time, params Action[] actions)
        {
            yield return new WaitForSeconds(time);
            foreach (var a in actions) a?.Invoke();
        }

        #endregion


        #region Запуск действий последовательно с паузой между ними

        public static void LaunchStepByStepByTime(float time, params Action[] actions)
        {
            InitRunner<TimerRunner>();
            runner.StartCoroutine(ILaunchStepByStepByTime(time, actions));
        }

        private static IEnumerator ILaunchStepByStepByTime(float time, params Action[] actions)
        {
            foreach (var a in actions) 
            {
                a?.Invoke();
                yield return new WaitForSeconds(time);
            }
        }

        #endregion
    }
}