using GameCreator.Editor.Common;
using GameCreator.Runtime.Common;
using GameCreator.Runtime.Inventory;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine.UIElements;

namespace GameCreator.Editor.Inventory
{
    [CustomEditor(typeof(Item))]
    public class ItemEditor : UnityEditor.Editor
    {
        private const string USS_PATH = EditorPaths.PACKAGES + "Inventory/Editor/StyleSheets/Item";

        private const string ERR_DUPLICATE_ID = "Another item has the same Item ID as this one";
        
        // MEMBERS: -------------------------------------------------------------------------------

        private VisualElement m_Root;
        private VisualElement m_Head;
        private VisualElement m_Body;
        private VisualElement m_Foot;

        private VisualElement m_ContentMsgID;
        
        private VisualElement m_ParentChain;
        private SerializedProperty m_Parent;
        
        private PropertyField m_FieldID;
        private PropertyField m_FieldParent;
        private PropertyField m_FieldPrefab;
        private PropertyField m_FieldCanDrop;
        
        private PropertyField m_FieldInfo;
        private PropertyField m_FieldShape;
        private PropertyField m_FieldPrice;
        
        private PropertyField m_FieldProperties;
        private PropertyField m_FieldSockets;
        private PropertyField m_FieldEquip;
        private PropertyField m_FieldUsage;
        private PropertyField m_FieldCrafting;

        // PAINT METHODS: -------------------------------------------------------------------------
        
        public override VisualElement CreateInspectorGUI()
        {
            this.m_Root = new VisualElement();
            this.m_Head = new VisualElement();
            this.m_Body = new VisualElement();
            this.m_Foot = new VisualElement();
            
            this.m_Root.Add(this.m_Head);
            this.m_Root.Add(this.m_Body);
            this.m_Root.Add(this.m_Foot);

            StyleSheet[] sheets = StyleSheetUtils.Load(USS_PATH);
            foreach (StyleSheet sheet in sheets) this.m_Root.styleSheets.Add(sheet);

            this.CreateHead();
            this.CreateBody();
            this.CreateFoot();
            
            this.m_FieldSockets.RegisterValueChangeCallback(_ =>
            {
                this.m_Body.Clear();
                this.CreateBody();
            });
            
            this.m_FieldParent.RegisterValueChangeCallback(_ =>
            {
                this.RefreshParentChain();
                
                this.m_Body.Clear();
                this.m_Foot.Clear();
                
                this.CreateBody();
                this.CreateFoot();
            });

            return this.m_Root;
        }

        private void CreateHead()
        {
            SerializedProperty id = this.serializedObject.FindProperty("m_ID");
            this.m_Parent = this.serializedObject.FindProperty("m_Parent");
            
            SerializedProperty prefab = this.serializedObject.FindProperty("m_Prefab");
            SerializedProperty canDrop = this.serializedObject.FindProperty("m_CanDrop");

            this.m_ContentMsgID = new VisualElement();
            this.m_FieldID = new PropertyField(id);
            this.m_ParentChain = new VisualElement();
            this.m_FieldParent = new PropertyField(this.m_Parent);
            this.m_FieldPrefab = new PropertyField(prefab);
            this.m_FieldCanDrop = new PropertyField(canDrop);
            
            this.m_Head.Add(new SpaceSmall());
            this.m_Head.Add(this.m_ContentMsgID);
            this.m_Head.Add(this.m_FieldID);
            this.m_Head.Add(new SpaceSmall());
            this.m_Head.Add(this.m_ParentChain);
            this.m_Head.Add(this.m_FieldParent);

            this.m_Head.Add(new SpaceSmall());
            this.m_Head.Add(this.m_FieldPrefab);
            this.m_Head.Add(this.m_FieldCanDrop);

            this.m_FieldID.Bind(this.serializedObject);
            this.m_FieldParent.Bind(this.serializedObject);
            this.m_FieldPrefab.Bind(this.serializedObject);
            
            this.RefreshErrorID();
            this.m_FieldID.RegisterValueChangeCallback(_ =>
            {
                this.RefreshErrorID();
            });
        }

        private void CreateBody()
        {
            SerializedProperty info = this.serializedObject.FindProperty("m_Info");
            SerializedProperty shape = this.serializedObject.FindProperty("m_Shape");
            SerializedProperty price = this.serializedObject.FindProperty("m_Price");

            this.m_FieldInfo = new PropertyField(info);
            this.m_FieldShape = new PropertyField(shape);
            this.m_FieldPrice = new PropertyField(price);
            
            this.m_Body.Add(new SpaceSmall());
            this.m_Body.Add(this.m_FieldInfo);
            this.m_Body.Add(this.m_FieldShape);
            this.m_Body.Add(this.m_FieldPrice);
            
            this.m_FieldInfo.Bind(this.serializedObject);
            this.m_FieldShape.Bind(this.serializedObject);
            this.m_FieldPrice.Bind(this.serializedObject);
        }
        
        private void CreateFoot()
        {
            SerializedProperty properties = this.serializedObject.FindProperty("m_Properties");
            SerializedProperty sockets = this.serializedObject.FindProperty("m_Sockets");
            SerializedProperty equip = this.serializedObject.FindProperty("m_Equip");
            SerializedProperty usage = this.serializedObject.FindProperty("m_Usage");
            SerializedProperty crafting = this.serializedObject.FindProperty("m_Crafting");

            this.m_FieldProperties = new PropertyField(properties);
            this.m_FieldSockets = new PropertyField(sockets);
            this.m_FieldEquip = new PropertyField(equip);
            this.m_FieldUsage = new PropertyField(usage);
            this.m_FieldCrafting = new PropertyField(crafting);
            
            this.m_Foot.Add(this.m_FieldProperties);
            this.m_Foot.Add(this.m_FieldSockets);
            this.m_Foot.Add(this.m_FieldEquip);
            this.m_Foot.Add(this.m_FieldUsage);
            this.m_Foot.Add(this.m_FieldCrafting);
            
            this.m_FieldProperties.Bind(this.serializedObject);
            this.m_FieldSockets.Bind(this.serializedObject);
            this.m_FieldEquip.Bind(this.serializedObject);
            this.m_FieldUsage.Bind(this.serializedObject);
            this.m_FieldCrafting.Bind(this.serializedObject);
        }
        
        // PRIVATE METHODS: -----------------------------------------------------------------------
        
        private void RefreshErrorID()
        {
            this.serializedObject.Update();
            this.m_ContentMsgID.Clear();

            SerializedProperty id = this.serializedObject.FindProperty("m_ID");
            
            string itemID = id
                .FindPropertyRelative(UniqueIDDrawer.SERIALIZED_ID)
                .FindPropertyRelative(IdStringDrawer.NAME_STRING)
                .stringValue;

            string[] guids = AssetDatabase.FindAssets($"t:{nameof(Item)}");
            foreach (string guid in guids)
            {
                string path = AssetDatabase.GUIDToAssetPath(guid);
                Item item = AssetDatabase.LoadAssetAtPath<Item>(path);
                
                if (item == null) continue;
                if (this.target == null) continue;

                if (item.ID.String == itemID && item != this.target)
                {
                    ErrorMessage error = new ErrorMessage(ERR_DUPLICATE_ID);
                    this.m_ContentMsgID.Add(error);
                    return;
                }
            }
        }
        
        private void RefreshParentChain()
        {
            this.m_ParentChain.Clear();
            Item parent = this.m_Parent.objectReferenceValue as Item;
            if (parent == null) return;

            while (parent.Parent != null)
            {
                ObjectField parentField = new ObjectField
                {
                    label = " ",
                    objectType = typeof(Item),
                    allowSceneObjects = false,
                    value = parent.Parent
                };
                
                parentField.SetEnabled(false);
                AlignLabel.On(parentField);
                
                this.m_ParentChain.Insert(0, parentField);
                
                parent = parent.Parent;
            }
        }
    }
}