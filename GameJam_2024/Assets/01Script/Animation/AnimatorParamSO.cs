using UnityEngine;

[CreateAssetMenu(fileName = "AnimatorParamSO", menuName = "SO/Animator/Param")]
public class AnimatorParamSO : ScriptableObject
{
    public string paramValue;
    public int hashValue;

    private void OnValidate()
    {
        hashValue = Animator.StringToHash(paramValue);
    }
}
