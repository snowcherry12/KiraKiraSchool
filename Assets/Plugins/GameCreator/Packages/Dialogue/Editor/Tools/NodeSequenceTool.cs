using GameCreator.Editor.Common;
using GameCreator.Editor.VisualScripting;
using GameCreator.Runtime.Common;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace GameCreator.Editor.Dialogue
{
    public class NodeSequenceTool : SequenceTool
    {
        private const string USS_PATH = EditorPaths.PACKAGES + "Dialogue/Editor/StyleSheets/NodeSequence";
        
        private static readonly IIcon ICON_PREVIEW_ON = new IconPreview(ColorTheme.Type.Blue);
        private static readonly IIcon ICON_PREVIEW_OFF = new IconPreview(ColorTheme.Type.TextLight);

        private const string NAME_CONTROL_PREVIEW = "GC-Dialogue-Node-Sequence-Preview";
        
        private const string CLASS_PREVIEW = "gc-dialogue-node-sequence-preview-on";
        private const string TIP_PREVIEW = "Toggle animation preview mode";

        // MEMBERS: -------------------------------------------------------------------------------
        
        private Button m_PreviewButton;
        private Image m_PreviewIcon;

        private GameObject m_Target;
        private AnimationClip m_AnimationClip;
        
        // PROPERTIES: ----------------------------------------------------------------------------

        public override bool ShowMetric0 => false;
        public override bool ShowMetric1 => true;

        public AnimationClip AnimationClip
        {
            get => this.m_AnimationClip;
            set
            {
                this.m_AnimationClip = value;
                this.RefreshPreview();
            }
        }

        public GameObject Target
        {
            get => this.m_Target;
            set
            {
                this.m_Target = value;
                this.RefreshPreview();
            }
        }

        // CONSTRUCTOR: ---------------------------------------------------------------------------
        
        public NodeSequenceTool(SerializedProperty property) : base(property)
        {
            StyleSheet[] sheets = StyleSheetUtils.Load(USS_PATH);
            foreach (StyleSheet sheet in sheets) this.styleSheets.Add(sheet);
            
            this.RegisterCallback<DetachFromPanelEvent>(_ =>
            {
                if (AnimationMode.InAnimationMode())
                {
                    AnimationMode.StopAnimationMode();
                }
            });
            
            this.PlaybackTool.EventChange += () =>
            {
                if (!AnimationMode.InAnimationMode())
                {
                    AnimationMode.StartAnimationMode();
                }
                
                this.RefreshPreviewControl();
                this.RefreshPreview();
            };
        }

        // OVERRIDE METHODS: ----------------------------------------------------------------------

        protected override void SetupControlL(PlaybackTool playbackTool)
        {
            this.m_PreviewButton = new Button(this.TogglePreview)
            {
                name = NAME_CONTROL_PREVIEW,
                tooltip = TIP_PREVIEW,
                style = { width = this.TracksOffsetL + 1 }
            };
            
            this.m_PreviewButton.AddToClassList(CLASS_PREVIEW);

            this.m_PreviewIcon = new Image();
            
            this.m_PreviewButton.Add(this.m_PreviewIcon);
            playbackTool.Add(this.m_PreviewButton);

            this.m_PreviewButton.SetEnabled(!EditorApplication.isPlayingOrWillChangePlaymode);
            this.RefreshPreviewControl();
        }

        // PRIVATE METHODS: -----------------------------------------------------------------------

        private void RefreshPreview()
        {
            if (EditorApplication.isPlaying) return;
            if (!AnimationMode.InAnimationMode()) return;
            
            if (this.m_AnimationClip == null) return;
            if (this.m_Target == null) return;
            
            Animator animator = this.m_Target.GetComponentInChildren<Animator>();
            if (animator == null) return;

            float animationLength = this.m_AnimationClip.length;
            this.PlaybackTool.MaxFrame = this.m_AnimationClip.frameRate * animationLength;
            
            AnimationMode.BeginSampling();
            
            AnimationMode.SampleAnimationClip(
                animator.gameObject,
                this.m_AnimationClip,
                this.PlaybackTool.Value * animationLength
            );

            AnimationMode.EndSampling();
        }
        
        private void TogglePreview()
        {
            switch (AnimationMode.InAnimationMode())
            {
                case true: AnimationMode.StopAnimationMode(); break;
                case false: AnimationMode.StartAnimationMode(); break;
            }

            this.RefreshPreviewControl();
            
            if (AnimationMode.InAnimationMode())
            {
                this.RefreshPreview();
            }
        }

        private void RefreshPreviewControl()
        {
            switch (AnimationMode.InAnimationMode())
            {
                case true:
                    this.m_PreviewIcon.image = ICON_PREVIEW_ON.Texture;
                    this.m_PreviewButton.EnableInClassList(CLASS_PREVIEW, true);
                    break;
                
                case false:
                    this.m_PreviewIcon.image = ICON_PREVIEW_OFF.Texture;
                    this.m_PreviewButton.EnableInClassList(CLASS_PREVIEW, false);
                    break;
            }
        }
    }
}