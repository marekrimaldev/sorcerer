using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VRTetris
{
    public class ArmDisplay : InGameDisplay
    {
        public void OnPauseBtnClick()
        {
            ButtonEvents.OnPauseGameRequested?.Invoke();
        }

        public void OnResumeBtnClick()
        {
            ButtonEvents.OnResumeGameRequested?.Invoke();
        }

        public void OnRestartBtnClick()
        {
            ButtonEvents.OnRestartGameRequested?.Invoke();
        }
    }
}
