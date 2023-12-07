using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;

namespace BuzzersXenotype
{
    public class CompAbilityEffect_Toxfeast : CompAbilityEffect
    {
        public new CompProperties_AbilityToxfeast Props => props as CompProperties_AbilityToxfeast;

        public override bool Valid(LocalTargetInfo target, bool throwMessages = false)
        {
            return base.Valid(target, throwMessages) && target.Thing != null && target.Thing.def == ThingDefOf.Wastepack;
        }

        public override void Apply(LocalTargetInfo target, LocalTargetInfo dest)
        {
            base.Apply(target, dest);

            if (Props.effecter != null)
            {
                Props.effecter.Spawn(target.Cell, target.Thing.Map).Cleanup();
            }

            Thing thing = target.Thing;
            parent.pawn.health.AddHediff(Props.hediff);
            parent.pawn.needs.food.CurLevel += Math.Min(thing.stackCount, Props.maxAmount) * Props.hungerRestore;

            thing.stackCount -= Props.maxAmount;

            if (thing.stackCount <= 0 && !thing.Destroyed)
            {
                thing.Destroy();
            }
        }
    }

    public class CompProperties_AbilityToxfeast : CompProperties_AbilityEffect
    {
        public int maxAmount = 5;
        public float hungerRestore = 0.075f; //In percent per pack
        public EffecterDef effecter;
        public HediffDef hediff;

        public CompProperties_AbilityToxfeast()
        {
            compClass = typeof(CompAbilityEffect_Toxfeast);
        }
    }
}
