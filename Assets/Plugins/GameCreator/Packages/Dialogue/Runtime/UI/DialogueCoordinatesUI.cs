using System;
using GameCreator.Runtime.Characters;
using GameCreator.Runtime.Common;
using UnityEngine;

namespace GameCreator.Runtime.Dialogue.UnityUI
{
    [Icon(EditorPaths.PACKAGES + "Dialogue/Editor/Gizmos/GizmoDialogueUI.png")]
    [AddComponentMenu("Game Creator/UI/Dialogue/Coordinates UI")]
    
    public class DialogueCoordinatesUI : TDialogueUnitUI
    {
        private enum AlignX
        {
            Left, Center, Right
        }
        
        private enum AlignY
        {
            Top, Middle, Bottom
        }

        private enum Anchor
        {
            Actor,
            GameObject,
        }
        
        // EXPOSED MEMBERS: -----------------------------------------------------------------------

        [SerializeField] private AlignY m_DefaultVertical = AlignY.Top;
        [SerializeField] private AlignX m_DefaultHorizontal = AlignX.Center;
        
        [SerializeField] private Vector2 m_Offset = Vector2.zero;
        [SerializeField] private bool m_KeepInParent = true;

        [SerializeField] private Anchor m_AnchorTo = Anchor.Actor;
        [SerializeField] private PropertyGetGameObject m_Anchor = GetGameObjectPlayer.Create();

        // MEMBERS: -------------------------------------------------------------------------------

        [NonSerialized] private Canvas m_Canvas;
        [NonSerialized] private RectTransform m_RectTransform;
        
        [NonSerialized] private readonly Vector3[] m_ParentCorners = new Vector3[4];
        
        [NonSerialized] private GameObject m_Target;

        // OVERRIDE METHODS: ----------------------------------------------------------------------

        public override void OnAwake(DialogueUI dialogueUI)
        {
            base.OnAwake(dialogueUI);
            
            this.m_Canvas = this.GetComponentInParent<Canvas>();
            this.m_RectTransform = this.GetComponent<RectTransform>();
        }

        public override void OnReset(bool isNew)
        { }

        public override void OnStartNext(Story story, int nodeId, Args args)
        {
            if (ApplicationManager.IsExiting) return;
            
            Node node = story.Content.Get(nodeId);
            if (node == null) return;

            this.m_Target = this.m_AnchorTo switch
            {
                Anchor.GameObject => this.m_Anchor.Get(args),
                Anchor.Actor => story.Content.GetTargetFromActor(node.Actor, args),
                _ => throw new ArgumentOutOfRangeException()
            };

            this.LateUpdate();
        }
        
        public override void OnFinishNext(Story story, int nodeId, Args args)
        {
            if (ApplicationManager.IsExiting) return;
            
            this.m_Target = null;
        }

        // UPDATE METHODS: ------------------------------------------------------------------------

        private void LateUpdate()
        {
            RectTransform panel = this.m_RectTransform.childCount > 0
                ? this.m_RectTransform.GetChild(0) as RectTransform
                : null;
            
            if (panel == null) return;

            Camera uiCamera = this.m_Canvas.worldCamera != null
                ? this.m_Canvas.worldCamera
                : ShortcutMainCamera.Get<Camera>();
            
            if (uiCamera == null) return;
            
            RectTransform canvasTransform = this.m_Canvas.Get<RectTransform>();
            Vector2 viewPosition = this.GetViewPosition(uiCamera);

            viewPosition = new Vector2(
                Mathf.Lerp(-1f, 1f, viewPosition.x),
                Mathf.Lerp(-1f, 1f, viewPosition.y)
            );

            Vector2 position = new Vector2(
                viewPosition.x * canvasTransform.rect.width * 0.5f,
                viewPosition.y * canvasTransform.rect.height * 0.5f
            );

            position += this.m_Offset;
            panel.localPosition = position;

            if (this.m_KeepInParent)
            {
                panel.anchorMin = Vector2.zero;
                panel.anchorMax = Vector2.zero;
                
                this.m_RectTransform.GetLocalCorners(this.m_ParentCorners);

                float offsetX = 0f;
                float offsetY = 0f;

                Bounds tooltipBounds = RectTransformUtility.CalculateRelativeRectTransformBounds(
                    this.m_RectTransform,
                    panel
                );

                Vector3 parentCornerTopLeft = this.m_ParentCorners[1];
                Vector3 parentCornerBottomRight = this.m_ParentCorners[3];
                
                if (tooltipBounds.min.x < parentCornerTopLeft.x)
                {
                    offsetX = parentCornerTopLeft.x - tooltipBounds.min.x;
                }
                
                if (tooltipBounds.min.y < parentCornerBottomRight.y)
                {
                    offsetY = parentCornerBottomRight.y - tooltipBounds.min.y;
                }

                if (tooltipBounds.max.x > parentCornerBottomRight.x)
                {
                    offsetX = parentCornerBottomRight.x - tooltipBounds.max.x;
                }

                if (tooltipBounds.max.y > parentCornerTopLeft.y)
                {
                    offsetY = parentCornerTopLeft.y - tooltipBounds.max.y;
                }

                panel.anchoredPosition += new Vector2(offsetX, offsetY);
            }
        }

        private Vector2 GetViewPosition(Camera uiCamera)
        {
            if (this.m_Target == null)
            {
                return new Vector2(
                    this.m_DefaultHorizontal switch
                    {
                        AlignX.Left => 0f,
                        AlignX.Center => 0.5f,
                        AlignX.Right => 1f,
                        _ => throw new ArgumentOutOfRangeException()
                    },
                    this.m_DefaultVertical switch
                    {
                        AlignY.Top => 1f,
                        AlignY.Middle => 0.5f,
                        AlignY.Bottom => 0f,
                        _ => throw new ArgumentOutOfRangeException()
                    }
                );
            }
            
            return uiCamera.WorldToViewportPoint(this.m_Target.transform.position);
        }
    }
}