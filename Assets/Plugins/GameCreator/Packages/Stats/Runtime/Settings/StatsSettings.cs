using GameCreator.Runtime.Common;
using UnityEditor;

namespace GameCreator.Runtime.Stats
{
    public class StatsSettings : AssetRepository<StatsRepository>
    {
        public override IIcon Icon => new IconTraits(ColorTheme.Type.TextLight);
        public override string Name => "Stats";
        
        #if UNITY_EDITOR

        private void OnEnable()
        {
            string[] statusEffectsGuids = AssetDatabase.FindAssets($"t:{nameof(StatusEffect)}");
            StatusEffect[] statusEffects = new StatusEffect[statusEffectsGuids.Length];

            for (int i = 0; i < statusEffectsGuids.Length; i++)
            {
                string statusEffectGuid = statusEffectsGuids[i];
                string statusEffectPath = AssetDatabase.GUIDToAssetPath(statusEffectGuid);
                statusEffects[i] = AssetDatabase.LoadAssetAtPath<StatusEffect>(statusEffectPath);
            }

            this.Get().StatusEffects.SetList(statusEffects);
        }

        #endif
    }
}