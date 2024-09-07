using System;
using GameCreator.Runtime.Common;
using UnityEngine;

namespace GameCreator.Runtime.Characters
{
    [Title("Character Bone Position")]
    [Category("Characters/Character Bone Position")]
    
    [Image(typeof(IconBoneSolid), ColorTheme.Type.Yellow)]
    [Description("The bone position of a Character game object")]

    [Serializable]
    public class GetPositionCharacterBone : PropertyTypeGetPosition
    {
        [SerializeField] private PropertyGetGameObject m_Character = GetGameObjectPlayer.Create();
        
        [SerializeField] private Bone m_Bone = new Bone(HumanBodyBones.RightHand); 

        public GetPositionCharacterBone()
        { }

        public GetPositionCharacterBone(PropertyGetGameObject character, Bone bone)
        {
            this.m_Character = character;
            this.m_Bone = bone;
        }

        public override Vector3 Get(Args args)
        {
            Character character = this.m_Character.Get<Character>(args);
            if (character == null || character.Animim?.Animator == null) return default;

            GameObject bone = this.m_Bone.Get(character.Animim?.Animator);
            return bone != null ? bone.transform.position : default;
        }

        public static PropertyGetPosition Create()
        {
            return new PropertyGetPosition(
                new GetPositionCharacterBone()
            );
        }

        public static PropertyGetPosition Create(PropertyGetGameObject character, Bone bone)
        {
            return new PropertyGetPosition(
                new GetPositionCharacterBone(character, bone)
            );
        }

        public override Vector3 EditorValue
        {
            get
            {
                GameObject gameObject = this.m_Character.EditorValue;
                if (gameObject == null) return default;

                Character character = gameObject.GetComponent<Character>();
                if (character == null) return default;

                Animator animator = character.Animim?.Animator;
                if (animator == null) return default;

                Transform bone = this.m_Bone.GetTransform(animator);
                return bone != null ? bone.position : default;
            }
        }

        public override string String => $"{this.m_Character}/{this.m_Bone}";
    }
}