using System;
using UniGLTF;
using UnityEngine;
using UniGLTF.SpringBoneJobs.InputPorts;
using System.Threading.Tasks;

namespace UniVRM10
{
    /// <summary>
    /// FastSpringbone(job + singleton) で動作します。
    /// FastSpringBoneService に登録します。
    /// FastSpringBoneService.LateUpdate[DefaultExecutionOrder(11010)] で動作します。
    /// </summary>
    public class Vrm10FastSpringboneRuntime : IVrm10SpringBoneRuntime
    {
        private Vrm10Instance m_instance;
        private FastSpringBones.FastSpringBoneService m_fastSpringBoneService;
        private FastSpringBoneBuffer m_fastSpringBoneBuffer;

        public Vector3 ExternalForce
        {
            get => m_fastSpringBoneBuffer.ExternalForce;
            set => m_fastSpringBoneBuffer.ExternalForce = value;
        }
        public bool IsSpringBoneEnabled
        {
            get => m_fastSpringBoneBuffer.IsSpringBoneEnabled;
            set => m_fastSpringBoneBuffer.IsSpringBoneEnabled = value;
        }

        public float DeltaTime => throw new NotImplementedException();

        public async Task InitializeAsync(Vrm10Instance instance, IAwaitCaller awaitCaller)
        {
            m_fastSpringBoneService = FastSpringBones.FastSpringBoneService.Instance;
            m_instance = instance;

            // NOTE: FastSpringBoneService は UnitTest などでは動作しない
            if (Application.isPlaying)
            {
                await ConstructSpringBoneAsync(awaitCaller);
            }
        }

        public void Dispose()
        {
            m_fastSpringBoneService.BufferCombiner.Unregister(m_fastSpringBoneBuffer);
            m_fastSpringBoneBuffer.Dispose();
        }

        /// <summary>
        /// このVRMに紐づくSpringBone関連のバッファを再構築する
        /// ランタイム実行時にSpringBoneに対して変更を行いたいときは、このメソッドを明示的に呼ぶ必要がある
        /// </summary>
        public bool ReconstructSpringBone()
        {
            // new ImmediateCaller() により即時実行して結果を得る。
            // スパイクは許容する。
            var task = ConstructSpringBoneAsync(new ImmediateCaller());
            return task.Result;
        }

        /// <summary>
        /// 多重実行防止。
        /// m_building は ConstructSpringBoneAsync 専用。他で使う場合は注意。
        /// </summary>
        private bool m_building = false;

        /// <returns>ConstructSpringBoneAsync がすでに実行中の場合は中止して false で戻る</returns>
        private async Task<bool> ConstructSpringBoneAsync(IAwaitCaller awaitCaller)
        {
            if (m_building)
            {
                return false;
            }
            m_building = true;

            // 登録削除
            if (m_fastSpringBoneBuffer != null)
            {
                m_fastSpringBoneService.BufferCombiner.Unregister(m_fastSpringBoneBuffer);
            }

            m_fastSpringBoneBuffer = await FastSpringBoneBufferFactory.ConstructSpringBoneAsync(new ImmediateCaller(), m_instance, m_fastSpringBoneBuffer);

            // 登録
            m_fastSpringBoneService.BufferCombiner.Register(m_fastSpringBoneBuffer);

            m_building = false;
            return true;
        }

        public void RestoreInitialTransform()
        {
            // Spring の joint に対応する transform の回転を初期状態
            var instance = m_instance.GetComponent<RuntimeGltfInstance>();
            for (int i = 0; i < m_fastSpringBoneBuffer.Transforms.Length; ++i)
            {
                var transform = m_fastSpringBoneBuffer.Transforms[i];
                transform.localRotation = instance.InitialTransformStates[transform].LocalRotation;
            }

            // TODO: jobs のバッファにも反映する必要あり
        }

        public void Process()
        {
            // FastSpringBoneService が実行するので何もしない
        }
    }
}