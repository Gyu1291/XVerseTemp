using System.Collections.Generic;

namespace XVerse.Player.Input
{
    [System.Serializable]
    public class PlayerInput
    {
        public string InputName;
        protected bool isActiveInput;
        public void InputLock() { isActiveInput = false; }
        public void InputUnLock() { isActiveInput = true; }

        public PlayerInput() { }
    }

    [System.Serializable]
    public class PlayerInputGroup<T> where T : PlayerInput
    {
        public string InputGroupName;
        public List<T> Inputs;

        public T GetInput(string inputName)
        {
            foreach (T input in Inputs)
            {
                if (input.InputName.Equals(inputName)) return input;
            }
            return null;
        }

        public void InputUnLockOnly(string inputName)
        {
            foreach (T input in Inputs)
            {
                if (input.InputName.Equals(inputName)) input.InputUnLock();
                else input.InputLock();
            }
        }
        public void InputLockOnly(string inputName)
        {
            foreach (T input in Inputs)
            {
                if (input.InputName.Equals(inputName)) input.InputLock();
                else input.InputUnLock();
            }
        }
        public void InputLockAll()
        {
            foreach (T input in Inputs)
            {
                input.InputLock();
            }
        }
        public void InputUnLockAll()
        {
            foreach (T input in Inputs)
            {
                input.InputUnLock();
            }
        }

    }
}