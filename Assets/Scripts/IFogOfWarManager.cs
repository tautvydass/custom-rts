using System.Collections.Generic;

public interface IFogOfWarManager
{
    void AddLightObject(ILighter lightObject);
    void AddLightObjects(IEnumerable<ILighter> lightObject);
}
