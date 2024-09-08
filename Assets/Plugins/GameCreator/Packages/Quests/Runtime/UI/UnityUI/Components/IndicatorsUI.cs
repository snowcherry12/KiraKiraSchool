using System;
using System.Collections.Generic;
using GameCreator.Runtime.Cameras;
using GameCreator.Runtime.Common;
using UnityEngine;

namespace GameCreator.Runtime.Quests.UnityUI
{
    [AddComponentMenu("Game Creator/UI/Quests/Indicators UI")]
    [Icon(RuntimePaths.PACKAGES + "Quests/Editor/Gizmos/GizmoIndicatorUI.png")]
    
    [DefaultExecutionOrder(ApplicationManager.EXECUTION_ORDER_LAST_LATER)]
    
    [Serializable]
    public class IndicatorsUI : MonoBehaviour
    {
        // EXPOSED MEMBERS: -----------------------------------------------------------------------

        [SerializeField] private PropertyGetGameObject m_Camera = GetGameObjectCameraMain.Create;

        [SerializeField] private InterestLayer m_Layers = InterestLayers.Every;
        
        [SerializeField] private GameObject m_Prefab;
        [SerializeField] private RectTransform m_Content;
        
        [SerializeField] private bool m_HiddenQuests;
        [SerializeField] private bool m_HideUntracked;
        [SerializeField] private bool m_KeepInBounds;
        
        // MEMBERS: -------------------------------------------------------------------------------

        [NonSerialized] private Camera m_Cam;
        [NonSerialized] private Canvas m_Canvas;
        
        [NonSerialized] private readonly Vector3[] m_ContentCorners = new Vector3[4];

        // INITIALIZERS: --------------------------------------------------------------------------

        private void Awake()
        {
            this.m_Canvas = this.m_Content != null
                ? this.m_Content.GetComponentInParent<Canvas>()
                : null;
        }

        // UPDATE METHOD: -------------------------------------------------------------------------

        private void LateUpdate()
        {
            if (this.m_Canvas == null) return;
            
            this.m_Cam = this.m_Camera.Get<Camera>(this.gameObject);
            if (this.m_Cam == null) return;

            List<TSpotPoi> points = PointsOfInterest.List;
            for (int i = points.Count - 1; i >= 0; --i)
            {
                if ((points[i].Layers & this.m_Layers) != 0) continue;
                points.RemoveAt(i);
            }
            
            points.Sort(this.SortByDistance);
            
            RectTransformUtils.RebuildChildren(this.m_Content, this.m_Prefab, points.Count);
            this.m_Content.GetLocalCorners(this.m_ContentCorners);

            for (int i = 0; i < points.Count; i++)
            {
                TSpotPoi spot = points[i];
                if (spot == null) continue;

                Vector3 spotPosition = spot.Position;
                
                Vector3 spotPositionRelative = this.m_Cam.transform.InverseTransformPoint(spotPosition);
                if (spotPositionRelative.z < 0f)
                {
                    spotPositionRelative.z *= -1f;
                    spotPosition = this.m_Cam.transform.TransformPoint(spotPositionRelative);
                }
                
                Vector2 screenPoint = RectTransformUtility.WorldToScreenPoint(this.m_Cam, spotPosition);
                
                screenPoint.x = Mathf.Clamp(screenPoint.x, 0f, this.m_Cam.pixelWidth);
                screenPoint.y = Mathf.Clamp(screenPoint.y, 0f, this.m_Cam.pixelHeight);

                bool valid = RectTransformUtility.ScreenPointToLocalPointInRectangle(
                    this.m_Content,
                    screenPoint,
                    this.m_Canvas.worldCamera,
                    out Vector2 position
                );

                GameObject itemInstance = this.m_Content.GetChild(i).gameObject;
                RectTransform itemRectTransform = itemInstance.Get<RectTransform>();
                
                if (!valid)
                {
                    itemInstance.SetActive(false);
                    continue;
                }
                
                itemRectTransform.pivot = new Vector2(0.5f, 0.5f);
                itemRectTransform.anchorMin = new Vector2(0.5f, 0.5f);
                itemRectTransform.anchorMax = new Vector2(0.5f, 0.5f);
                itemRectTransform.anchoredPosition = position;

                bool onScreen = true;

                Bounds itemBounds = RectTransformUtility.CalculateRelativeRectTransformBounds(
                    this.m_Content,
                    itemRectTransform
                );
                
                Vector3 contentCornerTopLeft = this.m_ContentCorners[1];
                Vector3 contentCornerBottomRight = this.m_ContentCorners[3];
                
                if (this.m_KeepInBounds)
                {
                    float offsetX = 0f;
                    float offsetY = 0f;
                
                    if (itemBounds.min.x < contentCornerTopLeft.x)
                    {
                        onScreen = false;
                        offsetX = contentCornerTopLeft.x - itemBounds.min.x;
                    }
                    
                    if (itemBounds.max.y > contentCornerTopLeft.y)
                    {
                        onScreen = false;
                        offsetY = contentCornerTopLeft.y - itemBounds.max.y;
                    }
                
                    if (itemBounds.min.y < contentCornerBottomRight.y)
                    {
                        onScreen = false;
                        offsetY = contentCornerBottomRight.y - itemBounds.min.y;
                    }

                    if (itemBounds.max.x > contentCornerBottomRight.x)
                    {
                        onScreen = false;
                        offsetX = contentCornerBottomRight.x - itemBounds.max.x;
                    }

                    itemRectTransform.anchoredPosition += new Vector2(offsetX, offsetY);
                }
                else
                {
                    if (itemBounds.min.x < contentCornerTopLeft.x) onScreen = false;
                    if (itemBounds.max.y > contentCornerTopLeft.y) onScreen = false;
                    
                    if (itemBounds.min.y < contentCornerBottomRight.y) onScreen = false;
                    if (itemBounds.max.x > contentCornerBottomRight.x) onScreen = false;
                }
                
                IndicatorItemUI itemIndicator = itemInstance.Get<IndicatorItemUI>();

                Vector2 screenCenter = new Vector2(
                    this.m_Cam.pixelWidth * 0.5f,
                    this.m_Cam.pixelHeight * 0.5f
                );
                
                Vector2 itemDirection = screenPoint - screenCenter;
                float rotation = Vector2.SignedAngle(Vector2.up, itemDirection);

                if (itemIndicator != null) itemIndicator.Refresh(spot, onScreen, rotation);
                
                bool isActive = this.m_KeepInBounds || onScreen;
                bool questFilter = spot.Filter(this.m_HiddenQuests, this.m_HideUntracked);

                itemInstance.SetActive(isActive && questFilter);
            }
        }
        
        // PRIVATE METHODS: -----------------------------------------------------------------------

        private int SortByDistance(TSpotPoi a, TSpotPoi b)
        {
            float distanceA = Vector3.Distance(this.m_Cam.transform.position, a.Position);
            float distanceB = Vector3.Distance(this.m_Cam.transform.position, b.Position);
            
            return distanceA.CompareTo(distanceB);
        }
    }
}