using System;
using System.Collections.Generic;
using System.Timers;
using UnityEditor;
namespace ExceptionSoftware.ExEditor
{

    [InitializeOnLoad]
    public static class ExTimer
    {
        static ExTimer()
        {
            EditorApplication.update -= UpdateEditor;
            EditorApplication.update += UpdateEditor;
        }

        static void UpdateEditor()
        {
            if (_actionsToMainThread.Count > 0)
            {
                while (_actionsToMainThread.Count > 0)
                {
                    _actionsToMainThread[0].TryInvoke();
                    _actionsToMainThread.RemoveAt(0);
                }
            }
        }

        #region Timer

        static List<Action> _actionsToMainThread = new List<Action>();
        static Dictionary<string, System.Timers.Timer> _timers = new Dictionary<string, System.Timers.Timer>();
        static List<ElapsedEventHandler> _eventsHandler = new List<ElapsedEventHandler>();
        static System.Timers.Timer _timer;
        static ElapsedEventHandler _handler;

        public static void CreateTimer(string key, float seconds, System.Action action)
        {
            if (!_timers.TryGetValue(key, out _timer))
            {
                _timer = new System.Timers.Timer();
                _timers.Add(key, _timer);
                _timer.AutoReset = false;
                _timer.Interval = seconds * 1000;
                _handler = delegate (object sender, ElapsedEventArgs e)
                {
                    _actionsToMainThread.Add(action);
                };

                _timer.Elapsed += _handler;
            }
            _timer.Stop();
            _timer.AutoReset = false;
            _timer.Interval = seconds * 1000;
            _timer.Start();
        }


        #endregion
    }
}
