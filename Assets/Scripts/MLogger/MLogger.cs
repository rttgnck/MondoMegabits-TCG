// MLogger
// version = '0.1.0'
// repo = 'MondoMegabits-TCG'
// Author(s): rttgnck
// Description: This is a custom logger to give each script it's own [ClassName] at the beginning of a log entry 
//
// Changelog:
//   v0.1.0  + Initial release
//
// ToDo:
//   + ...

using UnityEngine;

namespace Mondo.MLogging
{
    public class MLogger
    {
        private string loggerClassName;
        private bool turnedOn = true;

        public MLogger(string _loggerClassName, bool _turnedOn = true)
        {
            turnedOn = _turnedOn;
            loggerClassName = _loggerClassName;
        }

        public string ClassName
        {
            get { return loggerClassName; }
            set { loggerClassName = value; } 
        }

        //Is enabled by default, can be turned on individually if class logger is turned off
        //for testing a small part of a clas
        public void Log(string msg, bool oneTime = false) {
            if (turnedOn || oneTime) {
                Debug.Log($"[{loggerClassName}] {msg}");
            }
        }
    }

}

