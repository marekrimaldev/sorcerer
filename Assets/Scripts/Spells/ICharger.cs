using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public interface ICharger
{
    public struct ChargeInfo
    {
        public float percent;
        public float time;
        public Vector3[] trajectory;

        public ChargeInfo(float chargePercent, float chargeTime, Vector3[] chargeTrajectory)
        {
            percent = chargePercent;
            time = chargeTime;
            trajectory = chargeTrajectory;
        }

        public Vector3 GetAcceleration()
        {
            return (trajectory[trajectory.Length - 1] - trajectory[0]);

            if (trajectory.Length <= 1)
                return Vector3.zero;

            Vector3 acc = Vector3.zero;
            Vector3 prev = trajectory[0];
            Vector3 curr = trajectory[1];
            Vector3 vec = curr - prev;
            Vector3 lastVec;
            for (int i = 2; i < trajectory.Length; i++)
            {
                prev = curr;
                curr = trajectory[i];

                lastVec = vec;
                vec = curr - prev;
                float verticalAngle = Vector3.SignedAngle(vec, lastVec, Vector3.right);
                float horizontalAngle = Vector3.SignedAngle(vec, lastVec, Vector3.up);

                acc += new Vector3(horizontalAngle, verticalAngle, 0);
            }

            Debug.Log("Acc = " + acc);
            return acc / trajectory.Length;
        }
    }

    public ChargeInfo GetChargeInfo();
}
