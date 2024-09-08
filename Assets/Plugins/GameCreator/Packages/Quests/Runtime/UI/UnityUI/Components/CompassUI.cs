using System;
using System.Collections.Generic;
using GameCreator.Runtime.Cameras;
using GameCreator.Runtime.Characters;
using GameCreator.Runtime.Common;
using UnityEngine;

namespace GameCreator.Runtime.Quests.UnityUI
{
    [AddComponentMenu("Game Creator/UI/Quests/Compass UI")]
    [Icon(RuntimePaths.PACKAGES + "Quests/Editor/Gizmos/GizmoCompassUI.png")]
    
    [DefaultExecutionOrder(ApplicationManager.EXECUTION_ORDER_LAST_LATER)]
    
    [Serializable]
    public class CompassUI : MonoBehaviour
    {
        private const float MIN_L = -0.99f;
        private const float MIN_R =  0.99f;

        // EXPOSED MEMBERS: -----------------------------------------------------------------------

        [SerializeField] private PropertyGetGameObject m_Character = GetGameObjectPlayer.Create();
        [SerializeField] private PropertyGetGameObject m_Camera = GetGameObjectCameraMain.Create;
        [SerializeField] private PropertyGetDecimal m_FieldOfView = GetDecimalDecimal.Create(180f);

        [SerializeField] private InterestLayer m_Layers = InterestLayers.Every;

        [SerializeField] private GameObject m_Prefab;
        [SerializeField] private RectTransform m_Content;
        
        [SerializeField] private bool m_HiddenQuests;
        [SerializeField] private bool m_HideUntracked;
        
        // MEMBERS: -------------------------------------------------------------------------------

        [NonSerialized] private Transform m_Origin;
        
        // UPDATE METHOD: -------------------------------------------------------------------------

        private void LateUpdate()
        {
            this.m_Origin = m_Character.Get<Transform>(this.gameObject);
            if (m_Origin == null) return;
            
            Camera cam = this.m_Camera.Get<Camera>(this.gameObject);
            if (cam == null) return;

            List<TSpotPoi> points = PointsOfInterest.List;
            for (int i = points.Count - 1; i >= 0; --i)
            {
                if ((points[i].Layers & this.m_Layers) != 0) continue;
                points.RemoveAt(i);
            }
            
            points.Sort(this.SortByDistance);
            
            RectTransformUtils.RebuildChildren(this.m_Content, this.m_Prefab, points.Count);

            for (int i = 0; i < points.Count; i++)
            {
                TSpotPoi spot = PointsOfInterest.List[i];
                if (spot == null) continue;
                
                Vector3 point = spot.Position;
                Vector3 direction = point - m_Origin.position;
                
                float angle = Vector2.SignedAngle(
                    new Vector2(direction.x, direction.z),
                    new Vector2(cam.transform.forward.x, cam.transform.forward.z)
                );

                float fov = (float) this.m_FieldOfView.Get(this.gameObject);
                float position = Mathf.Clamp(angle * 2f / fov, -1f, 1f);
                
                GameObject itemInstance = this.m_Content.GetChild(i).gameObject;
                RectTransform itemRectTransform = itemInstance.Get<RectTransform>(); 
                
                itemRectTransform.pivot = new Vector2(0.5f, 0.5f);
                itemRectTransform.anchorMin = new Vector2(0.5f, 0.5f);
                itemRectTransform.anchorMax = new Vector2(0.5f, 0.5f);
                itemRectTransform.anchoredPosition = new Vector2(
                    this.m_Content.rect.width / 2f * position, 
                    0f
                );

                CompassItemUI itemCompass = itemInstance.Get<CompassItemUI>();

                float ratio = Mathf.InverseLerp(MIN_L, MIN_R, position);
                if (itemCompass != null) itemCompass.Refresh(spot, ratio);
                
                bool inFrame = position is > MIN_L and < MIN_R;
                bool questFilter = spot.Filter(this.m_HiddenQuests, this.m_HideUntracked);
                
                itemInstance.SetActive(inFrame && questFilter);
            }
        }
        
        // PRIVATE METHODS: -----------------------------------------------------------------------

        private int SortByDistance(TSpotPoi a, TSpotPoi b)
        {
            float distanceA = Vector3.Distance(m_Origin.position, a.Position);
            float distanceB = Vector3.Distance(m_Origin.position, b.Position);
            
            return distanceA.CompareTo(distanceB);
        }
    }
}