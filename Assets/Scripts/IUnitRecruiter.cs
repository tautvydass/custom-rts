using UnityEngine;
using System;

public interface IUnitRecruiter
{
    Vector3 TargetPosition { get; }

    void SetTargetPosition(Vector3 targetPosition);
    event Action<Unit> OnUnitRecruited;
}
