using UnityEngine;

[CreateAssetMenu(fileName = "New Curve Evaluator", menuName = "RTS/Map/Evaluators/Curve Evaluator")]
public class CurveEvaluator : Evaluator
{
    [SerializeField]
    private AnimationCurve curve;

    public override float Evaluate(float value)
    {
        return curve.Evaluate(value);
    }
}
