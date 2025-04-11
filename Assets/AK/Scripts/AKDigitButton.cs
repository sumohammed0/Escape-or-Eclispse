using UnityEngine;
using System;

public class AKDigitButton : MonoBehaviour
{
    public int digitValue;

    // Event to notify listeners when this button is "used"
    public event Action<int> OnButtonPressed;

    public void Trigger()
    {
        OnButtonPressed?.Invoke(digitValue);
    }
}
