using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Changes to apply to the parameters of an animator.
/// </summary>
[CreateAssetMenu(fileName = "AnimatorChanges", menuName = "ScriptableObjects/AnimatorChanges")]
public class AnimatorChanges : ScriptableObject
{
    [SerializeField] private List<Change<float>> floatParamChanges = new List<Change<float>>();
    [SerializeField] private List<Change<int>> intParamChanges = new List<Change<int>>();
    [SerializeField] private List<Change<bool>> boolParamChanges = new List<Change<bool>>();
    [SerializeField] private List<Change> triggerParamChanges = new List<Change>();

    
    /// <summary>
    /// Applies the changes from the scriptable object to the given animator.
    /// </summary>
    /// <param name="animator">Animator to change the parameters of.</param>
    public void ApplyToAnimator(Animator animator)
    {
        foreach (Change<float> change in floatParamChanges)
        {
            animator.SetFloat(change.Name, change.Value);
        }
        foreach (Change<int> change in intParamChanges)
        {
            animator.SetInteger(change.Name, change.Value);
        }
        foreach (Change<bool> change in boolParamChanges)
        {
            animator.SetBool(change.Name, change.Value);
        }
        foreach (Change change in triggerParamChanges)
        {
            animator.SetTrigger(change.Name);
        }
    }


    /// <summary>
    /// Single change to a given paramater.
    /// </summary>
    [Serializable]
    public class Change<T>: Change where T : unmanaged
    {
        public T Value => paramValue;
        [SerializeField] private T paramValue = default;
    }
    /// <summary>
    /// Base change with a name. Also change for a trigger.
    /// </summary>
    public class Change
    {
        public string Name => paramName;
        [SerializeField] private string paramName = "";
    }
}