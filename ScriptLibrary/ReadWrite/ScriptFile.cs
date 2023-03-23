using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Xml;
using System.Xml.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace ScriptLibrary.ReadWrite
{
    public class ScriptFile
    {
        public static ScriptBase Read(string path)
        {
            using (Stream stream = File.Open(path, FileMode.Open))
            {
                var binaryFormatter = new BinaryFormatter();
                return (ScriptBase)binaryFormatter.Deserialize(stream);
            }
        }

        public static void Write(string filePath, ScriptBase script) 
        {
            using (Stream stream = File.Open(filePath, FileMode.Create))
            {
                var binaryFormatter = new BinaryFormatter();
                binaryFormatter.Serialize(stream, script);
            }
        }
    }
}
