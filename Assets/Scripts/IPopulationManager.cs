using System;

public interface IPopulationManager
{
    int GetAvailablePopulation();
    bool TryRecruit(RecruitmentWorkOrder recruitmentWorkOrder);
    void Cancel(RecruitmentWorkOrder recruitmentWorkOrder);
    event Action<Population> OnPopulationUpdated;
}
