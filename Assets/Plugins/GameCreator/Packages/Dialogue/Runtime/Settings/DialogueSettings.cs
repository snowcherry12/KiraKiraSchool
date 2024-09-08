using GameCreator.Runtime.Common;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace GameCreator.Runtime.Dialogue
{
    public class DialogueSettings : AssetRepository<DialogueRepository>
    {
        public override IIcon Icon => new IconNodeText(ColorTheme.Type.TextLight);
        public override string Name => "Dialogue";
    }
}