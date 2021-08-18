using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace GameEventSystem
{
    public static class InputEventIDList
    {
        /***********************************************************************************/
        public static readonly GameEventIdentifier<InputAction.CallbackContext> Aim =
            new GameEventIdentifier<InputAction.CallbackContext>("Aim");
        /***********************************************************************************/
        public static readonly GameEventIdentifier<InputAction.CallbackContext> AimLook =
            new GameEventIdentifier<InputAction.CallbackContext>("AimLook");
        /***********************************************************************************/
        public static readonly GameEventIdentifier<InputAction.CallbackContext> AimLookVertical =
            new GameEventIdentifier<InputAction.CallbackContext>("AimLookVertical");
        /***********************************************************************************/
        public static readonly GameEventIdentifier<InputAction.CallbackContext> Attack =
            new GameEventIdentifier<InputAction.CallbackContext>("Attack");
        /***********************************************************************************/
        public static readonly GameEventIdentifier<InputAction.CallbackContext> Jump =
            new GameEventIdentifier<InputAction.CallbackContext>("Jump");
        /***********************************************************************************/
        public static readonly GameEventIdentifier<InputAction.CallbackContext> Movement =
            new GameEventIdentifier<InputAction.CallbackContext>("Movement");
        /***********************************************************************************/
        public static readonly GameEventIdentifier<InputAction.CallbackContext> Sprint =
            new GameEventIdentifier<InputAction.CallbackContext>("Sprint");
        /***********************************************************************************/
        public static readonly GameEventIdentifier<InputAction.CallbackContext> Zoom =
            new GameEventIdentifier<InputAction.CallbackContext>("Zoom");
        /***********************************************************************************/
    }
}
