using System;
using System.Collections.Generic;
using GameCreator.Runtime.Cameras;
using GameCreator.Runtime.Characters;
using GameCreator.Runtime.Common;
using GameCreator.Runtime.VisualScripting;
using UnityEngine;

namespace GameCreator.Runtime.Perception.UnityUI
{
    [AddComponentMenu("Game Creator/UI/Perception/Indicator Awareness UI")]
    [Icon(RuntimePaths.PACKAGES + "Perception/Editor/Gizmos/GizmoAwarenessUI.png")]

    [DefaultExecutionOrder(ApplicationManager.EXECUTION_ORDER_LAST_LATER)]

    [Serializable]
    public class IndicatorAwarenessUI : MonoBehaviour
    {
        #if UNITY_EDITOR

        [UnityEditor.InitializeOnEnterPlayMode]
        private static void OnEnterPlayMode()
        {
            ListPerceptions.Clear();
        }

        #endif

        private static readonly List<ISpatialHash> ListPerceptions = new List<ISpatialHash>();

        // EXPOSED MEMBERS: -----------------------------------------------------------------------

        [SerializeField] private PropertyGetGameObject m_Camera = GetGameObjectCameraMain.Create;

        [SerializeField] private PropertyGetGameObject m_Target = GetGameObjectPlayer.Create();
        [SerializeField] private PropertyGetDecimal m_Radius = GetDecimalDecimal.Create(15f);

        [SerializeField] private PropertyGetColor m_ColorNone = GetColorColorsWhite.Create;
        [SerializeField] private PropertyGetColor m_ColorSuspicious = GetColorColorsYellow.Create;
        [SerializeField] private PropertyGetColor m_ColorAlert = GetColorColorsYellow.Create;
        [SerializeField] private PropertyGetColor m_ColorAware = GetColorColorsRed.Create;

        [SerializeField] private GameObject m_Prefab;
        [SerializeField] private RectTransform m_Content;

        [SerializeField] private bool m_KeepInBounds;
        [SerializeField] private ConditionList m_Filter = new ConditionList();

        // MEMBERS: -------------------------------------------------------------------------------

        [NonSerialized] private Camera m_CameraCache;
        [NonSerialized] private Canvas m_Canvas;

        [NonSerialized] private Args m_ArgsComponent;
        [NonSerialized] private Args m_ArgsTarget;

        [NonSerialized] private readonly Vector3[] m_ContentCorners = new Vector3[4];

        // INITIALIZERS: --------------------------------------------------------------------------

        private void Awake()
        {
            this.m_Canvas = this.m_Content != null
                ? this.m_Content.GetComponentInParent<Canvas>()
                : null;

            this.m_ArgsComponent = new Args(this.gameObject);
        }

        private void OnEnable()
        {
            GameObject target = this.m_Target.Get(this.m_ArgsComponent);
            this.m_ArgsTarget = new Args(target);
        }

        // UPDATE METHOD: -------------------------------------------------------------------------

        private void LateUpdate()
        {
            if (this.m_Canvas == null) return;

            this.m_CameraCache = this.m_Camera.Get<Camera>(this.gameObject);
            if (this.m_CameraCache == null) return;

            GameObject target = this.m_Target.Get(this.m_ArgsComponent);
            if (target != this.m_ArgsTarget.Self)
            {
                this.m_ArgsTarget.ChangeSelf(target);
            }

            ListPerceptions.Clear();
            if (target != null)
            {
                float radius = (float) this.m_Radius.Get(this.m_ArgsComponent);
                SpatialHashPerception.Find(target.transform.position, radius, ListPerceptions);
            }

            for (int i = ListPerceptions.Count - 1; i >= 0; --i)
            {
                Perception perception = ListPerceptions[i] as Perception;
                if (perception == null || perception.gameObject == target)
                {
                    ListPerceptions.RemoveAt(i);
                    continue;
                }

                this.m_ArgsTarget.ChangeTarget(perception);
                if (!this.m_Filter.Check(this.m_ArgsTarget, CheckMode.And))
                {
                    ListPerceptions.RemoveAt(i);
                }
            }

            ListPerceptions.Sort(this.SortByDistance);

            RectTransformUtils.RebuildChildren(this.m_Content, this.m_Prefab, ListPerceptions.Count);
            this.m_Content.GetLocalCorners(this.m_ContentCorners);

            for (int i = 0; i < ListPerceptions.Count; i++)
            {
                ISpatialHash spatialHash = ListPerceptions[i];
                Perception perception = spatialHash as Perception;
                Character character = perception.Get<Character>();

                if (perception == null) continue;

                Vector3 instancePosition = spatialHash.Position;
                if (character != null)
                {
                    instancePosition += Vector3.up * (character.Motion.Height * 0.5f);
                }

                Vector3 spotPositionRelative = this.m_CameraCache.transform.InverseTransformPoint(
                    instancePosition
                );

                if (spotPositionRelative.z < 0f)
                {
                    spotPositionRelative.z *= -1f;
                    instancePosition = this.m_CameraCache.transform.TransformPoint(spotPositionRelative);
                }

                Vector2 screenPoint = RectTransformUtility.WorldToScreenPoint(this.m_CameraCache, instancePosition);

                screenPoint.x = Mathf.Clamp(screenPoint.x, 0f, this.m_CameraCache.pixelWidth);
                screenPoint.y = Mathf.Clamp(screenPoint.y, 0f, this.m_CameraCache.pixelHeight);

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

                IndicatorAwarenessItemUI itemIndicator = itemInstance.Get<IndicatorAwarenessItemUI>();

                Vector2 screenCenter = new Vector2(
                    this.m_CameraCache.pixelWidth * 0.5f,
                    this.m_CameraCache.pixelHeight * 0.5f
                );

                Vector2 itemDirection = screenPoint - screenCenter;
                float rotation = Vector2.SignedAngle(Vector2.up, itemDirection);

                if (itemIndicator != null)
                {
                    float awareness = perception.GetTracker(target)?.Awareness ?? 0f;
                    Color color = Tracker.GetStage(awareness) switch
                    {
                        AwareStage.None => this.m_ColorNone.Get(this.m_ArgsComponent),
                        AwareStage.Suspicious => this.m_ColorSuspicious.Get(this.m_ArgsComponent),
                        AwareStage.Alert => this.m_ColorAlert.Get(this.m_ArgsComponent),
                        AwareStage.Aware => this.m_ColorAware.Get(this.m_ArgsComponent),
                        _ => throw new ArgumentOutOfRangeException()
                    };

                    itemIndicator.Refresh(
                        awareness,
                        color,
                        onScreen, rotation
                    );
                }

                itemInstance.SetActive(this.m_KeepInBounds || onScreen);
            }
        }

        // PRIVATE METHODS: -----------------------------------------------------------------------

        private int SortByDistance(ISpatialHash a, ISpatialHash b)
        {
            float distanceA = Vector3.Distance(this.m_CameraCache.transform.position, a.Position);
            float distanceB = Vector3.Distance(this.m_CameraCache.transform.position, b.Position);

            return distanceA.CompareTo(distanceB);
        }
    }
}
