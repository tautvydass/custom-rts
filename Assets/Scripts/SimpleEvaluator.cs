using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Ridge Evaluator", menuName = "RTS/Map/Evaluators/Ridge Evaluator")]
public class SimpleEvaluator : Evaluator
{
    [SerializeField]
    public List<Vector2> values;

	public override float Evaluate(float value)
    {
        foreach(var ridge in values)
        {
            if(value < ridge.x)
            {
                return ridge.y;
            }
        }

        return 1;
    }
}
