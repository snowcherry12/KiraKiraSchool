using System;
using GameCreator.Runtime.Characters;
using GameCreator.Runtime.Common;
using UnityEngine;
using UnityEngine.Serialization;

namespace GameCreator.Runtime.Inventory
{
    [Serializable]
    public class EquipmentRuntimeSlot
    {
        // MEMBERS: -------------------------------------------------------------------------------
        
        [SerializeField] private bool m_Override;
        [SerializeField] private HandleField m_OverrideHandle = new HandleField();
        
        [SerializeField] private IdString rootRuntimeItemIDEquipped = IdString.EMPTY;

        // PROPERTIES: ----------------------------------------------------------------------------

        public bool IsEquipped => !string.IsNullOrEmpty(this.rootRuntimeItemIDEquipped.String);
        public IdString RootRuntimeItemIDEquipped => this.rootRuntimeItemIDEquipped;

        // PUBLIC METHODS: ------------------------------------------------------------------------

        internal void Equip(Bag bag, HandleResult handle, RuntimeItem runtimeItem)
        {
            this.rootRuntimeItemIDEquipped = runtimeItem.RuntimeID;
            
            GameObject prefab = runtimeItem.Item.Equip.Prefab;
            if (prefab == null) return;
            
            Character character = bag.Wearer.Get<Character>();

            Args args = new Args(bag.gameObject, bag.Wearer);
            HandleResult overrideHandle = this.m_OverrideHandle.Get(args);
            
            Vector3 position = this.m_Override ? overrideHandle.LocalPosition : handle.LocalPosition;
            Quaternion rotation = this.m_Override ? overrideHandle.LocalRotation : handle.LocalRotation;
            
            IBone bone = this.m_Override ? overrideHandle.Bone : handle.Bone;
            
            if (character != null)
            {
                runtimeItem.PropInstance = character.Props.AttachPrefab(
                    bone, prefab, 
                    position, rotation
                );
            }
            else
            {
                Transform boneTransform = this.GetWearerBone(bag.Wearer, bone);
                if (boneTransform != null)
                {
                    if (runtimeItem.PropInstance != null)
                    {
                        UnityEngine.Object.Destroy(runtimeItem.PropInstance);
                    }

                    runtimeItem.PropInstance = UnityEngine.Object.Instantiate(
                        prefab, position, rotation, boneTransform
                    );
                }
            }

            if (runtimeItem.PropInstance == null) return;
            
            Prop prop = runtimeItem.PropInstance.Get<Prop>();
            if (prop != null) prop.Setup(runtimeItem);
        }
        
        internal void Unequip(Bag bag)
        {
            RuntimeItem runtimeItem = bag.Content.GetRuntimeItem(this.rootRuntimeItemIDEquipped);
            Character character = bag.Wearer.Get<Character>();

            if (runtimeItem != null)
            {
                if (character != null)
                {
                    character.Props.RemovePrefab(runtimeItem.Item.Equip.Prefab);
                }
                else
                {
                    if (runtimeItem.PropInstance != null)
                    {
                        UnityEngine.Object.Destroy(runtimeItem.PropInstance);
                    }
                }   
            }

            this.rootRuntimeItemIDEquipped = IdString.EMPTY;
        }

        // PRIVATE METHODS: -----------------------------------------------------------------------

        private Transform GetWearerBone(GameObject wearer, IBone bone)
        {
            if (wearer == null) return null;
            
            Character character = wearer.Get<Character>();
            Animator animator = character != null 
                ? character.Animim.Animator 
                : wearer.Get<Animator>();
        
            return animator != null 
                ? bone.GetTransform(animator) 
                : wearer.transform;
        }
    }
}