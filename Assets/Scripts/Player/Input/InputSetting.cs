using System.Collections.Generic;
using UnityEngine;

namespace XVerse.Player.Input
{
    [CreateAssetMenu(fileName = "New InputSetting", menuName = "Player/Input Setting", order = 0)]
    [System.Serializable]
    public sealed class InputSetting : ScriptableObject
    {
        public string InputSettingName;
        public List<KeyboardInputGroup> KeyboardInputSetting;
        public List<MouseInputGroup> MouseInputSetting;

        public InputSetting()
        {
            KeyboardInputSetting = new List<KeyboardInputGroup>();
            MouseInputSetting = new List<MouseInputGroup>();
        }

        private string getString<T, U>(string name) where T : PlayerInputGroup<U> where U : PlayerInput, new()
        {
            return new U() is KeyboardInput ? "KeyboardInput" : "MouseInput";
        }

        private U GetInput<T, U>(string name, List<T> group) where T : PlayerInputGroup<U> where U : PlayerInput, new()
        {
            string str = getString<T, U>(name);

            U input = null;
            foreach (T t in group)
            {
                input = t.GetInput(name);
                if (input != null) break;
            }
            if (input == null) { Debug.LogError($"{str} with name {name} doesn't exist"); }
            return input;
        }

        private T GetInputGroup<T, U>(string name, List<T> group) where T : PlayerInputGroup<U> where U : PlayerInput, new()
        {
            string str = getString<T, U>(name);

            T input = null;
            foreach (T t in group)
            {
                if (t.InputGroupName == name) { input = t; break; }
            }
            if (input == null) { Debug.LogError($"{str} with name {name} doesn't exist"); }
            return input;
        }

        private void InputLockAll<T, U>(string name, List<T> group) where T : PlayerInputGroup<U> where U : PlayerInput, new()
        {
            GetInputGroup<T, U>(name, group).InputLockAll();
        }

        private void InputLockAll<T, U>(List<T> group) where T : PlayerInputGroup<U> where U : PlayerInput, new()
        {
            foreach (T t in group)
            {
                t.InputLockAll();
            }
        }

        private void InputUnLockAll<T, U>(string name, List<T> group) where T : PlayerInputGroup<U> where U : PlayerInput, new()
        {
            GetInputGroup<T, U>(name, group).InputUnLockAll();
        }

        private void InputUnLockAll<T, U>(List<T> group) where T : PlayerInputGroup<U> where U : PlayerInput, new()
        {
            foreach (T t in group)
            {
                t.InputUnLockAll();
            }
        }

        private void InputUnLockOnly<T, U>(string name, List<T> group, bool isGroupName = true) where T : PlayerInputGroup<U> where U : PlayerInput, new()
        {
            if (isGroupName)
            {
                foreach (T t in group)
                {
                    if (t.InputGroupName.Equals(name)) { t.InputUnLockAll(); break; }
                    else { t.InputLockAll(); }
                }
            }
            else
            {
                foreach (T t in group)
                {
                    if (t.GetInput(name) != null) { t.InputUnLockOnly(name); break; }
                    else { t.InputLockAll(); }
                }
            }
        }

        private void InputUnLockOnly<T, U>(string groupname, string inputname, List<T> group) where T : PlayerInputGroup<U> where U : PlayerInput, new()
        {
            GetInputGroup<T, U>(groupname, group).InputUnLockOnly(inputname);
        }

        private void InputUnLockOnly<T, U>(List<T> group) where T : PlayerInputGroup<U> where U : PlayerInput, new()
        {
            foreach (T t in group)
            {
                t.InputUnLockAll();
            }
        }

        private void InputLockOnly<T, U>(string name, List<T> group, bool isGroupName = true) where T : PlayerInputGroup<U> where U : PlayerInput, new()
        {
            if (isGroupName)
            {
                foreach (T t in group)
                {
                    if (t.InputGroupName.Equals(name)) { t.InputLockAll(); break; }
                    else { t.InputUnLockAll(); }
                }
            }
            else
            {
                foreach (T t in group)
                {
                    if (t.GetInput(name) != null) { t.InputLockOnly(name); break; }
                    else { t.InputUnLockAll(); }
                }
            }
        }

        private void InputLockOnly<T, U>(string groupname, string inputname, List<T> group) where T : PlayerInputGroup<U> where U : PlayerInput, new()
        {
            GetInputGroup<T, U>(groupname, group).InputLockOnly(inputname);
        }

        private void InputLockOnly<T, U>(List<T> group) where T : PlayerInputGroup<U> where U : PlayerInput, new()
        {
            foreach (T t in group)
            {
                t.InputLockAll();
            }
        }

        private KeyboardInput GetKey(string name)
        {
            return GetInput<KeyboardInputGroup, KeyboardInput>(name, KeyboardInputSetting);
        }

        private MouseInput GetMouse(string name)
        {
            return GetInput<MouseInputGroup, MouseInput>(name, MouseInputSetting);
        }

        private KeyboardInputGroup GetKeyGroup(string name)
        {
            return GetInputGroup<KeyboardInputGroup, KeyboardInput>(name, KeyboardInputSetting);
        }

        private MouseInputGroup GetMouseGroup(string name)
        {
            return GetInputGroup<MouseInputGroup, MouseInput>(name, MouseInputSetting);
        }

        /// <summary>
        /// Get KeyInputName of KeyInput with KeyInput name and KeyInputGroup name
        /// </summary>
        /// <param name="group"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public KeyboardInputName GetKeyInput(string group, string key)
        {
            if (GetKeyGroup(group) != null && GetKeyGroup(group).GetInput(key) != null) { return GetKeyGroup(group).GetInput(key).InputKeyName; }
            return KeyboardInputName.None;
        }

        /// <summary>
        /// Get MouseInputName of MouseInput with MouseInput name and MouseInputGroup name
        /// </summary>
        /// <param name="group"></param>
        /// <param name="mouse"></param>
        /// <returns></returns>
        public MouseInputName GetMouseInput(string group, string mouse)
        {
            if (GetKeyGroup(group) != null && GetKeyGroup(group).GetInput(mouse) != null) { return GetMouseGroup(group).GetInput(mouse).InputMouseName; }
            return MouseInputName.None;
        }

        /// <summary>
        /// Change KeyInputName of KeyInput with KeyInput name
        /// </summary>
        /// <param name="name"></param>
        /// <param name="input"></param>
        public void ChangeKeyInput(string name, KeyboardInputName input)
        {
            if (GetKey(name) != null) { GetKey(name).InputKeyName = input; }
        }

        /// <summary>
        /// Change KeyInputName of KeyInput with KeyInput name and KeyInputGroup name
        /// </summary>
        /// <param name="group"></param>
        /// <param name="key"></param>
        /// <param name="input"></param>
        public void ChangeKeyInput(string group, string key, KeyboardInputName input)
        {
            if (GetKeyGroup(group) != null && GetKeyGroup(group).GetInput(key) != null) { GetKeyGroup(group).GetInput(key).InputKeyName = input; }
        }

        /// <summary>
        /// Change MouseInputName of MouseInput with MouseInput name
        /// </summary>
        /// <param name="name"></param>
        /// <param name="input"></param>
        public void ChangeMouseInput(string name, MouseInputName input)
        {
            if (GetMouse(name) != null) { GetMouse(name).InputMouseName = input; }
        }

        /// <summary>
        /// Change MouseInputName of MouseInput with MouseInput name and MouseInputGroup name
        /// </summary>
        /// <param name="group"></param>
        /// <param name="mouse"></param>
        /// <param name="input"></param>
        public void ChangeMouseInput(string group, string mouse, MouseInputName input)
        {
            if (GetMouseGroup(group) != null && GetMouseGroup(group).GetInput(mouse) != null) { GetMouseGroup(group).GetInput(mouse).InputMouseName = input; }
        }

        /// <summary>
        /// return IsInput of KeyInput with KeyInput name
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public bool KeyInput(string name)
        {
            if (GetKey(name) == null) return false;
            return GetKey(name).IsInput;
        }

        /// <summary>
        /// return IsInput of KeyInput with KeyInput name and KeyInputGroup name
        /// </summary>
        /// <param name="group"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public bool KeyInput(string group, string key)
        {
            if (GetKeyGroup(group) == null || GetKeyGroup(group).GetInput(key) == null) return false;
            return GetKeyGroup(group).GetInput(key).IsInput;
        }

        /// <summary>
        /// return IsInput of MouseInput with MouseInput name
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public bool MouseInput(string name)
        {
            if (GetMouse(name) == null) return false;
            return GetMouse(name).IsInput;
        }

        /// <summary>
        /// return IsInput of MouseInput with MouseInput name and MouseInputGroup name
        /// </summary>
        /// <param name="group"></param>
        /// <param name="mouse"></param>
        /// <returns></returns>
        public bool MouseInput(string group, string mouse)
        {
            if (GetMouseGroup(group) == null || GetMouseGroup(group).GetInput(mouse) == null) return false;
            return GetMouseGroup(group).GetInput(mouse).IsInput;
        }

        /// <summary>
        /// lock all PlayerInputs
        /// </summary>
        public void InputLockAll()
        {
            InputLockAll<KeyboardInputGroup, KeyboardInput>(KeyboardInputSetting);
            InputLockAll<MouseInputGroup, MouseInput>(MouseInputSetting);
        }

        /// <summary>
        /// lock all KeyboardInputs
        /// <para>if {name} is not null, lock all KeyboardInputs of the KeyboardInputGroup with name {name}</para>
        /// </summary>
        /// <param name="name"></param>
        public void KeyInputLockAll(string name = null)
        {
            if (name == null)
            {
                InputLockAll<KeyboardInputGroup, KeyboardInput>(KeyboardInputSetting);
            }
            else
            {
                InputLockAll<KeyboardInputGroup, KeyboardInput>(name, KeyboardInputSetting);
            }
        }

        /// <summary>
        /// lock all MouseInputs
        /// <para>if {name} is not null, lock all MouseInputs of the MouseInputGroup with name {name}</para>
        /// </summary>
        /// <param name="name"></param>
        public void MouseInputLockAll(string name = null)
        {
            if (name == null)
            {
                InputLockAll<MouseInputGroup, MouseInput>(MouseInputSetting);
            }
            else
            {
                InputLockAll<MouseInputGroup, MouseInput>(name, MouseInputSetting);
            }
        }

        /// <summary>
        /// unlock all PlayerInputs
        /// </summary>
        public void InputUnLockAll()
        {
            InputUnLockAll<KeyboardInputGroup, KeyboardInput>(KeyboardInputSetting);
            InputUnLockAll<MouseInputGroup, MouseInput>(MouseInputSetting);
        }

        /// <summary>
        /// unlock all KeyboardInputs
        /// <para>if {name} is not null, unlock all KeyboardInputs of the KeyboardInputGroup with name {name}</para>
        /// </summary>
        /// <param name="name"></param>
        public void KeyInputUnLockAll(string name = null)
        {
            if (name == null)
            {
                InputUnLockAll<KeyboardInputGroup, KeyboardInput>(KeyboardInputSetting);
            }
            else
            {
                InputUnLockAll<KeyboardInputGroup, KeyboardInput>(name, KeyboardInputSetting);
            }
        }

        /// <summary>
        /// unlock all MouseInputs
        /// <para>if {name} is not null, unlock all MouseInputs of the MouseInputGroup with name {name}</para>
        /// </summary>
        /// <param name="name"></param>
        public void MouseInputUnLockAll(string name = null)
        {
            if (name == null)
            {
                InputUnLockAll<MouseInputGroup, MouseInput>(MouseInputSetting);
            }
            else
            {
                InputUnLockAll<MouseInputGroup, MouseInput>(name, MouseInputSetting);
            }
        }

        /// <summary>
        /// unlock only KeyboardInput with KeyboardInputGroup name and KeyboardInput name
        /// </summary>
        /// <param name="groupName"></param>
        /// <param name="inputName"></param>
        public void KeyInputUnLockOnly(string groupName, string inputName)
        {
            InputUnLockOnly<KeyboardInputGroup, KeyboardInput>(groupName, inputName, KeyboardInputSetting);
        }

        /// <summary>
        /// if {isGroupName} is true, unlock all KeyboardInputs with KeyboardInputGroup name
        /// <para>else, unlock only KeyboardInput with KeyboardInput name</para>
        /// </summary>
        /// <param name="name"></param>
        /// <param name="isGroupName"></param>
        public void KeyInputUnLockOnly(string name, bool isGroupName = true)
        {
            InputUnLockOnly<KeyboardInputGroup, KeyboardInput>(name, KeyboardInputSetting, isGroupName);
        }

        /// <summary>
        /// unlock only MouseInput with MouseInputGroup name and MouseInput name
        /// </summary>
        /// <param name="groupName"></param>
        /// <param name="inputName"></param>
        public void MouseInputUnLockOnly(string groupName, string inputName)
        {
            InputUnLockOnly<MouseInputGroup, MouseInput>(groupName, inputName, MouseInputSetting);
        }

        /// <summary>
        /// if {isGroupName} is true, unlock all MouseInputs with MouseInputGroup name
        /// <para>else, unlock only MouseInput with MouseInput name</para>
        /// </summary>
        /// <param name="name"></param>
        /// <param name="isGroupName"></param>
        public void MouseInputUnLockOnly(string name, bool isGroupName = true)
        {
            InputUnLockOnly<MouseInputGroup, MouseInput>(name, MouseInputSetting, isGroupName);
        }

        /// <summary>
        /// lock only KeyboardInput with KeyboardInputGroup name and KeyboardInput name
        /// </summary>
        /// <param name="groupName"></param>
        /// <param name="inputName"></param>
        public void KeyInputLockOnly(string groupName, string inputName)
        {
            InputLockOnly<KeyboardInputGroup, KeyboardInput>(groupName, inputName, KeyboardInputSetting);
        }

        /// <summary>
        /// if {isGroupName} is true, lock all KeyboardInputs with KeyboardInputGroup name
        /// <para>else, lock only KeyboardInput with KeyboardInput name</para>
        /// </summary>
        /// <param name="name"></param>
        /// <param name="isGroupName"></param>
        public void KeyInputLockOnly(string name, bool isGroupName = true)
        {
            InputLockOnly<KeyboardInputGroup, KeyboardInput>(name, KeyboardInputSetting, isGroupName);
        }

        /// <summary>
        /// lock only MouseInput with MouseInputGroup name and MouseInput name
        /// </summary>
        /// <param name="groupName"></param>
        /// <param name="inputName"></param>
        public void MouseInputLockOnly(string groupName, string inputName)
        {
            InputLockOnly<MouseInputGroup, MouseInput>(groupName, inputName, MouseInputSetting);
        }

        /// <summary>
        /// if {isGroupName} is true, lock all MouseInputs with MouseInputGroup name
        /// <para>else, lock only MouseInput with MouseInput name</para>
        /// </summary>
        /// <param name="name"></param>
        /// <param name="isGroupName"></param>
        public void MouseInputLockOnly(string name, bool isGroupName = true)
        {
            InputLockOnly<MouseInputGroup, MouseInput>(name, MouseInputSetting, isGroupName);
        }
    }
}