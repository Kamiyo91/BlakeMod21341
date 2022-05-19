using ModJam_Ka21341.Buffs;

namespace ModJam_Ka21341.Cards
{
    public class DiceCardSelfAbility_BlakeEgo_Ka21341 : DiceCardSelfAbilityBase
    {
        public override bool OnChooseCard(BattleUnitModel owner)
        {
            return owner.bufListDetail.GetActivatedBufList()
                .Exists(x => x is BattleUnitBuf_Bloodlust_Ka21341);
        }
    }
}