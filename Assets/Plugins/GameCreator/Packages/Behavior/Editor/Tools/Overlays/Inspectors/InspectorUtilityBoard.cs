using UnityEditor.Overlays;

namespace GameCreator.Editor.Behavior
{
    [Overlay(
        id = ID,
        displayName = NAME,
        editorWindowType = typeof(WindowUtilityBoard),
        defaultDisplay = true,
        defaultDockZone = DockZone.RightColumn,
        defaultDockPosition = DockPosition.Top,
        defaultDockIndex = 1,
        defaultLayout = Layout.Panel
    )]
    
    internal class InspectorUtilityBoard : TInspector
    {
        // PROPERTIES: ----------------------------------------------------------------------------
        
        protected override Layout supportedLayouts => Layout.Panel;
    }
}