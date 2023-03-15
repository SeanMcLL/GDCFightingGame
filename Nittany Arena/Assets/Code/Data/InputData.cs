using UnityEngine;

[CreateAssetMenu(fileName = "New Input Data", menuName = "Fighting Game/Input Data")]
public class InputData : ScriptableObject
{
    public struct FrameInputData
    {
        public Vector2 StickLeft, StickRight;
        public bool ButtonNorth, ButtonEast, ButtonSouth, ButtonWest;
        public bool LeftShoulder, RightShoulder, LeftTrigger, RightTrigger;
        public bool Start;

        public FrameInputData Copy()
        {
            FrameInputData copy = new FrameInputData();

            copy.StickLeft = StickLeft;
            copy.StickRight = StickRight;
            copy.ButtonNorth = ButtonNorth;
            copy.ButtonEast = ButtonEast;
            copy.ButtonSouth = ButtonSouth;
            copy.ButtonWest = ButtonWest;
            copy.LeftShoulder = LeftShoulder;
            copy.RightShoulder = RightShoulder;
            copy.LeftTrigger = LeftTrigger;
            copy.RightTrigger = RightTrigger;
            copy.Start = Start;

            return copy;
        }
    }

    public FrameInputData PreviousFrame;
    public FrameInputData CurrentFrame;
}
