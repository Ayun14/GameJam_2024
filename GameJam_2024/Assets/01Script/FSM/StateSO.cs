using UnityEngine;

[CreateAssetMenu(fileName = "StateSO", menuName = "SO/FSM/StateSO")]
public class StateSO : ScriptableObject
{
    public string stateName;
    public string className;
    public AnimatorParamSO stateParam;
}
