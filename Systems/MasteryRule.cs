using Terraria.GameContent.ItemDropRules;

namespace PetsOverhaul.Systems
{
    public class FirstKillEoC : IItemDropRuleCondition, IProvideItemConditionDescription
    {
        public bool CanDrop(DropAttemptInfo info)
        {
            return !MasteryShardCheck.masteryShardObtained1;
        }

        public bool CanShowItemDropInUI()
        {
            return true;
        }

        public string GetConditionDescription()
        {
            return null;
        }
    }
    public class FirstKillWoF : IItemDropRuleCondition, IProvideItemConditionDescription
    {
        public bool CanDrop(DropAttemptInfo info)
        {
            return !MasteryShardCheck.masteryShardObtained2;
        }

        public bool CanShowItemDropInUI()
        {
            return true;
        }

        public string GetConditionDescription()
        {
            return null;
        }
    }
    public class FirstKillGolem : IItemDropRuleCondition, IProvideItemConditionDescription
    {
        public bool CanDrop(DropAttemptInfo info)
        {
            return !MasteryShardCheck.masteryShardObtained3;
        }

        public bool CanShowItemDropInUI()
        {
            return true;
        }

        public string GetConditionDescription()
        {
            return null;
        }
    }
    public class FirstKillSkeletron : IItemDropRuleCondition, IProvideItemConditionDescription
    {
        public bool CanDrop(DropAttemptInfo info)
        {
            return !MasteryShardCheck.masteryShardObtained4;
        }

        public bool CanShowItemDropInUI()
        {
            return true;
        }

        public string GetConditionDescription()
        {
            return null;
        }
    }
    public class FirstKillMoonLord : IItemDropRuleCondition, IProvideItemConditionDescription
    {
        public bool CanDrop(DropAttemptInfo info)
        {
            return !MasteryShardCheck.masteryShardObtained5;
        }

        public bool CanShowItemDropInUI()
        {
            return true;
        }

        public string GetConditionDescription()
        {
            return null;
        }
    }
}
