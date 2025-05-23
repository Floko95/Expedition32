using UnityEngine;

namespace BitDuc.Demo.SinglePlayer
{
    public class LocalFighterInput: MonoBehaviour, FighterInput
    {
        enum Side { Left, Right }

        [SerializeField] Side side;

        public bool Left => Input.GetKey(side == Side.Left ? KeyCode.A : KeyCode.LeftArrow);
        public bool Right => Input.GetKey(side == Side.Left ? KeyCode.D : KeyCode.RightArrow);
        public bool Attack => Input.GetKey(side == Side.Left ? KeyCode.G : KeyCode.K);
    }
}
