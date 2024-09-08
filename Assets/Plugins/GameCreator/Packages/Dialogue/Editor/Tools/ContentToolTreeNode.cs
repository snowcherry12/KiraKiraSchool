using System;
using GameCreator.Editor.Common;
using GameCreator.Runtime.Common;
using GameCreator.Runtime.Dialogue;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace GameCreator.Editor.Dialogue
{
    public class ContentToolTreeNode : VisualElement
    {
        private static readonly IIcon ICON_DEFAULT = new IconNull(ColorTheme.Type.TextLight);
        private static readonly IIcon ICON_TEXT = new IconNodeText(ColorTheme.Type.TextLight);
        private static readonly IIcon ICON_CHOICE = new IconNodeChoice(ColorTheme.Type.TextLight);
        private static readonly IIcon ICON_RANDOM = new IconNodeRandom(ColorTheme.Type.TextLight);

        private const string NAME_ICON = "GC-Dialogue-Node-Icon";
        private const string NAME_ACTOR = "GC-Dialogue-Node-Actor";
        private const string NAME_TEXT = "GC-Dialogue-Node-Text";
        private const string NAME_TAG = "GC-Dialogue-Node-Tag";

        // MEMBERS: -------------------------------------------------------------------------------

        private readonly Image m_Icon;
        private readonly Label m_Actor;
        private readonly Label m_Text;
        private readonly Label m_Tag;
        
        // PROPERTIES: ----------------------------------------------------------------------------

        private SerializedProperty PropertyData { get; set; }
        private int Id { get; set; }
        
        private ContentTool ContentTool { get; }
        
        // EVENTS: --------------------------------------------------------------------------------

        public event Action EventChangeTag;
        
        // CONSTRUCTOR: ---------------------------------------------------------------------------

        public ContentToolTreeNode(ContentTool contentTool)
        {
            this.ContentTool = contentTool;

            this.m_Icon = new Image
            {
                name = NAME_ICON,
                image = ICON_DEFAULT.Texture
            };

            this.m_Actor = new Label
            {
                name = NAME_ACTOR,
                text = string.Empty
            };
            
            this.m_Text = new Label
            {
                name = NAME_TEXT,
                text = string.Empty
            };

            this.m_Tag = new Label
            {
                name = NAME_TAG,
                text = string.Empty
            };
            
            this.Add(this.m_Icon);
            this.Add(this.m_Actor);
            this.Add(this.m_Text);
            this.Add(new FlexibleSpace());
            this.Add(this.m_Tag);
            
            ContextualMenuManipulator man = new ContextualMenuManipulator(this.OnOpenMenu);
            this.AddManipulator(man);
            
            this.RegisterCallback<MouseDownEvent>(mouseEvent =>
            {
                if (mouseEvent.clickCount != 2) return;
                this.ContentTool.Inspector.ToggleState();
            });
        }

        private void OnOpenMenu(ContextualMenuPopulateEvent menu)
        {
            menu.menu.AppendAction(
                "Delete", 
                _ =>
                {
                    Node node = this.PropertyData.GetValue<Node>();
                    if (node == null) return;

                    bool valid = this.ContentTool.Tree.Select(this.Id);
                    if (!valid) return;

                    this.ContentTool.Tree.RemoveSelection();
                });
            
            menu.menu.AppendSeparator();
            menu.menu.AppendAction(
                "Tag...", 
                _ =>
                {
                    InputDropdownText.Open("Tag", this, result =>
                    {
                        this.ContentTool.SerializedObject.Update();
                        this.PropertyData
                            .FindPropertyRelative(ContentToolInspectorNode.PROPERTY_TAG)
                            .FindPropertyRelative(IdStringDrawer.NAME_STRING).stringValue = result;
                        SerializationUtils.ApplyUnregisteredSerialization(this.ContentTool.SerializedObject);
                        this.Refresh();
                        this.EventChangeTag?.Invoke();
                    }, this.PropertyData
                        .FindPropertyRelative(ContentToolInspectorNode.PROPERTY_TAG)
                        .FindPropertyRelative(IdStringDrawer.NAME_STRING).stringValue
                    );
                });
        }

        // PUBLIC METHODS: ------------------------------------------------------------------------

        public void BindItem(SerializedProperty propertyData, int id)
        {
            this.PropertyData = propertyData;
            this.Id = id;
            
            this.Refresh();
            
            this.ContentTool.Inspector.EventChange -= this.Refresh;
            this.ContentTool.Inspector.EventChange += this.Refresh;

            this.ContentTool.Settings.EventDisplayActors -= this.Refresh;
            this.ContentTool.Settings.EventDisplayActors += this.Refresh;
            
            this.ContentTool.Settings.EventDisplayTags -= this.Refresh;
            this.ContentTool.Settings.EventDisplayTags += this.Refresh;
        }

        public void UnbindItem()
        {
            this.ContentTool.Inspector.EventChange -= this.Refresh;
            this.ContentTool.Settings.EventDisplayActors -= this.Refresh;
            this.ContentTool.Settings.EventDisplayTags -= this.Refresh;
        }
        
        // PRIVATE METHODS: -----------------------------------------------------------------------
        
        private void Refresh()
        {
            Node node = this.PropertyData.GetValue<Node>();
            if (node == null) return;

            this.m_Icon.image = node.NodeType switch
            {
                NodeTypeText => ICON_TEXT.Texture,
                NodeTypeChoice => ICON_CHOICE.Texture,
                NodeTypeRandom => ICON_RANDOM.Texture,
                _ => ICON_DEFAULT.Texture
            };

            string actor = node.Actor != null
                ? TextUtils.Humanize(node.Actor.name)
                : string.Empty;

            this.m_Actor.text = actor;

            Color backColor = ColorTheme.ColorFromHash(actor.GetHashCode());
            Color textColor = ColorTheme.Get(ColorTheme.Type.Dark);

            this.m_Actor.style.backgroundColor = backColor;
            this.m_Actor.style.color = textColor;

            bool showActor = this.ContentTool.Settings.DisplayActors && !string.IsNullOrEmpty(actor);
            this.m_Actor.style.display = showActor
                ? DisplayStyle.Flex
                : DisplayStyle.None;

            string text = node.ToString();
            this.m_Text.text = text;
            this.m_Text.style.display = !string.IsNullOrEmpty(text)
                ? DisplayStyle.Flex
                : DisplayStyle.None;
            
            string tag = node.Tag.String;
            bool showTags = this.ContentTool.Settings.DisplayTags && !string.IsNullOrEmpty(tag);
            this.m_Tag.text = tag;
            this.m_Tag.style.display = showTags 
                ? DisplayStyle.Flex
                : DisplayStyle.None;
        }
    }
}