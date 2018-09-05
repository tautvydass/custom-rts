using System;
using System.Collections.Generic;

public class StructureSelectionInfo
{
    public Action<WorkOrder> BeginWorkOrderCall { get; set; }
    public IReadOnlyList<WorkOrder> WorkOrders { get; set; }
    public float WorkOrderTimeElapsed { get; set; }
    public float WorkOrderDuration { get; set; }
    public bool Working { get; set; }
}
