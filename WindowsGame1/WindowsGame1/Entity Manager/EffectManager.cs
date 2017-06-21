using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WindowsGame1.Entity_Manager
{
    class EffectManager
    {
        public Dictionary<short, CharacterEffect> effects;
        public Dictionary<short, Abilities.AbilityBase> abilities;
        public List<string> messages;

        public EffectManager (
            DataLoader.EffectLoader[] _effects,
            DataLoader.ExtendedEffectLoader[] _extendedEffects,
            Abilities.AbilityBase[] _abilities
            )
        {
            short arrayIterator = 0;
            messages = new List<string>();
            effects = new Dictionary<short, CharacterEffect>();
            abilities = new Dictionary<short, Abilities.AbilityBase>(); 

            for (arrayIterator = 0; arrayIterator < _effects.Length; arrayIterator++)
            {
                effects.Add( _effects[arrayIterator].effectID, getEffectData(_effects[arrayIterator] ));
            }

            messages.Add("Loaded " + _effects.Length + " basic effects.");

            if (_extendedEffects != null)
            {

                for (arrayIterator = 0; arrayIterator < _extendedEffects.Length; arrayIterator++)
                {
                    effects.Add(_extendedEffects[arrayIterator].effectID, getEffectData(_extendedEffects[arrayIterator]));
                }

                messages.Add("Loaded " + _extendedEffects.Length + " extended effects");
            }
            else messages.Add("Did not load any extended effects (null parameter)");

            //for (short arrayIterator = 0; arrayIterator < _effects.Length; arrayIterator++)
            //{
            //    abilities.Add(_abilities[arrayIterator], _effect);
            //}
        }
        public List<string> takeMessages()
        {
            List<string> _messages = messages;
            messages.RemoveRange(0, messages.Count);
            return _messages;
        }

        private CharacterEffect getEffectData(DataLoader.EffectLoader _effect)
        {
            CharacterEffect tempEffect = new CharacterEffect();
            tempEffect.effectID = _effect.effectID;
            tempEffect.name = _effect.effectName;
			tempEffect.description = _effect.description;
			tempEffect.spriteSource = _effect.spriteSource;
            tempEffect.permanent = _effect.permanent;
            tempEffect.stacks = _effect.stacks;
            tempEffect.consumable = _effect.consumable;
			tempEffect.stacksPerUse = _effect.stacksPerUse;
            tempEffect.maxStacks = _effect.maxStacks;
            tempEffect.durationMS = _effect.durationMS;
            tempEffect.tickSpeed = _effect.tickSpeed;
            tempEffect.tickTimer = 0;
            tempEffect.ticks = 0;

            tempEffect.hpIncrease = _effect.hpIncrease;
            tempEffect.hpMaxIncrease = _effect.hpMaxIncrease;
            tempEffect.hpRegenPer5increase = _effect.hpRegenPer5increase;
            tempEffect.manaIncrease = _effect.manaIncrease;
            tempEffect.manaMaxIncrease = _effect.manaMaxIncrease;
            tempEffect.manaPer5Increase = _effect.manaPer5Increase;

            tempEffect.attackPowerIncrease = _effect.attackPowerIncrease;
            tempEffect.healPowerIncrease = _effect.healPowerIncrease;
            tempEffect.magicPowerIncrease = _effect.magicPowerIncrease;
            tempEffect.defenseIncrease = _effect.defenseIncrease;
            tempEffect.magicDefenseIncrease = _effect.magicDefenseIncrease;

            return tempEffect;
        }

        private EffectExtended getEffectData(DataLoader.ExtendedEffectLoader _effect)
        {
            EffectExtended tempEffect = new EffectExtended();
            tempEffect = (EffectExtended)(getEffectData((DataLoader.EffectLoader)_effect));

            tempEffect.hpPercentIncrease = _effect.hpPercentIncrease;
            tempEffect.hpMaxPercentIncrease = _effect.hpMaxPercentIncrease;
            tempEffect.hpRegenPer5PercentIncrease = _effect.hpRegenPer5PercentIncrease;

            tempEffect.manaPercentIncrease = _effect.manaPercentIncrease;
            tempEffect.manaMaxPercentIncrease = _effect.manaMaxPercentIncrease;
            tempEffect.manaPer5PercentIncrease = _effect.manaPer5PercentIncrease;

            tempEffect.attackPowerPercentIncrease = _effect.attackPowerPercentIncrease;
            tempEffect.healPowerIncrease = _effect.healPowerIncrease;
            tempEffect.magicPowerPercentIncrease = _effect.magicPowerPercentIncrease;

            tempEffect.defensePercentIncrease = _effect.defensePercentIncrease;
            tempEffect.evadeChancePercentIncrease = _effect.evadeChancePercentIncrease;
            tempEffect.magicDefensePercentIncrease = _effect.magicDefensePercentIncrease;

            return tempEffect;
        }

        public CharacterEffect getEffect(short _effectID)
        {
            CharacterEffect effect = effects[_effectID];
            return effect;
        }
    }
}
