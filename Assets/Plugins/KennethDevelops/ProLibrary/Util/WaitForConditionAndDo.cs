using System;
using UnityEngine;

namespace KennethDevelops.ProLibrary.Util{
    public class WaitForConditionAndDo : CustomYieldInstruction{
        
        private Action _action;
        private Func<bool> _condition;
        private bool   _forceStopped;


        public override bool keepWaiting{
            get {
                if (_forceStopped) return false;
                if (_condition()) {
                    _action();
                    return false;
                }
                return true;
            }
        }

        public WaitForConditionAndDo(Func<bool> condition, Action action){
            _condition = condition;
            _action  = action;
        }

        public void StopAndDo(){
            _action();
            _forceStopped = true;
        }
    }
}