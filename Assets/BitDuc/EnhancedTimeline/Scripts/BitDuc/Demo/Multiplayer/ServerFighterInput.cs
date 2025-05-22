#if MIRROR
using System;
using Mirror;
using UnityEngine;

namespace BitDuc.Demo.Multiplayer
{
    public class ServerFighterInput: NetworkBehaviour, FighterInput
    {
        public bool Left => CheckInput(InputCommand.Left);
        public bool Right => CheckInput(InputCommand.Right);
        public bool Attack => CheckInputDown(InputCommand.Attack);

        [Flags]
        enum InputCommand
        {
            None = 0,
            Left = 1,
            Right = 2,
            Attack = 4,
        }

        InputCommand previousInput = InputCommand.None;
        InputCommand synchronizedInput;

        void OnGUI()
        {
            if (isClient)
                GUILayout.Label($"Attack: {previousInput & InputCommand.Attack}");

            if (isServer)
                GUILayout.Label($"Attack: {synchronizedInput & InputCommand.Attack}");
        }

        void Update()
        {
            if (isServer && !isClient)
                return;

            var input =
                GetInput(KeyCode.LeftArrow, InputCommand.Left) | 
                GetInput(KeyCode.RightArrow, InputCommand.Right) |
                GetInput(KeyCode.Z, InputCommand.Attack);

            if (previousInput == input)
                return;

            SendInput(input);
            previousInput = input;
        }

        [Command]
        void SendInput(InputCommand input)
        {
            previousInput = synchronizedInput;
            synchronizedInput = input;
        }

        static InputCommand GetInput(KeyCode code, InputCommand command) =>
            Input.GetKey(code) ? command : InputCommand.None;

        bool CheckInput(InputCommand toCheck) =>
            (synchronizedInput & toCheck) > 0;

        bool CheckInputDown(InputCommand toCheck) =>
            (previousInput & toCheck) == 0 && CheckInput(toCheck);
    }
}
#endif
