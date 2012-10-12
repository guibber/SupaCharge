namespace SupaCharge.Core.IOAbstractions {
  public interface IDirectory {
    string[] GetFiles(string path, string searchPattern);
    string GetCurrentDirectory();
  }
}