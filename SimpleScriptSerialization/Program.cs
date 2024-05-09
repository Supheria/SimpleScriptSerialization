using LocalUtilities.FileUtilities;
using LocalUtilities.Serializations;
using LocalUtilities.SimpleScript.Data;
using LocalUtilities.SimpleScript.Serialization;
using LocalUtilities.StringUtilities;
using LocalUtilities.UIUtilities;
using System.Text;

namespace SimpleScriptSerialization
{
    internal static class Program
    {
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            var data = new TestFormDataSerialization("ShitForm").LoadFromFile(out var m);
            new TestFormDataSerialization("ShitForm") { Source = data }.SaveToFile();

            Application.Run(new Form1());
        }
    }

    public class TestFormData : FormData
    {
        public override Size MinimumSize { get; set; }

        public List<TestFormData> Datas { get; set; } = [];
    }

    public class TestFormDataSerialization : FormDataSerialization<TestFormData>
    {
        public TestFormDataSerialization(string localName) : base(localName, new())
        {
            OnSerialize += FormData_Serialize;
            OnDeserialize += FormData_Deserialize;
        }

        private void FormData_Serialize(SsSerializer serializer)
        {
            Source.Datas.Serialize(serializer, new TestFormDataSerialization(LocalName));
        }

        private void FormData_Deserialize(Token token)
        {
            if (token is Scope scope)
            {
                if (scope.Name == LocalName)
                    Source.Datas.Add(SsDeserializeTool.Deserialize(new TestFormDataSerialization(LocalName), scope));
            }
        }
    }
}