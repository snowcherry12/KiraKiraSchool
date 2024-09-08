using System;
using System.Collections.Generic;
using GameCreator.Runtime.Cameras;
using GameCreator.Runtime.Characters;
using GameCreator.Runtime.Common;
using UnityEngine;

namespace GameCreator.Runtime.Quests.UnityUI
{
    [AddComponentMenu("Game Creator/UI/Quests/Minimap UI")]
    [Icon(RuntimePaths.PACKAGES + "Quests/Editor/Gizmos/GizmoMinimapUI.png")]
    
    [DefaultExecutionOrder(ApplicationManager.EXECUTION_ORDER_LAST_LATER)]
    
    [Serializable]
    public class MinimapUI : MonoBehaviour
    {
        // EXPOSED MEMBERS: -----------------------------------------------------------------------
        
        [SerializeField] private PropertyGetGameObject m_Character = GetGameObjectPlayer.Create();
        [SerializeField] private PropertyGetGameObject m_Camera = GetGameObjectCameraMain.Create;
        [SerializeField] private PropertyGetDecimal m_Radius = GetDecimalDecimal.Create(10f);

        [SerializeField] private InterestLayer m_Layers = InterestLayers.Every;
        
        [SerializeField] private GameObject m_Prefab;
        [SerializeField] private RectTransform m_Content;
        
        [SerializeField] private bool m_HiddenQuests;
        [SerializeField] private bool m_HideUntracked;
        [SerializeField] private bool m_KeepInBounds;
        
        // MEMBERS: -------------------------------------------------------------------------------

        [NonSerialized] private Character m_Source;
                
        // UPDATE METHOD: -------------------------------------------------------------------------

        private void LateUpdate()
        {
            this.m_Source = m_Character.Get<Character>(this.gameObject);
            if (m_Source == null) return;

            Transform cam = this.m_Camera.Get<Transform>(this.gameObject);
            if (cam == null) return;

            List<TSpotPoi> points = PointsOfInterest.List;
            for (int i = points.Count - 1; i >= 0; --i)
            {
                if ((points[i].Layers & this.m_Layers) != 0) continue;
                points.RemoveAt(i);
            }
            
            points.Sort(this.SortByDistance);
            
            RectTransformUtils.RebuildChildren(this.m_Content, this.m_Prefab, points.Count);

            float radius = (float) this.m_Radius.Get(this.gameObject);
            Vector3 sourceXZ = new Vector3(this.m_Source.Feet.x, 0f, this.m_Source.Feet.z);
            
            for (int i = 0; i < points.Count; i++)
            {
                TSpotPoi spot = PointsOfInterest.List[i];
                if (spot == null) continue;
                
                Vector3 point = spot.Position;
                point = new Vector3(point.x, 0f, point.z);
                
                Vector3 direction = Vector3.Scale(point - sourceXZ, Vector3Plane.NormalUp);
                Vector2 position = cam.InverseTransformDirection(direction).XZ();

                position = position.normalized * direction.magnitude;
                position = Vector2.ClampMagnitude(position / radius, 1f);

                GameObject itemInstance = this.m_Content.GetChild(i).gameObject;
                
                RectTransform itemRectTransform = itemInstance.Get<RectTransform>(); 
                
                itemRectTransform.pivot = new Vector2(0.5f, 0.5f);
                itemRectTransform.anchorMin = new Vector2(0.5f, 0.5f);
                itemRectTransform.anchorMax = new Vector2(0.5f, 0.5f);
                itemRectTransform.anchoredPosition = new Vector2(
                    this.m_Content.rect.width / 2f * position.x, 
                    this.m_Content.rect.height / 2f * position.y
                );
                
                MinimapItemUI itemMinimap = itemInstance.Get<MinimapItemUI>();
                if (itemMinimap != null) itemMinimap.Refresh(spot);
                
                bool questFilter = spot.Filter(this.m_HiddenQuests, this.m_HideUntracked);
                bool inBounds = this.m_KeepInBounds || position.magnitude <= 0.99f;
                itemInstance.SetActive(inBounds && questFilter);
            }
        }
        
        // PRIVATE METHODS: -----------------------------------------------------------------------

        private int SortByDistance(TSpotPoi a, TSpotPoi b)
        {
            float distanceA = Vector3.Distance(m_Source.Feet, a.Position);
            float distanceB = Vector3.Distance(m_Source.Feet, b.Position);
            
            return distanceA.CompareTo(distanceB);
        }
    }
}