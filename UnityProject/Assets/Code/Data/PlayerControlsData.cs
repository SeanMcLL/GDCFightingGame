using UnityEngine;

[CreateAssetMenu(menuName = "Fighting Game/Player Controls")]
public class PlayerControlsData : ScriptableObject
{
    public struct FrameInputData
    {
        public Vector2 Movement;

        public bool Jump;
        public bool Attack;

        public FrameInputData Copy()
        {
            FrameInputData copy = new FrameInputData();

            copy.Movement = Movement;

            copy.Jump = Jump;
            copy.Attack = Attack;

            return copy;
        }

        public override string ToString()
        {
            return string.Format("Movement: {0}\t" +
                                 "Jump:     {1}\t" +
                                 "Attack:   {2}\t",
                                 Movement, Jump, Attack);
        }
    }

    public FrameInputData PreviousFrame;
    public FrameInputData CurrentFrame;
}
