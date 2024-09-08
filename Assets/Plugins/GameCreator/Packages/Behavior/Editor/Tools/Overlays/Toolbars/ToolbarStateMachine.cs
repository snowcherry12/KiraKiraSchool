using UnityEditor.Overlays;

namespace GameCreator.Editor.Behavior
{
    [Overlay(
        id = ID,
        displayName = NAME,
        editorWindowType = typeof(WindowStateMachine),
        defaultDisplay = true,
        defaultDockZone = DockZone.TopToolbar,
        defaultDockPosition = DockPosition.Top,
        defaultDockIndex = 1,
        defaultLayout = Layout.HorizontalToolbar
    )]
    
    internal class ToolbarStateMachine : TToolbar
    { }
}