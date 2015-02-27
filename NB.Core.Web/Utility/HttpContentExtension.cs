using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace NB.Core.Web.Utility
{
 public static class HttpContentExtensions
 {
      public static Task ReadAsFileAsync(this HttpContent content, string filename, bool overwrite)
      {
          string pathname = Path.GetFullPath(filename);
          if (!overwrite && File.Exists(filename))
          {
              throw new InvalidOperationException(string.Format("File {0} already exists.", pathname));
          }

          using (FileStream fileStream = new FileStream(pathname, FileMode.Create, FileAccess.Write, FileShare.None))
          {
              return content.CopyToAsync(fileStream).ContinueWith(
                  (copyTask) =>
                  {
                      fileStream.Close();
                  });
          }
      }
  }
}
