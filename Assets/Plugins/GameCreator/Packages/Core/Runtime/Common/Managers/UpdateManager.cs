using System;
using System.Collections.Generic;
using UnityEngine;

namespace GameCreator.Runtime.Common
{
    [AddComponentMenu("")]
    public class UpdateManager : Singleton<UpdateManager>
    {
        // MEMBERS: -------------------------------------------------------------------------------

        [NonSerialized] private Dictionary<int, TUpdateRegistry> m_Updates;
        
        // INITIALIZE METHODS: --------------------------------------------------------------------

        protected override void OnCreate()
        {
            base.OnCreate();
            this.m_Updates = new Dictionary<int, TUpdateRegistry>();
        }
        
        // PUBLIC METHODS: ------------------------------------------------------------------------
        
        public static void SubscribeUpdate(Action action, int order)
        {
            Instance.RequireRegistry(order).SubscribeUpdate(action);
        }
        
        public static void SubscribeLateUpdate(Action action, int order)
        {
            Instance.RequireRegistry(order).SubscribeLateUpdate(action);
        }
        
        public static void SubscribeFixedUpdate(Action action, int order)
        {
            Instance.RequireRegistry(order).SubscribeFixedUpdate(action);
        }

        public static void UnsubscribeUpdate(Action action, int order)
        {
            if (Instance.m_Updates.TryGetValue(order, out TUpdateRegistry update) == false) return;
            update.UnsubscribeUpdate(action);
        }
        
        public static void UnsubscribeLateUpdate(Action action, int order)
        {
            if (Instance.m_Updates.TryGetValue(order, out TUpdateRegistry update) == false) return;
            update.UnsubscribeLateUpdate(action);
        }
        
        public static void UnsubscribeFixedUpdate(Action action, int order)
        {
            if (Instance.m_Updates.TryGetValue(order, out TUpdateRegistry update) == false) return;
            update.UnsubscribeFixedUpdate(action);
        }

        // PRIVATE METHODS: -----------------------------------------------------------------------

        private TUpdateRegistry RequireRegistry(int order)
        {
            if (this.m_Updates.TryGetValue(order, out TUpdateRegistry update) == false)
            {
                this.m_Updates.Add(order, null);
            }

            if (update == null)
            {
                Type type = order switch
                {
                    ApplicationManager.EXECUTION_ORDER_DEFAULT => typeof(UpdateRegistryDefault),
                    ApplicationManager.EXECUTION_ORDER_DEFAULT_LATER => typeof(UpdateRegistryDefaultLater),
                    ApplicationManager.EXECUTION_ORDER_DEFAULT_EARLIER => typeof(UpdateRegistryDefaultEarlier),
                    ApplicationManager.EXECUTION_ORDER_FIRST => typeof(UpdateRegistryFirst),
                    ApplicationManager.EXECUTION_ORDER_FIRST_LATER => typeof(UpdateRegistryFirstLater),
                    ApplicationManager.EXECUTION_ORDER_FIRST_EARLIER => typeof(UpdateRegistryFirstEarlier),
                    ApplicationManager.EXECUTION_ORDER_LAST => typeof(UpdateRegistryLast),
                    ApplicationManager.EXECUTION_ORDER_LAST_LATER => typeof(UpdateRegistryLastLater),
                    ApplicationManager.EXECUTION_ORDER_LAST_EARLIER => typeof(UpdateRegistryLastEarlier),
                    _ => throw new ArgumentException($"Invalid Execution Order {order}")
                };
                
                update = (TUpdateRegistry) this.gameObject.Require(type);
                this.m_Updates[order] = update;
            }

            if (!update.enabled)
            {
                update.enabled = true;
            }

            return update;
        }
        
        ///////////////////////////////////////////////////////////////////////////////////////////
        // COMPONENTS: ----------------------------------------------------------------------------
        
        [AddComponentMenu("")]
        public abstract class TUpdateRegistry : MonoBehaviour
        {
            private event Action EventUpdate;
            private event Action EventLateUpdate;
            private event Action EventFixedUpdate;

            public void SubscribeUpdate(Action action) => this.EventUpdate += action;
            public void SubscribeLateUpdate(Action action) => this.EventLateUpdate += action;
            public void SubscribeFixedUpdate(Action action) => this.EventFixedUpdate += action;
            
            public void UnsubscribeUpdate(Action action)
            {
                this.EventUpdate -= action;
                this.DestroyUpdateRegistry();
            }

            public void UnsubscribeLateUpdate(Action action)
            {
                this.EventLateUpdate -= action;
                this.DestroyUpdateRegistry();
            }

            public void UnsubscribeFixedUpdate(Action action)
            {
                this.EventFixedUpdate -= action;
                this.DestroyUpdateRegistry();
            }
            
            private void DestroyUpdateRegistry()
            {
                if (this.EventUpdate != null) return;
                if (this.EventLateUpdate != null) return;
                if (this.EventFixedUpdate != null) return;
                
                this.enabled = false;
            }

            protected void OnUpdate()
            {
                this.EventUpdate?.Invoke();
            }
            
            protected void OnLateUpdate()
            {
                this.EventLateUpdate?.Invoke();
            }
            
            protected void OnFixedUpdate()
            {
                this.EventFixedUpdate?.Invoke();
            }
        }
        
        [AddComponentMenu("")] [DefaultExecutionOrder(ApplicationManager.EXECUTION_ORDER_DEFAULT)]
        public class UpdateRegistryDefault : TUpdateRegistry
        {
            private void Update() => this.OnUpdate();
            private void LateUpdate() => this.OnLateUpdate();
            private void FixedUpdate() => this.OnFixedUpdate();
        }
        
        [AddComponentMenu("")] [DefaultExecutionOrder(ApplicationManager.EXECUTION_ORDER_DEFAULT_LATER)]
        public class UpdateRegistryDefaultLater : TUpdateRegistry
        {
            private void Update() => this.OnUpdate();
            private void LateUpdate() => this.OnLateUpdate();
            private void FixedUpdate() => this.OnFixedUpdate();
        }
        
        [AddComponentMenu("")] [DefaultExecutionOrder(ApplicationManager.EXECUTION_ORDER_DEFAULT_EARLIER)]
        public class UpdateRegistryDefaultEarlier : TUpdateRegistry
        {
            private void Update() => this.OnUpdate();
            private void LateUpdate() => this.OnLateUpdate();
            private void FixedUpdate() => this.OnFixedUpdate();
        }
        
        [AddComponentMenu("")] [DefaultExecutionOrder(ApplicationManager.EXECUTION_ORDER_FIRST)]
        public class UpdateRegistryFirst : TUpdateRegistry
        {
            private void Update() => this.OnUpdate();
            private void LateUpdate() => this.OnLateUpdate();
            private void FixedUpdate() => this.OnFixedUpdate();
        }
        
        [AddComponentMenu("")] [DefaultExecutionOrder(ApplicationManager.EXECUTION_ORDER_FIRST_LATER)]
        public class UpdateRegistryFirstLater : TUpdateRegistry
        {
            private void Update() => this.OnUpdate();
            private void LateUpdate() => this.OnLateUpdate();
            private void FixedUpdate() => this.OnFixedUpdate();
        }
        
        [AddComponentMenu("")] [DefaultExecutionOrder(ApplicationManager.EXECUTION_ORDER_FIRST_EARLIER)]
        public class UpdateRegistryFirstEarlier : TUpdateRegistry
        {
            private void Update() => this.OnUpdate();
            private void LateUpdate() => this.OnLateUpdate();
            private void FixedUpdate() => this.OnFixedUpdate();
        }
        
        [AddComponentMenu("")] [DefaultExecutionOrder(ApplicationManager.EXECUTION_ORDER_LAST)]
        public class UpdateRegistryLast : TUpdateRegistry
        {
            private void Update() => this.OnUpdate();
            private void LateUpdate() => this.OnLateUpdate();
            private void FixedUpdate() => this.OnFixedUpdate();
        }
        
        [AddComponentMenu("")] [DefaultExecutionOrder(ApplicationManager.EXECUTION_ORDER_LAST_LATER)]
        public class UpdateRegistryLastLater : TUpdateRegistry
        {
            private void Update() => this.OnUpdate();
            private void LateUpdate() => this.OnLateUpdate();
            private void FixedUpdate() => this.OnFixedUpdate();
        }
        
        [AddComponentMenu("")] [DefaultExecutionOrder(ApplicationManager.EXECUTION_ORDER_LAST_EARLIER)]
        public class UpdateRegistryLastEarlier : TUpdateRegistry
        {
            private void Update() => this.OnUpdate();
            private void LateUpdate() => this.OnLateUpdate();
            private void FixedUpdate() => this.OnFixedUpdate();
        }
    }
}