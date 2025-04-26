using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace GameCreator.Runtime.Common.UnityUI
{
    [Title("Input Field")]
    [Category("UI/Input Field")]
    
    [Description("Sets the Input Field text value")]
    [Image(typeof(IconUIInputField), ColorTheme.Type.TextLight)]

    [Serializable] [HideLabelsInEditor]
    public class SetStringUIInputField : PropertyTypeSetString
    {
        [SerializeField] private PropertyGetGameObject m_InputField = GetGameObjectInstance.Create();

        public override void Set(string value, Args args)
        {
            GameObject gameObject = this.m_InputField.Get(args);
            if (gameObject == null) return;

            InputField inputField = gameObject.Get<InputField>();
            if (inputField != null)
            {
                inputField.text = value;
                return;
            }

            TMP_InputField tmpInputField = gameObject.Get<TMP_InputField>();
            if (tmpInputField != null)
            {
                tmpInputField.text = value;
                return;
            }
        }

        public override string Get(Args args)
        {
            GameObject gameObject = this.m_InputField.Get(args);
            if (gameObject == null) return default;

            string result = string.Empty;
            
            InputField inputField = gameObject.Get<InputField>();
            if (inputField != null) result = inputField.text;
            
            TMP_InputField tmpInputField = gameObject.Get<TMP_InputField>();
            if (tmpInputField != null) result = tmpInputField.text;

            return result;
        }

        public static PropertySetString Create => new PropertySetString(
            new SetStringUIInputField()
        );
        
        public override string String => this.m_InputField.ToString();
    }
}