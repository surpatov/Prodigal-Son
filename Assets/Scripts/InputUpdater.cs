// InputUpdater.cs Ч единственное место
using UnityEngine;

[DefaultExecutionOrder(-100)]
public class InputUpdater : MonoBehaviour
{
    private void Update() => ControllerInput.Update();
}