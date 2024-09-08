using System;
using GameCreator.Runtime.Common;
using UnityEngine;

namespace GameCreator.Runtime.Dialogue
{
    [Serializable]
    public class NodeText : TPolymorphicList<NodeText.NodeTextValue>
    {
        [Serializable] 
        public class NodeTextValue : TPolymorphicItem<NodeTextValue>
        {
            [SerializeField] private PropertyGetString m_Value = GetStringEmpty.Create;
            
            [SerializeField] private bool m_InBold;
            [SerializeField] private bool m_InItalic;
            
            [SerializeField] private bool m_UseColor;
            [SerializeField] private PropertyGetColor m_Color = GetColorColorsYellow.Create;
            
            // PROPERTIES: ------------------------------------------------------------------------

            public bool InBold => this.m_InBold;
            public bool InItalic => this.m_InItalic;

            public bool UseColor => this.m_UseColor;

            // PUBLIC METHODS: --------------------------------------------------------------------

            public string GetText(Args args) => this.m_Value.Get(args);

            public Color GetColor(Args args) => this.m_Color.Get(args);

            public override string ToString() => this.m_Value.ToString();
        }
        
        // MEMBERS: -------------------------------------------------------------------------------

        [SerializeField] private PropertyGetString m_Text = GetStringTextArea.Create();
        
        [SerializeReference] private NodeTextValue[] m_Values = Array.Empty<NodeTextValue>();
        
        // PROPERTIES: ----------------------------------------------------------------------------
        
        [field: NonSerialized] public string Value { get; private set; }

        public override int Length => this.m_Values.Length;
        
        // CONSTRUCTORS: --------------------------------------------------------------------------

        public NodeText()
        { }

        public NodeText(string text) : this()
        {
            this.m_Text = GetStringTextArea.Create(text);
        }
        
        // PUBLIC METHODS: ------------------------------------------------------------------------

        public void Init(Args args)
        {
            this.Value = this.Get(args);
        }

        public string Get(Args args)
        {
            string text = this.m_Text.Get(args);

            for (int i = 0; i < this.m_Values.Length; ++i)
            {
                NodeTextValue value = this.m_Values[i];
                if (value == null) continue;
                
                string pointer = $"{{{i}}}";
                string content = value.GetText(args);

                if (value.InBold) content = $"<b>{content}</b>";
                if (value.InItalic) content = $"<i>{content}</i>";
                if (value.UseColor)
                {
                    string color = ColorUtility.ToHtmlStringRGBA(value.GetColor(args));
                    content = $"<color=#{color}>{content}</color>";
                }

                text = text.Replace(pointer, content);
            }

            Values values = DialogueRepository.Get.Values;
            foreach (Value entry in values.Get)
            {
                string pointer = $"{{{entry.Key}}}";
                string content = entry.GetText(args);
                
                if (entry.InBold) content = $"<b>{content}</b>";
                if (entry.InItalic) content = $"<i>{content}</i>";
                if (entry.UseColor)
                {
                    string color = ColorUtility.ToHtmlStringRGBA(entry.GetColor(args));
                    content = $"<color=#{color}>{content}</color>";
                }

                text = text.Replace(pointer, content);
            }

            return text;
        }
        
        // STRING: --------------------------------------------------------------------------------

        public override string ToString() => this.m_Text.ToString();
    }
}