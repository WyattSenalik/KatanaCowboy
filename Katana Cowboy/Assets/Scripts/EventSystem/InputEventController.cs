using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace GameEventSystem
{
    [RequireComponent(typeof(PlayerInput))]
    public class InputEventController : MonoBehaviour
    {
        [SerializeField] private InputGameEvent[] inputEvents = new InputGameEvent[0];
        public void SetInputEventLength(int length)
        {
            if (length != inputEvents.Length)//
            {
                Debug.Log("Do thing");
                InputGameEvent[] oldInputEvents = inputEvents.Clone() as InputGameEvent[];
                inputEvents = new InputGameEvent[length];
                for (int i = 0; i < length; ++i)
                {
                    inputEvents[i].InputEvent = oldInputEvents[i].InputEvent;
                }
            }
        }
    }
}