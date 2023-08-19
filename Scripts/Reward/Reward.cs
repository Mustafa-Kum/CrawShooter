using System;

[Serializable]

public class Reward
{
    public int healthReward;
    public int creditReward;
    public int staminaReward;
}

public interface IRewardListener
{
    public void Reward(Reward reward);
}
