using System;
using GameCreator.Editor.Common;
using GameCreator.Runtime.Common;
using GameCreator.Runtime.Melee;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace GameCreator.Editor.Melee
{
    public class CombosToolTreeNode : VisualElement
    {
        private static readonly Color COLOR_A = ColorUtils.Parse("c2f771");
        private static readonly Color COLOR_B = ColorUtils.Parse("87d8f6");
        private static readonly Color COLOR_C = ColorUtils.Parse("e9754c");
        private static readonly Color COLOR_D = ColorUtils.Parse("f1c437");
        private static readonly Color COLOR_E = ColorUtils.Parse("a692e9");
        private static readonly Color COLOR_F = ColorUtils.Parse("f7b171");
        private static readonly Color COLOR_G = ColorUtils.Parse("a2f7e4");
        private static readonly Color COLOR_H = ColorUtils.Parse("d790d4");

        private static readonly IIcon ICON_ROOT_INORDER = new IconMeleeSkillInOrder(ColorTheme.Type.Green);
        private static readonly IIcon ICON_ROOT_ANYTIME = new IconMeleeSkillAnytime(ColorTheme.Type.Green);
        
        private static readonly IIcon ICON_ROOT_HISTORY_INORDER = new IconMeleeSkillInOrder(ColorTheme.Type.TextLight);
        private static readonly IIcon ICON_ROOT_HISTORY_ANYTIME = new IconMeleeSkillAnytime(ColorTheme.Type.TextLight);
        
        private static readonly IIcon ICON_A_TAP_HISTORY = new IconMeleeTapA(ColorTheme.Type.TextLight);
        private static readonly IIcon ICON_B_TAP_HISTORY = new IconMeleeTapB(ColorTheme.Type.TextLight);
        private static readonly IIcon ICON_C_TAP_HISTORY = new IconMeleeTapC(ColorTheme.Type.TextLight);
        private static readonly IIcon ICON_D_TAP_HISTORY = new IconMeleeTapD(ColorTheme.Type.TextLight);
        private static readonly IIcon ICON_E_TAP_HISTORY = new IconMeleeTapE(ColorTheme.Type.TextLight);
        private static readonly IIcon ICON_F_TAP_HISTORY = new IconMeleeTapF(ColorTheme.Type.TextLight);
        private static readonly IIcon ICON_G_TAP_HISTORY = new IconMeleeTapG(ColorTheme.Type.TextLight);
        private static readonly IIcon ICON_H_TAP_HISTORY = new IconMeleeTapH(ColorTheme.Type.TextLight);
        
        private static readonly IIcon ICON_A_CHARGE_HISTORY = new IconMeleeChargeA(ColorTheme.Type.TextLight);
        private static readonly IIcon ICON_B_CHARGE_HISTORY = new IconMeleeChargeB(ColorTheme.Type.TextLight);
        private static readonly IIcon ICON_C_CHARGE_HISTORY = new IconMeleeChargeC(ColorTheme.Type.TextLight);
        private static readonly IIcon ICON_D_CHARGE_HISTORY = new IconMeleeChargeD(ColorTheme.Type.TextLight);
        private static readonly IIcon ICON_E_CHARGE_HISTORY = new IconMeleeChargeE(ColorTheme.Type.TextLight);
        private static readonly IIcon ICON_F_CHARGE_HISTORY = new IconMeleeChargeF(ColorTheme.Type.TextLight);
        private static readonly IIcon ICON_G_CHARGE_HISTORY = new IconMeleeChargeG(ColorTheme.Type.TextLight);
        private static readonly IIcon ICON_H_CHARGE_HISTORY = new IconMeleeChargeH(ColorTheme.Type.TextLight);

        private static readonly IIcon ICON_A_CHARGE = new IconMeleeChargeA(COLOR_A);
        private static readonly IIcon ICON_B_CHARGE = new IconMeleeChargeB(COLOR_B);
        private static readonly IIcon ICON_C_CHARGE = new IconMeleeChargeC(COLOR_C);
        private static readonly IIcon ICON_D_CHARGE = new IconMeleeChargeD(COLOR_D);
        private static readonly IIcon ICON_E_CHARGE = new IconMeleeChargeE(COLOR_E);
        private static readonly IIcon ICON_F_CHARGE = new IconMeleeChargeF(COLOR_F);
        private static readonly IIcon ICON_G_CHARGE = new IconMeleeChargeG(COLOR_G);
        private static readonly IIcon ICON_H_CHARGE = new IconMeleeChargeH(COLOR_H);
        
        private static readonly IIcon ICON_A_TAP = new IconMeleeTapA(COLOR_A);
        private static readonly IIcon ICON_B_TAP = new IconMeleeTapB(COLOR_B);
        private static readonly IIcon ICON_C_TAP = new IconMeleeTapC(COLOR_C);
        private static readonly IIcon ICON_D_TAP = new IconMeleeTapD(COLOR_D);
        private static readonly IIcon ICON_E_TAP = new IconMeleeTapE(COLOR_E);
        private static readonly IIcon ICON_F_TAP = new IconMeleeTapF(COLOR_F);
        private static readonly IIcon ICON_G_TAP = new IconMeleeTapG(COLOR_G);
        private static readonly IIcon ICON_H_TAP = new IconMeleeTapH(COLOR_H);

        private const string NAME_ICONS = "GC-Melee-Node-Icons";
        private const string NAME_ICON_HISTORY = "GC-Melee-Node-Icon-History";
        private const string NAME_ICON_CURRENT = "GC-Melee-Node-Icon-Current";
        private const string NAME_SKILL = "GC-Melee-Node-Skill";

        // MEMBERS: -------------------------------------------------------------------------------

        private readonly VisualElement m_Icons; 
        private readonly Label m_Text;
        
        // PROPERTIES: ----------------------------------------------------------------------------
        
        private SerializedProperty PropertyData { get; set; }
        private int Id { get; set; }
        
        private CombosTool CombosTool { get; }

        // CONSTRUCTOR: ---------------------------------------------------------------------------

        public CombosToolTreeNode(CombosTool combosTool)
        {
            this.CombosTool = combosTool;

            this.m_Icons = new VisualElement { name = NAME_ICONS };
            
            this.m_Text = new Label
            {
                name = NAME_SKILL,
                text = string.Empty
            };
            
            this.Add(this.m_Icons);
            this.Add(this.m_Text);

            ContextualMenuManipulator man = new ContextualMenuManipulator(this.OnOpenMenu);
            this.AddManipulator(man);
        }
        
        // PUBLIC METHODS: ------------------------------------------------------------------------

        public void BindItem(SerializedProperty propertyData, int id)
        {
            this.PropertyData = propertyData;
            this.Id = id;
            
            this.Refresh();
            
            this.CombosTool.Inspector.EventChange -= this.Refresh;
            this.CombosTool.Inspector.EventChange += this.Refresh;
        }

        public void UnbindItem()
        {
            this.CombosTool.Inspector.EventChange -= this.Refresh;
        }
        
        // PRIVATE METHODS: -----------------------------------------------------------------------
        
        private void Refresh()
        {
            this.m_Icons.Clear();
            
            ComboItem comboItem = this.PropertyData.GetValue<ComboItem>();
            if (comboItem == null) return;

            ComboItemParent[] parentsData = this.CombosTool.Instance.GetParentData(this.Id);
            for (int i = 0; i < parentsData.Length; i++)
            {
                ComboItemParent parentData = parentsData[i];
                Image image = new Image
                {
                    name = NAME_ICON_HISTORY,
                    image = parentData.Mode switch
                    {
                        MeleeMode.Tap => parentData.Key switch
                        {
                            MeleeKey.A => ICON_A_TAP_HISTORY.Texture,
                            MeleeKey.B => ICON_B_TAP_HISTORY.Texture,
                            MeleeKey.C => ICON_C_TAP_HISTORY.Texture,
                            MeleeKey.D => ICON_D_TAP_HISTORY.Texture,
                            MeleeKey.E => ICON_E_TAP_HISTORY.Texture,
                            MeleeKey.F => ICON_F_TAP_HISTORY.Texture,
                            MeleeKey.G => ICON_G_TAP_HISTORY.Texture,
                            MeleeKey.H => ICON_H_TAP_HISTORY.Texture,
                            _ => throw new ArgumentOutOfRangeException()
                        },
                        MeleeMode.Charge => parentData.Key switch
                        {
                            MeleeKey.A => ICON_A_CHARGE_HISTORY.Texture,
                            MeleeKey.B => ICON_B_CHARGE_HISTORY.Texture,
                            MeleeKey.C => ICON_C_CHARGE_HISTORY.Texture,
                            MeleeKey.D => ICON_D_CHARGE_HISTORY.Texture,
                            MeleeKey.E => ICON_E_CHARGE_HISTORY.Texture,
                            MeleeKey.F => ICON_F_CHARGE_HISTORY.Texture,
                            MeleeKey.G => ICON_G_CHARGE_HISTORY.Texture,
                            MeleeKey.H => ICON_H_CHARGE_HISTORY.Texture,
                            _ => throw new ArgumentOutOfRangeException()
                        },
                        _ => throw new ArgumentOutOfRangeException()
                    }
                };

                if (i == 0)
                {
                    Image rootExecution = new Image
                    {
                        image = parentData.When switch
                        {
                            MeleeExecute.InOrder => ICON_ROOT_HISTORY_INORDER.Texture,
                            MeleeExecute.AnyTime => ICON_ROOT_HISTORY_ANYTIME.Texture,
                            _ => throw new ArgumentOutOfRangeException()
                        }
                    };
                    
                    image.Add(rootExecution);
                }

                this.m_Icons.Add(image);
            }

            Image current = new Image
            {
                name = NAME_ICON_CURRENT,
                image = comboItem.Mode switch
                {
                    MeleeMode.Tap => comboItem.Key switch
                    {
                        MeleeKey.A => ICON_A_TAP.Texture,
                        MeleeKey.B => ICON_B_TAP.Texture,
                        MeleeKey.C => ICON_C_TAP.Texture,
                        MeleeKey.D => ICON_D_TAP.Texture,
                        MeleeKey.E => ICON_E_TAP.Texture,
                        MeleeKey.F => ICON_F_TAP.Texture,
                        MeleeKey.G => ICON_G_TAP.Texture,
                        MeleeKey.H => ICON_H_TAP.Texture,
                        _ => throw new ArgumentOutOfRangeException()
                    },
                    MeleeMode.Charge => comboItem.Key switch
                    {
                        MeleeKey.A => ICON_A_CHARGE.Texture,
                        MeleeKey.B => ICON_B_CHARGE.Texture,
                        MeleeKey.C => ICON_C_CHARGE.Texture,
                        MeleeKey.D => ICON_D_CHARGE.Texture,
                        MeleeKey.E => ICON_E_CHARGE.Texture,
                        MeleeKey.F => ICON_F_CHARGE.Texture,
                        MeleeKey.G => ICON_G_CHARGE.Texture,
                        MeleeKey.H => ICON_H_CHARGE.Texture,
                        _ => throw new ArgumentOutOfRangeException()
                    },
                    _ => throw new ArgumentOutOfRangeException()
                }
            };

            if (parentsData.Length == 0)
            {
                Image rootExecution = new Image
                {
                    image = comboItem.When switch
                    {
                        MeleeExecute.InOrder => ICON_ROOT_INORDER.Texture,
                        MeleeExecute.AnyTime => ICON_ROOT_ANYTIME.Texture,
                        _ => throw new ArgumentOutOfRangeException()
                    }
                };
                
                current.Add(rootExecution);
            }
            
            this.m_Icons.Add(current);
            
            string text = TextUtils.Humanize(comboItem.ToString());
            this.m_Text.text = text;
            
            bool isDisabled = this.PropertyData
                .FindPropertyRelative(ComboItemDrawer.PROP_IS_DISABLED)
                .boolValue;
            
            this.m_Icons.SetEnabled(!isDisabled);
            this.m_Text.SetEnabled(!isDisabled);
        }
        
        private void OnOpenMenu(ContextualMenuPopulateEvent menu)
        {
            menu.menu.AppendAction(
                "Activate",
                _ => this.SetState(false),
                _ => this.GetState()
                    ? DropdownMenuAction.Status.Normal 
                    : DropdownMenuAction.Status.Hidden
            );
            
            menu.menu.AppendAction(
                "Deactivate",
                _ => this.SetState(true),
                _ => this.GetState()
                    ? DropdownMenuAction.Status.Hidden 
                    : DropdownMenuAction.Status.Normal
            );
            
            menu.menu.AppendSeparator();
            menu.menu.AppendAction(
                "Delete",
                _ =>
                {
                    ComboItem comboItem = this.PropertyData.GetValue<ComboItem>();
                    if (comboItem == null) return;

                    bool valid = this.CombosTool.Tree.Select(this.Id);
                    if (!valid) return;

                    this.CombosTool.Tree.RemoveSelection();
                }
            );
        }

        private void SetState(bool value)
        {
            ComboItem comboItem = this.PropertyData.GetValue<ComboItem>();
            if (comboItem == null) return;

            SerializedProperty isDisabled = this.PropertyData
                .FindPropertyRelative(ComboItemDrawer.PROP_IS_DISABLED);

            isDisabled.boolValue = value;
            SerializationUtils.ApplyUnregisteredSerialization(isDisabled.serializedObject);
            
            this.Refresh();
        }
        
        private bool GetState()
        {
            ComboItem comboItem = this.PropertyData.GetValue<ComboItem>();
            
            if (comboItem == null) return false;

            return this.PropertyData
                .FindPropertyRelative(ComboItemDrawer.PROP_IS_DISABLED)
                .boolValue;
        }
    }
}