namespace GameCreator.Runtime.Common
{
    public class FMODSettings : AssetRepository<FMODRepository>
    {
        public override IIcon Icon => new IconAudioMixer(ColorTheme.Type.TextLight);
        public override string Name => "FMOD";

        // public override int Priority => 0;

        // public override bool IsFullScreen => true;
    }
}