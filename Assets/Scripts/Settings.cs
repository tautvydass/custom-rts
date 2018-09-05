using UnityEngine;

public static class Settings
{
	public static class Tags
	{
		public static readonly string Unit = "Unit";
		public static readonly string Structure = "Structure";
		public static readonly string Ground = "Ground";
		public static readonly string UserInterface = "UI";
        public static readonly string Harvestable = "Harvestable";
	}

	public static class DefaultResources
	{
		public static readonly int Wood = 0;
		public static readonly int Food = 500;
		public static readonly int Stone = 0;
		public static readonly int Metal = 0;
	}

	public static readonly int MaxPopulation = 200;
	public static readonly int DefaultPopulation = 1;

    public static readonly int MaxWorkerStash = 10;

    public static readonly float WorkIterationLength = 0.5f;

    private static readonly int[] TreeResources = new int[] { 300, 320, 350, 400, 550, 600 };

    public static int GetRandomWoodCapacity() => TreeResources[Random.Range(0, 5)];
}
