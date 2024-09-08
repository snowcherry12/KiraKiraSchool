using System;

namespace GameCreator.Runtime.Inventory
{
    [Serializable]
    public abstract class TBagShape : IBagShape
    {
        // PROPERTIES: ----------------------------------------------------------------------------
        
        [field: NonSerialized] protected Bag Bag { get; private set; }

        public abstract int MaxWidth { get; }
        public abstract int MaxHeight { get; }
        public abstract int MaxWeight { get; }
        
        public abstract bool HasInfiniteWidth { get; }
        public abstract bool HasInfiniteHeight { get; }
        
        public abstract bool CanIncreaseWidth { get; }
        public abstract bool CanDecreaseWidth { get; }
        
        public abstract bool CanIncreaseHeight { get; }
        public abstract bool CanDecreaseHeight { get; }
        
        // EVENTS: --------------------------------------------------------------------------------

        public event Action EventChange;
        
        // PUBLIC METHODS: ------------------------------------------------------------------------

        public virtual void OnAwake(Bag bag)
        {
            this.Bag = bag;
        }

        public virtual void OnLoad(TokenBagShape tokenBagShape)
        {
            if (this.CanIncreaseWidth && tokenBagShape.MaxWidth > this.MaxWidth)
            {
                int increment = tokenBagShape.MaxWidth - this.MaxWidth;
                this.IncreaseWidth(increment);
            }
            
            if (this.CanIncreaseHeight && tokenBagShape.MaxHeight > this.MaxHeight)
            {
                int increment = tokenBagShape.MaxHeight - this.MaxHeight;
                this.IncreaseHeight(increment);
            }
        }

        public abstract bool IncreaseWidth(int numColumns);
        public abstract bool DecreaseWidth(int numColumns);
        
        public abstract bool IncreaseHeight(int numRows);
        public abstract bool DecreaseHeight(int numRows);
        
        // PROTECTED METHODS: ---------------------------------------------------------------------

        protected void ExecuteEventChange()
        {
            this.EventChange?.Invoke();
        }
    }
}