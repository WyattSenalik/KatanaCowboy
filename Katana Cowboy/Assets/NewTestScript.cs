using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using GameEventSystem;

public class NewTestScript : MonoBehaviour
{
  private void NewTest()
  {
    GameEventIdentifier<UnityEngine.InputSystem.InputAction.CallbackContext> testEvent = EventIDList.NewTest;
  }
}
