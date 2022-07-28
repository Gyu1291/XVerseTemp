using System;
using UnityEngine;
using System.Collections.Generic;

namespace XVerse.Player.Input
{
    public enum KeyboardInputName
    {
        None = -1,
        BackQuote, Alpha1, Alpha2, Alpha3, Alpha4, Alpha5,
        Alpha6, Alpha7, Alpha8, Alpha9, Alpha0, Minus, Plus, Backspace,
        Tab, Q, W, E, R, T, Y, U, I, O, P, LeftBracket, RightBracket, Backslash,
        CapsLock, A, S, D, F, G, H, J, K, L, Semicolon, Quote, Return,
        LeftShift, Z, X, C, V, B, N, M, Comma, Period, Slash, RightShift,
        LeftControl, LeftCommand, Space, UpArrow, LeftArrow, DownArrow, RightArrow
    };

    public enum KeyboardInputType
    {
        KeyDown, Key, KeyUp
    }

    [Serializable]
    public sealed class KeyboardInput : PlayerInput
    {
        public KeyboardInputName InputKeyName;
        public KeyboardInputType InputKeyType;

        public bool IsInput
        {
            get
            {
                switch (InputKeyType)
                {
                    case KeyboardInputType.Key:
                        return isActiveInput && UnityEngine.Input.GetKey(ToKeyCode());
                    case KeyboardInputType.KeyDown:
                        return isActiveInput && UnityEngine.Input.GetKeyDown(ToKeyCode());
                    case KeyboardInputType.KeyUp:
                        return isActiveInput && UnityEngine.Input.GetKeyUp(ToKeyCode());
                    default:
                        Debug.LogError("KeyboardInputType Error");
                        return false;
                }
            }
        }

        public KeyboardInput(string name, KeyboardInputName keyName, KeyboardInputType keyType)
        {
            InputName = name;
            isActiveInput = true;
            if (Enum.IsDefined(typeof(KeyCode), InputKeyName.ToString()))
            {
                InputKeyName = keyName;
                InputKeyType = keyType;
            }
        }

        public KeyboardInput()
        {
            isActiveInput = true;
            InputKeyName = KeyboardInputName.W;
            InputKeyType = KeyboardInputType.Key;
        }

        public KeyCode ToKeyCode()
        {
            return (KeyCode)Enum.Parse(typeof(KeyCode), InputKeyName.ToString());
        }

        public void SetKeyboardInputName(KeyboardInputName keyName)
        {
            if (Enum.IsDefined(typeof(KeyCode), InputKeyName.ToString()))
            {
                InputKeyName = keyName;
            }
        }
    }

    [Serializable]
    public sealed class KeyboardInputGroup : PlayerInputGroup<KeyboardInput>
    {
        public KeyboardInputGroup(string name) : this()
        {
            InputGroupName = name;
        }

        public KeyboardInputGroup(string name, List<KeyboardInput> keySet) : this(name)
        {
            Inputs = keySet;
        }

        public KeyboardInputGroup()
        {
            Inputs = new List<KeyboardInput>();
        }
    }

}