

public static class RewardManager
{
	public static float CurrentReward { get; set; }

	// TODO: Check if pacman is colliding with a ghost
	// If not then ask the worldStaticEntity Grid  we stored what pacman is on
	public static event System.Action<Coordinates> CalculateReward;

}
