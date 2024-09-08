using System;
using System.Collections.Generic;
using GameCreator.Runtime.Common;
using GameCreator.Runtime.Common.UnityUI;
using UnityEngine;

namespace GameCreator.Runtime.Inventory.UnityUI
{
    [Serializable]
    public class CellContentUI : RuntimeItemUI
    {
        private enum DisplayStack
        {
            Always,
            StackGreaterThanOne
        }
        
        #if UNITY_EDITOR

        [UnityEditor.InitializeOnEnterPlayMode]
        private static void OnEnterPlayMode()
        {
            PARTIAL_SPRITES = new Dictionary<Vector3Int, Sprite>();
        }
        
        #endif

        private static Dictionary<Vector3Int, Sprite> PARTIAL_SPRITES = new Dictionary<Vector3Int, Sprite>();
        
        // EXPOSED MEMBERS: -----------------------------------------------------------------------
        
        [SerializeField] private GameObject m_ActiveCornerTopLeft;
        [SerializeField] private GameObject m_ActiveCornerTopRight;
        [SerializeField] private GameObject m_ActiveCornerBottomLeft;
        [SerializeField] private GameObject m_ActiveCornerBottomRight;

        [SerializeField] private DisplayStack m_DisplayStack = DisplayStack.StackGreaterThanOne;
        [SerializeField] private GameObject m_StackContent;
        [SerializeField] private TextReference m_StackCount = new TextReference();

        // MEMBERS: -------------------------------------------------------------------------------

        [NonSerialized] private Cell m_Cell;
        
        [NonSerialized] private Vector2Int m_StartPosition;
        [NonSerialized] private Vector2Int m_CurrentPosition;

        // PUBLIC METHODS: ------------------------------------------------------------------------
        
        public void RefreshUI(Bag bag, Vector2Int position, bool forceSingleChunk)
        {
            this.m_Cell = bag.Content.GetContent(position);

            this.m_CurrentPosition = position;
            this.m_StartPosition = bag.Content.FindStartPosition(position);

            RuntimeItem runtimeItem = this.m_Cell is { Available: false }
                ? this.m_Cell.RootRuntimeItem 
                : null;

            this.RefreshUI(bag, runtimeItem, true, forceSingleChunk);

            CellCorner corner = this.GetCorner();
            if (this.m_ActiveCornerTopLeft != null) this.m_ActiveCornerTopLeft.SetActive(((int) corner & (int) CellCorner.TopLeft) != 0);
            if (this.m_ActiveCornerTopRight != null) this.m_ActiveCornerTopRight.SetActive(((int) corner & (int) CellCorner.TopRight) != 0);
            if (this.m_ActiveCornerBottomLeft != null) this.m_ActiveCornerBottomLeft.SetActive(((int) corner & (int) CellCorner.BottomLeft) != 0);
            if (this.m_ActiveCornerBottomRight != null) this.m_ActiveCornerBottomRight.SetActive(((int) corner & (int) CellCorner.BottomRight) != 0);

            int stackCountValue = this.m_Cell?.Count ?? 0;
            this.m_StackCount.Text = stackCountValue > 0 
                ? stackCountValue.ToString() 
                : string.Empty;

            if (this.m_DisplayStack == DisplayStack.StackGreaterThanOne)
            {
                if (this.m_StackContent != null)
                {
                    this.m_StackContent.SetActive(stackCountValue > 1);
                }
            }
        }
        
        // UI EVENTS: -----------------------------------------------------------------------------

        public void OnHover(Bag bag, Vector2Int position)
        {
            Cell cell = bag.Content.GetContent(position);
            this.OnHover(cell?.RootRuntimeItem);
        }

        public void OnSelect(Bag bag, Vector2Int position)
        {
            Cell cell = bag.Content.GetContent(position);
            this.OnSelect(cell?.RootRuntimeItem);
        }

        // OVERRIDE METHODS: ----------------------------------------------------------------------
        
        protected override Sprite GetSprite(Item item, bool asChunks, Args args)
        {
            Sprite sprite = base.GetSprite(item, asChunks, args);
            if (!asChunks) return sprite;
            
            if (sprite == null) return null;

            int cellsX = item.Shape.Width;
            int cellsY = item.Shape.Height;
            
            if (cellsX == 1 && cellsY == 1) return sprite;

            int cellsOffsetX = this.m_CurrentPosition.x - this.m_StartPosition.x;
            int cellsOffsetY = this.m_CurrentPosition.y - this.m_StartPosition.y;

            Vector3Int key = new Vector3Int(cellsOffsetX, cellsOffsetY, sprite.GetInstanceID());
            if (!PARTIAL_SPRITES.TryGetValue(key, out Sprite partialSprite))
            {
                float cellW = sprite.rect.width  / cellsX;
                float cellH = sprite.rect.height / cellsY;
            
                Rect rect = new Rect(
                    Mathf.Floor(sprite.rect.x + cellsOffsetX * cellW),
                    Mathf.Floor(sprite.rect.y + cellH * (cellsY - cellsOffsetY - 1)),
                    Mathf.Ceil(cellW),
                    Mathf.Ceil(cellH)
                );
                
                partialSprite = Sprite.Create(sprite.texture, rect, sprite.pivot);
                PARTIAL_SPRITES[key] = partialSprite;
            }

            return partialSprite;
        }
        
        // PRIVATE METHODS: -----------------------------------------------------------------------
        
        private CellCorner GetCorner()
        {
            if (this.m_Cell == null || this.m_Cell.Available) return 0;
            CellCorner corner = 0;
            
            int cellsX = this.m_Cell.Item.Shape.Width;
            int cellsY = this.m_Cell.Item.Shape.Height;

            int positionX = this.m_CurrentPosition.x - this.m_StartPosition.x;
            int positionY = this.m_CurrentPosition.y - this.m_StartPosition.y;
            
            if (positionX == 0)
            {
                if (positionY == 0) corner |= CellCorner.TopLeft;
                if (positionY == cellsY - 1) corner |= CellCorner.BottomLeft;
            }

            if (positionX == cellsX - 1)
            {
                if (positionY == 0) corner |= CellCorner.TopRight;
                if (positionY == cellsY - 1) corner |= CellCorner.BottomRight;
            }
            
            return corner;
        }
    }
}