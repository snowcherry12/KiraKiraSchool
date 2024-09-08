using System;

namespace GameCreator.Runtime.Behavior
{
    public class ValueBehaviorTreeShuffle : IValue
    {
        // MEMBERS: -------------------------------------------------------------------------------

        [NonSerialized] private int[] m_List = Array.Empty<int>();

        // CONSTRUCTORS: --------------------------------------------------------------------------

        public ValueBehaviorTreeShuffle()
        { }
        
        // PUBLIC METHODS: ------------------------------------------------------------------------

        public int Get(int index)
        {
            return this.m_List != null 
                ? this.m_List[Math.Clamp(index, 0, this.m_List.Length)]
                : index;
        }
        
        public void Shuffle(int size)
        {
            if (this.m_List == null || this.m_List.Length != size)
            {
                this.m_List = new int[size];
                for (int i = 0; i < size; ++i)
                {
                    this.m_List[i] = i;
                }
            }

            while (size > 1)
            {
                int random = UnityEngine.Random.Range(0, size);
                size -= 1;
                
                (this.m_List[size], this.m_List[random]) = (this.m_List[random], this.m_List[size]);
            }
        }
        
        public void Restart()
        {
            this.m_List = Array.Empty<int>();
        }
    }
}