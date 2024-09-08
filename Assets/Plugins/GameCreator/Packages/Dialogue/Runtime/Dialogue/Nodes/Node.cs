using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using GameCreator.Runtime.Characters;
using GameCreator.Runtime.Common;
using GameCreator.Runtime.Common.Audio;
using GameCreator.Runtime.VisualScripting;
using UnityEngine;
using UnityEngine.Playables;

namespace GameCreator.Runtime.Dialogue
{
    [Serializable]
    public class Node
    {
        // EXPOSED MEMBERS: -----------------------------------------------------------------------

        [SerializeReference] private TNodeType m_NodeType = new NodeTypeText();

        [SerializeField] private RunConditionsList m_Conditions = new RunConditionsList();

        [SerializeField] private NodeText m_Text = new NodeText();
        [SerializeField] private PropertyGetAudio m_Audio = GetAudioNone.Create;

        [SerializeField] private Acting m_Acting = new Acting();
        
        [SerializeField] private AnimationClip m_Animation;
        [SerializeField] private NodeAnimation m_AnimationData = new NodeAnimation();
        
        [SerializeField] private NodeSequence m_Sequence = new NodeSequence(new Track[] 
        {
            new TrackDefault()
        });
        
        [SerializeField] private RunInstructionsList m_OnStart = new RunInstructionsList();
        [SerializeField] private RunInstructionsList m_OnFinish = new RunInstructionsList();

        [SerializeField] private NodeDuration m_Duration = NodeDuration.UntilInteraction;
        [SerializeField] private PropertyGetDecimal m_Timeout = GetDecimalDecimal.Create(3f);

        [SerializeField] private IdString m_Tag = IdString.EMPTY;
        [SerializeField] private NodeJump m_Jump = NodeJump.Continue();
        
        // MEMBERS: -------------------------------------------------------------------------------
        
        [NonSerialized] private bool m_Continue;
        [NonSerialized] private bool m_RunTypewriter;
        
        [NonSerialized] private float m_TypewriterLength;
        [NonSerialized] private float m_AudioLength;
        [NonSerialized] private float m_AnimationLength;
        
        // PROPERTIES: ----------------------------------------------------------------------------

        public TNodeType NodeType
        {
            get => this.m_NodeType;
            set => this.m_NodeType = value;
        }

        public string Text => this.m_Text.Value;
        public Actor Actor => this.m_Acting.Actor;

        public int Expression => this.m_Acting.Expression;
        public PortraitMode Portrait => this.m_Acting.Portrait;

        public AnimationClip Animation => this.m_Animation;
        public NodeDuration Duration => this.m_Duration;
        
        public IdString Tag => this.m_Tag;

        public NodeJump Jump => this.m_Jump;
        
        // EVENTS: --------------------------------------------------------------------------------

        public event Action<int> EventStart;
        
        public event Action<int> EventStartOnStart;
        public event Action<int> EventFinishOnStart;
        
        public event Action<int> EventStartText;
        public event Action<int> EventFinishText;
        
        public event Action<int> EventStartType;
        public event Action<int> EventFinishType;
        
        public event Action<int> EventStartOnFinish;
        public event Action<int> EventFinishOnFinish;
        
        public event Action<int> EventFinish;

        public event Action<int> EventStartChoice;
        public event Action<int> EventFinishChoice;

        // CONSTRUCTOR: ---------------------------------------------------------------------------

        public Node()
        { }

        public Node(string text) : this()
        {
            this.m_Text = new NodeText(text);
        }
        
        // GETTERS: -------------------------------------------------------------------------------

        public string GetText(Args args) => this.m_Text.Get(args);

        public AudioClip GetAudio(Args args) => this.m_Audio.Get(args);
        
        // PUBLIC METHODS: ------------------------------------------------------------------------

        public async Task<NodeJump> Run(int id, Story story, Args args)
        {
            this.m_Continue = false;
            this.m_RunTypewriter = true;
            
            this.m_TypewriterLength = 0f;
            this.m_AudioLength = 0f;
            this.m_AnimationLength = 0f;

            this.m_Text.Init(args);

            this.EventStart?.Invoke(id);
            
            this.EventStartOnStart?.Invoke(id);
            await this.RunOnStart(args);
            this.EventFinishOnStart?.Invoke(id);
        
            GameObject speaker = story.Content.GetTargetFromActor(this.Actor, args);
            AudioClip audio = this.GetAudio(args);

            if (audio != null)
            {
                AudioConfigSpeech config = speaker != null
                    ? AudioConfigSpeech.Create(1f, SpatialBlending.Spatial, speaker)
                    : AudioConfigSpeech.Create(1f, SpatialBlending.None, null);

                _ = AudioManager.Instance.Speech.Play(audio, config, args);
                if (this.m_Duration == NodeDuration.Audio) this.m_AudioLength = audio.length;
            }

            if (this.m_Animation != null)
            {
                Character character = speaker.Get<Character>();
                if (character != null)
                {
                    ConfigGesture configGesture = new ConfigGesture(
                        0f, this.m_Animation.length, 1f,
                        this.m_AnimationData.UseRootMotion, 
                        this.m_AnimationData.TransitionIn,
                        this.m_AnimationData.TransitionOut
                    );
                    
                    _ = character.Gestures.CrossFade(
                        this.m_Animation,
                        this.m_AnimationData.AvatarMask,
                        this.m_AnimationData.BlendMode,
                        configGesture,
                        true
                    );
                }
                else
                {
                    Animator animator = speaker.Get<Animator>();
                    if (animator != null) AnimationPlayableUtilities.PlayClip(
                        animator, this.m_Animation, 
                        out PlayableGraph _
                    );
                }
                
                _ = m_Sequence.Run(story.Time, this.m_Animation, args);

                if (this.m_Duration == NodeDuration.Animation)
                {
                    this.m_AnimationLength = this.m_Animation.length;
                }
            }

            if (this.Actor != null)
            {
                this.m_TypewriterLength = this.Actor.Typewriter.GetDuration(this.Text);
            }

            Typewriter typewriter = this.Actor != null ? this.Actor.Typewriter : null;
            
            TimeMode time = story.Time;
            float startTime = time.Time;

            AudioClip gibberishClip = null;
            AudioConfigSoundUI gibberishConfig = AudioConfigSoundUI.Create(
                1f, 
                typewriter?.Pitch ?? new Vector2(1f, 1f)
            );

            this.EventStartText?.Invoke(id);
            while (!this.CanContinue(startTime, time, args) && !story.IsCanceled)
            {
                if (startTime + this.m_TypewriterLength > time.Time && this.m_RunTypewriter)
                {
                    if (gibberishClip == null)
                    {
                        gibberishClip = typewriter?.GetGibberish(args);
                    }

                    if (!AudioManager.Instance.UserInterface.IsPlaying(gibberishClip))
                    {
                        _ = AudioManager.Instance.UserInterface.Play(
                            gibberishClip, 
                            gibberishConfig, 
                            args
                        );
                    }
                }

                await Task.Yield();
            }

            if (gibberishClip != null)
            {
                _ = AudioManager.Instance.Speech.Stop(gibberishClip, 0.25f);
            }
            this.EventFinishText?.Invoke(id);

            this.m_NodeType.EventStartChoice -= this.OnStartChoice;
            this.m_NodeType.EventFinishChoice -= this.OnFinishChoice;
            
            this.m_NodeType.EventStartChoice += this.OnStartChoice;
            this.m_NodeType.EventFinishChoice += this.OnFinishChoice;
            
            this.EventStartType?.Invoke(id);
            
            await this.m_NodeType.Run(id, story, args);
            story.Visits.Nodes.Add(id);
            story.Visits.Tags.Add(this.m_Tag);

            this.EventFinishType?.Invoke(id);
            
            this.m_NodeType.EventStartChoice -= this.OnStartChoice;
            this.m_NodeType.EventFinishChoice -= this.OnFinishChoice;

            if (audio != null) _ = AudioManager.Instance.Speech.Stop(audio, 0f);
            
            if (this.m_Animation != null)
            {
                Character character = speaker.Get<Character>();
                if (character != null) character.Gestures.Stop(0f, 0f);
                if (this.m_Sequence.IsRunning) this.m_Sequence.Cancel(args);
            }
            
            this.EventStartOnFinish?.Invoke(id);
            await this.RunOnFinish(args);
            this.EventFinishOnFinish?.Invoke(id);
            
            this.EventFinish?.Invoke(id);
            return this.Jump;
        }

        public List<int> GetNext(int id, Story story, Args args)
        {
            return this.m_NodeType.GetNext(id, story, args);
        }

        public bool CanRun(Args args)
        {
            RunnerConfig config = new RunnerConfig
            {
                Name = "Can Run Node",
                Location = new RunnerLocationParent(args.Self.transform)
            };

            return this.m_Conditions.Check(args, config);
        }

        public void Continue()
        {
            this.m_Continue = true;
        }
        
        // INTERNAL METHODS: ----------------------------------------------------------------------

        internal void StopTypewriter()
        {
            this.m_RunTypewriter = false;
        }

        // PRIVATE METHODS: -----------------------------------------------------------------------
        
        private bool CanContinue(float startTime, TimeMode time, Args args)
        {
            return this.m_Duration switch
            {
                NodeDuration.UntilInteraction => 
                    this.m_Continue,
                
                NodeDuration.Timeout => 
                    time.Time > startTime + this.m_TypewriterLength + this.m_Timeout.Get(args),
                
                NodeDuration.Audio => 
                    time.Time > startTime + this.m_AudioLength,
                
                NodeDuration.Animation => 
                    time.Time > startTime + this.m_AnimationLength,
                
                _ => throw new ArgumentOutOfRangeException()
            };
        }

        private void OnStartChoice(int nodeId) => this.EventStartChoice?.Invoke(nodeId);
        private void OnFinishChoice(int nodeId) => this.EventFinishChoice?.Invoke(nodeId);

        // PRIVATE VISUAL SCRIPTING METHODS: ------------------------------------------------------ 
        
        private async Task RunOnStart(Args args)
        {
            RunnerConfig config = new RunnerConfig
            {
                Name = "On Start Node",
                Location = new RunnerLocationParent(args.ComponentFromSelf<Transform>())
            };
            
            await this.m_OnStart.Run(args.Clone, config);
        }
        
        private async Task RunOnFinish(Args args)
        {
            RunnerConfig config = new RunnerConfig
            {
                Name = "On Finish Node",
                Location = new RunnerLocationParent(args.ComponentFromSelf<Transform>())
            };
            
            await this.m_OnFinish.Run(args.Clone, config);
        }

        // TO STRING: -----------------------------------------------------------------------------

        public override string ToString() => this.m_Text.ToString();
    }
}