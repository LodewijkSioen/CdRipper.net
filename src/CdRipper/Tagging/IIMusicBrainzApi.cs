namespace CdRipper.Tagging
{
    public interface IIMusicBrainzApi
    {
        string GetReleasesByDiscId(string discId);
        string GetRelease(string releaseId);
    }
}