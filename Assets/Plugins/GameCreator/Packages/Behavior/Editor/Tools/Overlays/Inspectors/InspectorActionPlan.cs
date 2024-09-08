using UnityEditor.Overlays;

namespace GameCreator.Editor.Behavior
{
    [Overlay(
        id = ID,
        displayName = NAME,
        editorWindowType = typeof(WindowActionPlan),
        defaultDisplay = true,
        defaultDockZone = DockZone.RightColumn,
        defaultDockPosition = DockPosition.Top,
        defaultDockIndex = 1,
        defaultLayout = Layout.Panel
    )]
    
    internal class InspectorActionPlan : TInspector
    {
        // PROPERTIES: ----------------------------------------------------------------------------
        
        protected override Layout supportedLayouts => Layout.Panel;
    }
}