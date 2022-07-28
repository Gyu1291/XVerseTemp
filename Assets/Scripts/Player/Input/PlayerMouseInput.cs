using System;
using UnityEngine;
using System.Collections.Generic;

namespace XVerse.Player.Input
{
    public enum MouseInputName
    {
        None = -1,
        Left, Right, Wheel
    }

    public enum MouseInputType
    {
        MouseDown, Mouse, MouseUp, Drag
    }

    [Serializable]
    public sealed class MouseInput : PlayerInput
    {
        public MouseInputName InputMouseName;
        public MouseInputType InputMouseType;

        public bool IsInput
        {
            get
            {
                switch (InputMouseType)
                {
                    case MouseInputType.Mouse:
                        return isActiveInput && UnityEngine.Input.GetMouseButton((int)InputMouseName);
                    case MouseInputType.MouseUp:
                        return isActiveInput && UnityEngine.Input.GetMouseButtonUp((int)InputMouseName);
                    case MouseInputType.MouseDown:
                        return isActiveInput && UnityEngine.Input.GetMouseButtonDown((int)InputMouseName);
                    case MouseInputType.Drag:
                        return isActiveInput;
                    default:
                        Debug.LogError("MouseInputType Error");
                        return false;
                }
            }
        }

        public MouseInput(string name, MouseInputName mouseName, MouseInputType mouseType)
        {
            InputName = name;
            isActiveInput = true;
            InputMouseName = mouseName;
            InputMouseType = mouseType;
        }

        public MouseInput()
        {
            isActiveInput = true;
            InputMouseName = MouseInputName.Left;
            InputMouseType = MouseInputType.Mouse;
        }

        public void SetMouseInputName(MouseInputName keyName)
        {
            if (Enum.IsDefined(typeof(KeyCode), InputMouseName.ToString()))
            {
                InputMouseName = keyName;
            }
        }
    }

    [Serializable]
    public sealed class MouseInputGroup : PlayerInputGroup<MouseInput>
    {
        public MouseInputGroup(string name) : this()
        {
            InputGroupName = name;
        }

        public MouseInputGroup(string name, List<MouseInput> mouseSet) : this(name)
        {
            Inputs = mouseSet;
        }

        public MouseInputGroup()
        {
            Inputs = new List<MouseInput>();
        }
    }
}