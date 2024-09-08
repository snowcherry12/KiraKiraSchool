using UnityEditor.Overlays;

namespace GameCreator.Editor.Behavior
{
    [Overlay(
        id = ID,
        displayName = NAME,
        editorWindowType = typeof(WindowActionPlan),
        defaultDisplay = true,
        defaultDockZone = DockZone.TopToolbar,
        defaultDockPosition = DockPosition.Bottom,
        defaultDockIndex = 1,
        defaultLayout = Layout.HorizontalToolbar
    )]
    
    internal class PanelActionPlan : TPanel
    { }
}