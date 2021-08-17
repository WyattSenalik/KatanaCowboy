using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using GameEventSystem;

public class WyattTest : MonoBehaviour
{
    private void Test()
    {
        GameEventIdentifier<UnityEngine.InputSystem.InputAction.CallbackContext> testEvent = EventIDList.Test1;
    }
}
