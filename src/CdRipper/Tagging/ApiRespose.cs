namespace CdRipper.Tagging
{
    public class ApiRespose
    {
        public bool IsFound { get; private set; }
        public string Json { get; private set; }

        public ApiRespose(bool isFound, string json)
        {
            Json = json;
            IsFound = isFound;
        }

        public override bool Equals(object obj)
        {
            var other = obj as ApiRespose;
            if (other == null) return false;

            return other.IsFound == this.IsFound && other.Json == this.Json;
        }
    }
}