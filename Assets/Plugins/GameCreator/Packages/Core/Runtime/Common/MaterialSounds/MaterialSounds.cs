using System;
using System.Collections.Generic;
using GameCreator.Runtime.Common.Audio;
using UnityEngine;

namespace GameCreator.Runtime.Common
{
    [Serializable]
    public class MaterialSounds
    {
        private static readonly Vector2 PITCH_VARIATION = new Vector2(-0.1f, 0.1f);
        private static readonly Vector2 PITCH_LERP_WEIGHT = new Vector2(0.5f, 1f);

        private static readonly Renderer[] RENDERERS = new Renderer[10];

        // EXPOSED MEMBERS: -----------------------------------------------------------------------
        
        [SerializeField] private MaterialSoundsAsset m_SoundsAsset;
        
        // MEMBERS: -------------------------------------------------------------------------------

        [NonSerialized] private Dictionary<Texture, MaterialSoundTexture> m_LookupTable;
        
        // PROPERTIES: ----------------------------------------------------------------------------

        public LayerMask LayerMask => this.m_SoundsAsset != null
            ? this.m_SoundsAsset.MaterialSounds.LayerMask
            : 0;
        
        // PUBLIC STATIC METHODS: -----------------------------------------------------------------

        public static void Play(Args args, Vector3 point, Vector3 normal, GameObject hit, MaterialSoundsAsset materialSounds, float yaw)
        {
            if (materialSounds == null) return;
            if (hit == null) return;
            
            switch (hit.Get<Collider>() is TerrainCollider)
            {
                case true: PlayTerrain(args, point, normal, hit, materialSounds, yaw); break;
                case false: PlayMesh(args, point, normal, hit, materialSounds, yaw); break;
            }
        }
        
        // PRIVATE STATIC METHODS: ----------------------------------------------------------------

        private static void PlayTerrain(
            Args args,
            Vector3 point,
            Vector3 normal,
            GameObject hit,
            MaterialSoundsAsset materialSounds,
            float yaw)
        {
            Terrain terrain = hit.Get<Terrain>();
            TerrainData terrainData = terrain.terrainData;

            float[] mixture = GetTerrainWeights(
                point, 
                terrainData, 
                terrain.GetPosition()
            );

            Texture maxTexture = null;
            float maxWeight = 0f;
            
            for (int i = 0; i < terrainData.alphamapLayers; i++)
            {
                float weight = i < mixture.Length ? mixture[i] : 0f;
                Texture texture = terrainData.terrainLayers[i].diffuseTexture;
                
                if (weight > maxWeight)
                {
                    maxTexture = texture;
                    maxWeight = weight;
                }
                
                PlaySound(args, texture, materialSounds);
            }

            if (maxTexture != null)
            {
                PlayImpact(point, normal, maxTexture, materialSounds, yaw);
            }
        }

        private static void PlayMesh(
            Args args,
            Vector3 point,
            Vector3 normal,
            GameObject hit,
            MaterialSoundsAsset materialSounds,
            float yaw)
        {
            int renderersCount = 1;
            RENDERERS[0] = hit.Get<Renderer>();
            
            if (RENDERERS[0] == null)
            {
                LODGroup lodGroup = hit.Get<LODGroup>();
                if (lodGroup != null && lodGroup.lodCount > 0)
                {
                    Renderer[] renderers = lodGroup.GetLODs()[0].renderers;
                    renderersCount = Mathf.Min(RENDERERS.Length, renderers.Length);
                    for (int i = 0; i < renderers.Length; ++i)
                    {
                        RENDERERS[i] = renderers[i];
                    }
                }   
            }

            for (int i = 0; i < renderersCount; ++i)
            {
                Renderer renderer = RENDERERS[i];
                if (renderer == null) continue;

                foreach (Material material in renderer.sharedMaterials)
                {
                    if (material.HasTexture(materialSounds.TextureID) == false) continue;
                    
                    Texture texture = material.GetTexture(materialSounds.TextureID);
                    if (texture == null) continue;
                    
                    PlaySound(args, texture, materialSounds);
                    PlayImpact(point, normal, texture, materialSounds, yaw);
                }
            }
        }

        // PUBLIC METHODS: ------------------------------------------------------------------------

        internal void OnStartup()
        {
            this.SetupSoundsTable();
        }

        internal void ChangeSoundsAsset(MaterialSoundsAsset materialSoundsAsset)
        {
            this.m_SoundsAsset = materialSoundsAsset;
            if (!Application.isPlaying) return;
            
            this.SetupSoundsTable();
        }
        
        public void Play(Transform transform, RaycastHit hit, float speed, Args args, float yaw)
        {
            if (this.m_SoundsAsset == null) return;

            switch (hit.collider is TerrainCollider)
            {
                case true: this.PlayTerrain(transform, args, hit, speed, yaw); break;
                case false: this.PlayMesh(transform, args, hit, this.m_SoundsAsset, speed, yaw); break;
            }
        }

        // MATERIAL GROUND TYPE: ------------------------------------------------------------------

        private void PlayTerrain(
            Transform transform,
            Args args,
            RaycastHit hit,
            float speed,
            float yaw)
        {
            Terrain terrain = hit.collider.Get<Terrain>();
            TerrainData terrainData = terrain.terrainData;

            float[] mixture = GetTerrainWeights(
                hit.point, 
                terrainData, 
                terrain.GetPosition()
            );

            Texture maxTexture = null;
            float maxWeight = 0f;
            
            for (int i = 0; i < terrainData.alphamapLayers; i++)
            {
                float weight = i < mixture.Length ? mixture[i] : 0f;
                Texture texture = terrainData.terrainLayers[i].diffuseTexture;
                
                if (weight > maxWeight)
                {
                    maxTexture = texture;
                    maxWeight = weight;
                }
                
                this.PlaySound(texture, weight, speed, transform, args);
            }

            if (maxTexture != null)
            {
                this.PlayImpact(maxTexture, transform, hit, yaw);
            }
        }

        private void PlayMesh(
            Transform transform,
            Args args,
            RaycastHit hit,
            MaterialSoundsAsset materialSounds,
            float speed,
            float yaw)
        {
            int renderersCount = 1;
            RENDERERS[0] = hit.collider.Get<Renderer>();
            
            if (RENDERERS[0] == null)
            {
                LODGroup lodGroup = hit.collider.Get<LODGroup>();
                if (lodGroup != null && lodGroup.lodCount > 0)
                {
                    Renderer[] renderers = lodGroup.GetLODs()[0].renderers;
                    renderersCount = Mathf.Min(RENDERERS.Length, renderers.Length);
                    for (int i = 0; i < renderers.Length; ++i)
                    {
                        RENDERERS[i] = renderers[i];
                    }
                }   
            }

            for (int i = 0; i < renderersCount; ++i)
            {
                Renderer renderer = RENDERERS[i];
                if (renderer == null) continue;

                foreach (Material material in renderer.sharedMaterials)
                {
                    // if (material.HasTexture(materialSounds.TextureID) == false) continue;
                    
                    Texture texture = material.GetTexture(materialSounds.TextureID);
                    // if (texture == null) continue;
                    
                    this.PlaySound(texture, 1f, speed, transform, args);
                    this.PlayImpact(texture, transform, hit, yaw);
                }
            }
        }

        // PRIVATE METHODS: -----------------------------------------------------------------------

        private void SetupSoundsTable()
        {
            this.m_LookupTable = new Dictionary<Texture, MaterialSoundTexture>();
            if (this.m_SoundsAsset == null) return;
            
            foreach (MaterialSoundTexture materialSound in this.m_SoundsAsset.MaterialSounds.MaterialSounds)
            {
                Texture texture = materialSound.Texture;
                
                if (texture == null) continue;
                this.m_LookupTable[texture] = materialSound;
            }
        }
        
        private void PlaySound(Texture texture, float weight, float speed, Transform target, Args args)
        {
            // if (texture == null) return;

            IMaterialSound materialSound;
            AudioConfigSoundEffect config;

            float pitch = Mathf.Lerp(PITCH_LERP_WEIGHT.x, PITCH_LERP_WEIGHT.y, weight);

            if (texture != null && this.m_LookupTable.TryGetValue(texture, out MaterialSoundTexture material))
            {
                AudioClip audioClip = material.Audio;
                FMODAudio fmodAudio = material.FMODAudio;

                if (audioClip == null && fmodAudio == null) return;

                materialSound = material;

                config = AudioConfigSoundEffect.Create(
                    material.Volume * weight * speed,
                    new Vector2(pitch + PITCH_VARIATION.x, pitch + PITCH_VARIATION.y),
                    0f, TimeMode.UpdateMode.GameTime, SpatialBlending.Spatial,
                    target.gameObject
                );
            }
            else
            {
                materialSound = this.m_SoundsAsset.MaterialSounds.DefaultSounds;

                config = AudioConfigSoundEffect.Create(
                    materialSound.Volume * weight * speed,
                    new Vector2(pitch + PITCH_VARIATION.x, pitch + PITCH_VARIATION.y),
                    0f, TimeMode.UpdateMode.GameTime, SpatialBlending.Spatial,
                    target.gameObject
                );
            }

            if (config.Volume < float.Epsilon) return;
            
            if (materialSound.FMODAudio != null)
            {
                _ = AudioManager.Instance.SoundEffect.Play(materialSound.FMODAudio, config, args);
            }
            else if (materialSound.Audio != null)
            {
                _ = AudioManager.Instance.SoundEffect.Play(materialSound.Audio, config, args);
            }
        }
        
        private void PlayImpact(Texture texture, Transform transform, RaycastHit hit, float yaw)
        {
            if (texture == null) return;
            IMaterialSound materialSound;
            
            if (this.m_LookupTable.TryGetValue(texture, out MaterialSoundTexture material))
            {
                AudioClip audioClip = material.Audio;
                FMODAudio fmodAudio = material.FMODAudio;
                if (audioClip == null && fmodAudio == null) return;
                
                materialSound = material;
            }
            else
            {
                materialSound = this.m_SoundsAsset.MaterialSounds.DefaultSounds;
            }

            GameObject impact = materialSound?.Impact.Create(
                hit.point,
                Quaternion.FromToRotation(Vector3.up, hit.normal), 
                null
            );

            if (impact != null) impact.transform.localRotation *= Quaternion.Euler(0f, yaw, 0f);
        }
        
        // PRIVATE STATIC HELPER METHODS: ---------------------------------------------------------
        
        private static float[] GetTerrainWeights(Vector3 point, TerrainData data, Vector3 terrain)
        {
            float positionX = point.x - terrain.x;
            float positionZ = point.z - terrain.z;
            
            int mapX = (int)(positionX / data.size.x * data.alphamapWidth);
            int mapZ = (int)(positionZ / data.size.z * data.alphamapHeight);
            
            float[,,] alphaMaps = data.GetAlphamaps(mapX, mapZ, 1, 1);

            int textureCount = alphaMaps.GetUpperBound(2);
            float[] mixture = new float[textureCount + 1];

            for(int i = 0; i < mixture.Length; ++i) 
            {
                mixture[i] = alphaMaps[0, 0, i];
            }

            return mixture;
        }
        
        private static void PlaySound(Args args, Texture texture, MaterialSoundsAsset materialSounds)
        {
            if (texture == null) return;

            foreach (MaterialSoundTexture material in materialSounds.MaterialSounds.MaterialSounds)
            {
                if (material.Texture != texture) continue;
                if (material.Audio == null && material.FMODAudio == null) continue;
                
                AudioConfigSoundEffect config = AudioConfigSoundEffect.Create(
                    material.Volume,
                    new Vector2(1f + PITCH_VARIATION.x, 1f + PITCH_VARIATION.y),
                    0f, TimeMode.UpdateMode.GameTime, SpatialBlending.Spatial,
                    args.Self
                );
                
                if (config.Volume < float.Epsilon) return;
                if (material.FMODAudio != null)
                {
                    _ = AudioManager.Instance.SoundEffect.Play(material.FMODAudio, config, args);
                }
                else if (material.Audio != null)
                {
                    _ = AudioManager.Instance.SoundEffect.Play(material.Audio, config, args);
                }
                return;
            }

            AudioConfigSoundEffect configDefault = AudioConfigSoundEffect.Create(
                materialSounds.MaterialSounds.DefaultSounds.Volume,
                new Vector2(1f + PITCH_VARIATION.x, 1f + PITCH_VARIATION.y),
                0f, TimeMode.UpdateMode.GameTime, SpatialBlending.Spatial,
                args.Self
            );
            
            if (configDefault.Volume < float.Epsilon) return;
            if (materialSounds.MaterialSounds.DefaultSounds.FMODAudio != null)
            {
                _ = AudioManager.Instance.SoundEffect.Play(
                    materialSounds.MaterialSounds.DefaultSounds.FMODAudio, 
                    configDefault, 
                    args
                );
            }
            else if (materialSounds.MaterialSounds.DefaultSounds.Audio != null)
            {
                _ = AudioManager.Instance.SoundEffect.Play(
                    materialSounds.MaterialSounds.DefaultSounds.Audio, 
                    configDefault, 
                    args
                );
            }
        }
        
        private static void PlayImpact(Vector3 point, Vector3 normal, Texture texture, MaterialSoundsAsset materialSounds, float yaw)
        {
            if (texture == null) return;

            foreach (MaterialSoundTexture material in materialSounds.MaterialSounds.MaterialSounds)
            {
                AudioClip audioClip = material.Audio;
                FMODAudio fmodAudio = material.FMODAudio;
                if (audioClip == null && fmodAudio == null) return;
                
                material.Impact.Create(
                    point,
                    Quaternion.FromToRotation(Vector3.up, normal), 
                    null
                );
                
                return;
            }
            
            GameObject impact = materialSounds.MaterialSounds.DefaultSounds?.Impact?.Create(
                point,
                Quaternion.FromToRotation(Vector3.up, normal), 
                null
            );

            if (impact != null) impact.transform.localRotation *= Quaternion.Euler(0f, yaw, 0f);
        }
    }
}