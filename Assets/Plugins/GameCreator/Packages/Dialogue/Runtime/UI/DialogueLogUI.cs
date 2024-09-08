using GameCreator.Runtime.Common;
using GameCreator.Runtime.Common.UnityUI;
using UnityEngine;
using UnityEngine.UI;

namespace GameCreator.Runtime.Dialogue.UnityUI
{
    [Icon(EditorPaths.PACKAGES + "Dialogue/Editor/Gizmos/GizmoDialogueUI.png")]
    [AddComponentMenu("Game Creator/UI/Dialogue/Dialogue Log UI")]
    
    public class DialogueLogUI : MonoBehaviour
    {
        // EXPOSED MEMBERS: -----------------------------------------------------------------------

        [SerializeField] private GameObject m_ActiveActor;
        [SerializeField] private TextReference m_ActorName = new TextReference();
        [SerializeField] private TextReference m_ActorDescription = new TextReference();
        
        [SerializeField] private GameObject m_ActivePortrait;
        [SerializeField] private Image m_Portrait;
        
        [SerializeField] private TextReference m_Text = new TextReference();

        // PUBLIC METHODS: ------------------------------------------------------------------------
        
        public void Setup(Node node, Args args)
        {
            if (node == null) return;

            if (this.m_ActiveActor != null)
            {
                this.m_ActiveActor.gameObject.SetActive(node.Actor != null);
                
                this.m_ActorName.Text = node.Actor != null 
                    ? node.Actor.GetName(args) 
                    : string.Empty;

                this.m_ActorDescription.Text = node.Actor != null
                    ? node.Actor.GetDescription(args)
                    : string.Empty;
            }
            
            Expression expression = node.Actor != null 
                ? node.Actor.GetExpressionFromIndex(node.Expression) 
                : null;
            
            Sprite expressionSprite = expression?.GetSprite(args);

            if (this.m_ActivePortrait != null)
            {
                Portrait portrait = node.Portrait != PortraitMode.ActorDefault
                    ? (Portrait) node.Portrait
                    : node.Actor != null
                        ? node.Actor.Portrait
                        : Portrait.None;
                
                bool showPortrait = expressionSprite != null && portrait != Portrait.None;
                this.m_ActivePortrait.SetActive(showPortrait);
            }

            if (this.m_Portrait != null)
            {
                this.m_Portrait.overrideSprite = expressionSprite;
            }

            this.m_Text.Text = node.Text;
        }
    }
}