using System;

[Serializable]
public class Resources
{
	public int wood;
	public int food;
	public int stone;
	public int metal;

	public Resources(int wood, int food, int stone, int metal)
	{
		this.wood = wood;
		this.food = food;
		this.stone = stone;
		this.metal = metal;
	}

    public Resources()
    {
        this.wood = 0;
        this.food = 0;
        this.stone = 0;
        this.metal = 0;
    }

    public void AddAmount(ResourceType resourceType, int amount)
    {
        switch(resourceType)
        {
            case ResourceType.Wood: wood += amount; break;
            case ResourceType.Food: food += amount; break;
            case ResourceType.Stone: stone += amount; break;
            case ResourceType.Metal: metal += amount; break;
        }
    }

	public static bool operator >=(Resources lhs, Resources rhs)
	{
		return lhs.wood >= rhs.wood &&
			lhs.food >= rhs.food &&
			lhs.stone >= rhs.stone &&
			lhs.metal >= rhs.metal;
	}

	public static bool operator <=(Resources lhs, Resources rhs)
	{
		return lhs.wood <= rhs.wood &&
			lhs.food <= rhs.food &&
			lhs.stone <= rhs.stone &&
			lhs.metal <= rhs.metal;
	}

    public void Subtract(Resources resources)
    {
        wood -= resources.wood;
        food -= resources.food;
        stone -= resources.stone;
        metal -= resources.metal;
    }

    public void Add(Resources resources)
    {
        wood += resources.wood;
        food += resources.food;
        stone += resources.stone;
        metal += resources.metal;
    }
}