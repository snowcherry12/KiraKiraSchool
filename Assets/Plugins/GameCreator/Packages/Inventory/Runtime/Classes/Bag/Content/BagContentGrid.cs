using System;
using System.Collections.Generic;
using GameCreator.Runtime.Common;
using UnityEngine;

namespace GameCreator.Runtime.Inventory
{
    [Serializable]
    public class BagContentGrid : TBagContent
    {
        [Serializable]
        public class Matrix : TSerializableMatrix2D<IdString>
        {
            public Matrix() : base() 
            { }
            
            public Matrix(int width, int height) : base(width, height) 
            { }
        }
        
        // EXPOSED MEMBERS: -----------------------------------------------------------------------

        [SerializeField] private Cells m_Cells = new Cells();
        [SerializeField] private Stack m_Stack = new Stack();
        [SerializeField] private Matrix m_Matrix = new Matrix();
        
        // PROPERTIES: ----------------------------------------------------------------------------

        public override List<Cell> CellList => new List<Cell>(this.m_Cells.Values);

        public override List<Bucket> RuntimeItemsClone
        {
            get
            {
                List<Bucket> result = new List<Bucket>();
                for (int i = 0; i < this.m_Matrix.MatrixHeight; ++i)
                {
                    for (int j = 0; j < this.m_Matrix.MatrixWidth; ++j)
                    {
                        bool valid = this.m_Matrix.TryGet(j, i, out IdString rootRuntimeItemId);
                        if (!valid) continue;

                        if (this.m_Cells.TryGetValue(rootRuntimeItemId, out Cell cell))
                        {
                            RuntimeItem[] runtimeItems = new RuntimeItem[cell.Count];
                            List<IdString> cellRuntimeItemIds = cell.List;
                            for (int k = 0; k < cellRuntimeItemIds.Count; ++k)
                            {
                                RuntimeItem runtimeItemCopy = cell.Get(cellRuntimeItemIds[k]);
                                runtimeItems[k] = CloneUtils.Deep(runtimeItemCopy);
                            }
                            
                            result.Add(new Bucket(runtimeItems));
                        }
                        else
                        {
                            RuntimeItem[] runtimeItems = Array.Empty<RuntimeItem>();
                            result.Add(new Bucket(runtimeItems));
                        }
                    }
                }

                return result;
            }
        }
        
        public override bool CellsAsChunks => true;

        // INITIALIZERS: --------------------------------------------------------------------------

        public override void OnAwake(Bag bag)
        {
            base.OnAwake(bag);
            
            this.m_Matrix = new Matrix(
                this.Bag.Shape.MaxWidth, 
                this.Bag.Shape.MaxHeight
            );
        }

        // CHECKERS: ------------------------------------------------------------------------------

        public override bool IsAvailable(Vector2Int position, Vector2Int size)
        {
            for (int offsetX = 0; offsetX < size.x; ++offsetX)
            {
                for (int offsetY = 0; offsetY < size.y; ++offsetY)
                {
                    IdString runtimeItemID = this.m_Matrix[
                        Mathf.Clamp(position.x + offsetX, 0, this.m_Matrix.MatrixWidth),
                        Mathf.Clamp(position.y + offsetY, 0, this.m_Matrix.MatrixHeight)
                    ];

                    if (!string.IsNullOrEmpty(runtimeItemID.String)) return false;
                }
            }

            return true;
        }

        public override bool CanAdd(RuntimeItem runtimeItem, bool allowStack)
        {
            if (runtimeItem == null) return false;
            if (this.m_Cells.ContainsKey(runtimeItem.RuntimeID)) return false;
            if (this.m_Stack.ContainsKey(runtimeItem.RuntimeID)) return false;

            foreach (KeyValuePair<IdString, Cell> entry in this.m_Cells)
            {
                if (allowStack && entry.Value.CanStack(runtimeItem)) return true;
            }

            Vector2Int position = this.FindAvailableSpace(runtimeItem);
            return position != INVALID;
        }

        public override bool CanAddType(Item item, bool allowStack)
        {
            if (item == null) return false;
            
            RuntimeItem tempRuntimeItem = new RuntimeItem(item);
            return this.CanAdd(tempRuntimeItem, allowStack);
        }

        public override bool CanMove(Vector2Int positionA, Vector2Int positionB, bool allowStack)
        {
            Vector2Int rootPositionSource = this.FindRoot(positionA);
            Vector2Int rootPositionTarget = positionB - (positionA - rootPositionSource);
            
            Cell cellA = this.GetContent(positionA);
            if (cellA == null || cellA.Available) return false;
            
            bool isAvailable = this.IsAvailableFor(cellA.Item, rootPositionTarget, cellA.RootRuntimeItemID);
            if (isAvailable) return true;
            
            Cell cellB = this.GetContent(positionB);
            if (cellB == null || cellB.Available) return false;
            
            if (cellA.RootRuntimeItemID == cellB.RootRuntimeItemID) return false;
            return allowStack && cellB.CanStack(cellA.RootRuntimeItem);
        }

        public override bool Contains(IdString runtimeItemID)
        {
            runtimeItemID = this.FindStackRoot(runtimeItemID);
            return this.m_Cells.ContainsKey(runtimeItemID);
        }

        public override bool Contains(RuntimeItem runtimeItem)
        {
            return runtimeItem != null && this.Contains(runtimeItem.RuntimeID);
        }

        public override bool ContainsType(Item item, int amount)
        {
            int count = 0;
            foreach (KeyValuePair<IdString, Cell> entry in this.m_Cells)
            {
                if (entry.Value.Available) continue;
                if (entry.Value.ContainsType(item.ID))
                {
                    count += entry.Value.Count;
                    if (count >= amount) return true;
                }
            }

            return false;
        }

        // GETTERS: -------------------------------------------------------------------------------

        public override RuntimeItem GetRuntimeItem(IdString runtimeItemID)
        {
            if (this.m_Stack.TryGetValue(runtimeItemID, out IdString rootRuntimeItemID))
            {
                return this.m_Cells[rootRuntimeItemID].Get(runtimeItemID);
            }

            return this.m_Cells.TryGetValue(runtimeItemID, out Cell cell)
                ? cell.Get(runtimeItemID)
                : null;
        }

        public override Cell GetContent(Vector2Int position)
        {
            if (!this.m_Matrix.TryGet(position, out IdString runtimeItemID)) return null;
            return this.m_Cells.TryGetValue(runtimeItemID, out Cell cell) ? cell : null;
        }

        public override Vector2Int FindPosition(IdString runtimeItemID)
        {
            runtimeItemID = this.FindStackRoot(runtimeItemID);
            for (int i = 0; i < this.m_Matrix.MatrixWidth; ++i)
            {
                for (int j = 0; j < this.m_Matrix.MatrixHeight; ++j)
                {
                    IdString matrixRuntimeItemID = this.m_Matrix[i, j];
                    if (matrixRuntimeItemID.Hash == runtimeItemID.Hash)
                    {
                        return new Vector2Int(i, j);
                    }
                }
            }

            return INVALID;
        }

        public override Vector2Int FindStartPosition(Vector2Int position)
        {
            return this.FindRoot(position);
        }
        
        public override int CountFreeSpaces()
        {
            if (this.Bag.Shape.HasInfiniteWidth) return int.MaxValue;
            if (this.Bag.Shape.HasInfiniteHeight) return int.MaxValue;

            int freeSpaces = 0;
            for (int i = 0; i < this.m_Matrix.MatrixWidth; ++i)
            {
                for (int j = 0; j < this.m_Matrix.MatrixHeight; ++j)
                {
                    if (this.m_Matrix[i, j] == IdString.EMPTY)
                    {
                        freeSpaces += 1;
                    }
                }
            }

            return freeSpaces;
        }

        // SETTERS: -------------------------------------------------------------------------------

        public override bool Move(Vector2Int positionA, Vector2Int positionB, bool allowStack)
        {
            Vector2Int rootPositionSource = this.FindRoot(positionA);
            Vector2Int rootPositionTarget = positionB - (positionA - rootPositionSource);
            
            if (rootPositionSource == INVALID) return false;
            if (!this.CanMove(positionA, positionB, allowStack)) return false;
            
            Cell cellA = this.GetContent(positionA);
            Cell cellB = this.GetContent(positionB);
            
            if (allowStack && cellB != null && cellA != cellB)
            {
                while (cellA.Count > 0 && this.CanMove(positionA, positionB, true))
                {
                    RuntimeItem removeRuntimeItem = this.Remove(positionA);
                    this.Add(removeRuntimeItem, positionB, true);
                }

                return true;
            }

            int width = cellA.Item.Shape.Width;
            int height = cellA.Item.Shape.Height;
            
            IdString rootRuntimeID = this.m_Matrix[rootPositionSource];

            for (int i = 0; i < width; ++i)
            {
                for (int j = 0; j < height; ++j)
                {
                    Vector2Int offset = new Vector2Int(i, j);
                    this.m_Matrix[rootPositionSource + offset] = IdString.EMPTY;
                }
            }
            
            for (int i = 0; i < width; ++i)
            {
                for (int j = 0; j < height; ++j)
                {
                    Vector2Int offset = new Vector2Int(i, j);
                    this.m_Matrix[rootPositionTarget + offset] = rootRuntimeID;
                }
            }

            this.ExecuteEventChange();
            return true;
        }

        public override bool Add(RuntimeItem runtimeItem, Vector2Int position, bool allowStack)
        {
            if (runtimeItem == null) return false;
            if (this.Contains(runtimeItem)) return false;

            RuntimeItem.Bag_LastItemAttemptedAdd = runtimeItem;
            
            IdString rootRuntimeItemID = this.m_Matrix[position];
            if (this.m_Cells.TryGetValue(rootRuntimeItemID, out Cell cell))
            {
                if (cell.Add(runtimeItem, allowStack))
                {
                    this.m_Stack[runtimeItem.RuntimeID] = rootRuntimeItemID;
                    
                    runtimeItem.Bag = this.Bag;
                    RuntimeItem.Bag_LastItemAdded = runtimeItem;
            
                    this.ExecuteEventAdd(runtimeItem);
                    this.ExecuteEventChange();
                    
                    return true;
                }
            }
            
            IdString runtimeItemID = runtimeItem.RuntimeID;
            if (!this.IsAvailableFor(runtimeItem.Item, position, runtimeItemID)) return false;
            
            this.m_Cells.Add(runtimeItemID, new Cell(runtimeItem));
            
            int width = runtimeItem.Item.Shape.Width;
            int height = runtimeItem.Item.Shape.Height;

            for (int i = 0; i < width; ++i)
            {
                for (int j = 0; j < height; ++j)
                {
                    Vector2Int offset = new Vector2Int(i, j);
                    this.m_Matrix[position + offset] = runtimeItemID;
                }
            }

            runtimeItem.Bag = this.Bag;
            RuntimeItem.Bag_LastItemAdded = runtimeItem;
            
            this.ExecuteEventAdd(runtimeItem);
            this.ExecuteEventChange();

            return true;
        }

        public override Vector2Int Add(RuntimeItem runtimeItem, bool allowStack)
        {
            for (int i = 0; i < this.m_Matrix.MatrixWidth; ++i)
            {
                for (int j = 0; j < this.m_Matrix.MatrixHeight; ++j)
                {
                    Vector2Int stackPosition = new Vector2Int(i, j);
                    IdString parentRuntimeItemID = this.m_Matrix[stackPosition];
                    if (this.m_Cells.TryGetValue(parentRuntimeItemID, out Cell cell))
                    {
                        if (allowStack && cell.CanStack(runtimeItem))
                        {
                            return this.Add(runtimeItem, stackPosition, true)
                                ? stackPosition
                                : INVALID;
                        }
                    }
                }
            }
            
            Vector2Int availablePosition = this.FindAvailableSpace(runtimeItem);
            if (availablePosition == INVALID) return INVALID;
            
            return this.Add(runtimeItem, availablePosition, allowStack)
                ? availablePosition
                : INVALID;
        }

        public override RuntimeItem AddType(Item item, Vector2Int position, bool allowStack)
        {
            if (item == null) return null;

            RuntimeItem runtimeItem = item.CreateRuntimeItem(this.Bag.Args);
            return this.Add(runtimeItem, position, allowStack)
                ? runtimeItem
                : null;
        }

        public override RuntimeItem AddType(Item item, bool allowStack)
        {
            if (item == null) return null;
            
            RuntimeItem runtimeItem = item.CreateRuntimeItem(this.Bag.Args);
            return this.Add(runtimeItem, allowStack) != INVALID
                ? runtimeItem
                : null;
        }

        public override RuntimeItem Remove(Vector2Int position)
        {
            return this.Remove(position, null);
        }

        public override RuntimeItem Remove(RuntimeItem runtimeItem)
        {
            if (runtimeItem == null) return null;
            
            Vector2Int position = this.FindPosition(runtimeItem.RuntimeID);
            return position != INVALID ? this.Remove(position, runtimeItem) : null;
        }

        public override RuntimeItem RemoveType(Item item)
        {
            if (item == null) return null;

            foreach (KeyValuePair<IdString, Cell> entry in this.m_Cells)
            {
                if (entry.Value.Available) continue;
                if (entry.Value.ContainsType(item.ID))
                {
                    Vector2Int position = this.FindPosition(entry.Value.RootRuntimeItemID);
                    if (position != INVALID) return this.Remove(position);
                }
            }
            
            for (int i = 0; i < this.m_Matrix.MatrixWidth; ++i)
            {
                for (int j = 0; j < this.m_Matrix.MatrixHeight; ++j)
                {
                    IdString rootRuntimeItemID = this.m_Matrix[i, j];
                    if (this.m_Cells.TryGetValue(rootRuntimeItemID, out Cell cell))
                    {
                        if (cell.Available) continue;
                        if (cell.Item.ID.Hash == item.ID.Hash)
                        {
                            Vector2Int position = new Vector2Int(i, j);
                            return this.Remove(position);
                        }
                    }
                }
            }
        
            return null;
        }

        public override void Sort(Func<int, int, int> comparison)
        { }
        
        // PROTECTED METHODS: ---------------------------------------------------------------------

        protected override void OnChangeShape()
        {
            base.OnChangeShape();

            Matrix matrix = new Matrix(
                this.Bag.Shape.MaxWidth, 
                this.Bag.Shape.MaxHeight
            );

            for (int i = 0; i < this.m_Matrix.MatrixWidth; ++i)
            {
                for (int j = 0; j < this.m_Matrix.MatrixHeight; ++j)
                {
                    matrix[i, j] = this.m_Matrix[i, j];
                }
            }

            this.m_Matrix = matrix;
        }

        // PRIVATE METHODS: -----------------------------------------------------------------------

        private IdString FindStackRoot(IdString runtimeItemID)
        {
            return this.m_Stack.TryGetValue(runtimeItemID, out IdString sourceRuntimeItemID) 
                ? sourceRuntimeItemID 
                : runtimeItemID;
        }

        private bool IsAvailableFor(Item item, Vector2Int rootPosition, params IdString[] ignoreRuntimeItemIDs)
        {
            int runtimeItemWidth = item.Shape.Width;
            int runtimeItemHeight = item.Shape.Height;
            
            for (int i = 0; i < runtimeItemWidth; ++i)
            {
                for (int j = 0; j < runtimeItemHeight; ++j)
                {
                    int positionX = rootPosition.x + i;
                    int positionY = rootPosition.y + j;

                    if (positionX < 0) return false;
                    if (positionY < 0) return false;
                    
                    if (positionX >= this.m_Matrix.MatrixWidth) return false;
                    if (positionY >= this.m_Matrix.MatrixHeight) return false;
        
                    IdString cellRuntimeItemID = this.m_Matrix[positionX, positionY];
                    if (!string.IsNullOrEmpty(cellRuntimeItemID.String))
                    {
                        bool matchIgnoreRuntimeItemID = false;
                        foreach (IdString ignoreRuntimeItemID in ignoreRuntimeItemIDs)
                        {
                            if (cellRuntimeItemID.Hash == ignoreRuntimeItemID.Hash)
                            {
                                matchIgnoreRuntimeItemID = true;
                                break;
                            }
                        }

                        if (!matchIgnoreRuntimeItemID) return false;
                    }
                }
            }
        
            return true;
        }

        private Vector2Int FindAvailableSpace(RuntimeItem runtimeItem)
        {
            for (int j = 0; j < this.m_Matrix.MatrixHeight; ++j)
            {
                for (int i = 0; i < this.m_Matrix.MatrixWidth; ++i)
                {
                    Vector2Int position = new Vector2Int(i, j);
                    if (this.IsAvailableFor(runtimeItem.Item, position, runtimeItem.RuntimeID))
                    {
                        return position;
                    }
                }
            }

            return INVALID;
        }

        private Vector2Int FindRoot(Vector2Int position)
        {
            if (!this.m_Matrix.TryGet(position, out IdString runtimeItemID)) return INVALID;

            while (position.x > 0)
            {
                IdString currentRuntimeItemID = this.m_Matrix[
                    position.x - 1, 
                    position.y
                ];

                if (currentRuntimeItemID.Hash != runtimeItemID.Hash) break;
                position.x -= 1;
            }
            
            while (position.y > 0)
            {
                IdString currentRuntimeItemID = this.m_Matrix[
                    position.x, 
                    position.y - 1
                ];

                if (currentRuntimeItemID.Hash != runtimeItemID.Hash) break;
                position.y -= 1;
            }

            return position;
        }
        
        private RuntimeItem Remove(Vector2Int position, RuntimeItem runtimeItem)
        {
            RuntimeItem.Bag_LastItemAttemptedRemove = runtimeItem;
            
            position = this.FindRoot(position);
            Cell cell = this.GetContent(position);
            
            if (cell == null || cell.Available) return null;

            runtimeItem = runtimeItem != null 
                ? cell.Remove(runtimeItem.RuntimeID) 
                : cell.Pop();
            
            if (runtimeItem == null) return null;
            this.m_Stack.Remove(runtimeItem.RuntimeID);

            if (cell.Count == 0)
            {
                this.m_Cells.Remove(runtimeItem.RuntimeID);
                
                int width = runtimeItem.Item.Shape.Width;
                int height = runtimeItem.Item.Shape.Height;

                for (int i = 0; i < width; ++i)
                {
                    for (int j = 0; j < height; ++j)
                    {
                        Vector2Int offset = new Vector2Int(i, j);
                        this.m_Matrix[position + offset] = IdString.EMPTY;
                    }
                }
            }
            
            RuntimeItem.Bag_LastItemRemoved = runtimeItem;
            this.Bag.Equipment.Unequip(runtimeItem);
            
            runtimeItem.Bag = null;

            this.ExecuteEventRemove(runtimeItem);
            this.ExecuteEventChange();

            return runtimeItem;
        }
    }
}