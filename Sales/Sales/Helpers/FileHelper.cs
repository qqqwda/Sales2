using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Sales.Helpers
{
    public class FileHelper
    {
        public static byte[] ReadFully(Stream input) //Convierte stream a arreglo de bytes
        {
            using (MemoryStream ms = new MemoryStream())
            {
                input.CopyTo(ms);
                return ms.ToArray();
            }
        }

    }
}
