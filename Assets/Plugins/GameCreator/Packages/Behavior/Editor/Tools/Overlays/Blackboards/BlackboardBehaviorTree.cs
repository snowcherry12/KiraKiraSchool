using UnityEditor.Overlays;

namespace GameCreator.Editor.Behavior
{
    [Overlay(
        id = ID,
        displayName = NAME,
        editorWindowType = typeof(WindowBehaviorTree),
        defaultDisplay = true,
        defaultDockZone = DockZone.LeftColumn,
        defaultDockPosition = DockPosition.Top,
        defaultDockIndex = 1,
        defaultLayout = Layout.Panel
    )]

    internal class BlackboardBehaviorTree : TBlackboard
    {
        // PROPERTIES: ----------------------------------------------------------------------------
        
        protected override Layout supportedLayouts => Layout.Panel;

        protected override string Title => "Blackboard";
    }
}