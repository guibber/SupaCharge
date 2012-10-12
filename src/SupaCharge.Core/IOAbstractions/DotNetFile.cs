using System.IO;

namespace SupaCharge.Core.IOAbstractions {
  public class DotNetFile : IFile {
    public FileStream Open(string path, FileMode mode) {
      return File.Open(path, mode);
    }
  }
}