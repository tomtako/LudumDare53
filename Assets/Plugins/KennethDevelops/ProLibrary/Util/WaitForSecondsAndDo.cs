using System;
using UnityEngine;

namespace KennethDevelops.ProLibrary.Util{
    
    public class WaitForSecondsAndDo : CustomYieldInstruction {

        private readonly Action _action;
        private readonly float  _seconds;
        private readonly float  _startTime;
        private bool   _forceStopped;


        public override bool keepWaiting{
            get {
                if (_forceStopped) return false;
                if (_startTime + _seconds <= Time.time) {
                    _action();
                    return false;
                }
                return true;
            }
        }

        public WaitForSecondsAndDo(float seconds, Action action) {
            _seconds = seconds;
            _action  = action;

            _startTime = Time.time;
        }

        public void StopAndDo(){
            _action();
            _forceStopped = true;
        }
    }
    
}