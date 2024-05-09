using LocalUtilities.FileUtilities;
using LocalUtilities.SimpleScript.Data;
using LocalUtilities.SimpleScript.Serialization;
using LocalUtilities.StringUtilities;
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
            var data = new FormData() { Datas = [new(), new()] };
            //var a = new FormDataSerialization() { Source = data }.SaveToFile();
            var b = new FormDataSerialization().LoadFromFile(out var m);

            Application.Run(new Form1());
        }
    }

    public class FormData
    {
        public Size MinimumSize { get; set; }

        public virtual Size Size { get; set; }

        public virtual Point Location { get; set; }

        public virtual FormWindowState WindowState { get; set; } = FormWindowState.Normal;

        public virtual int Padding { get; set; } = 12;

        public List<FormData> Datas { get; set; } = [];
    }

    public class FormDataSerialization : SsSerialization<FormData>
    {
        public FormDataSerialization() : base(new())
        {
            OnSerialize += FormData_Serialize;
            OnDeserialize += FormData_Deserialize;
        }

        public override string LocalName => nameof(FormData);

        private void FormData_Serialize(SsSerializer serializer)
        {
            serializer.WriteTag(nameof(Source.MinimumSize), (Source.MinimumSize.Width, Source.MinimumSize.Height).ToArrayString());
            serializer.WriteTag(nameof(Source.Location), (Source.Location.X, Source.Location.Y).ToArrayString());
            serializer.WriteTag(nameof(Source.WindowState), Source.WindowState.ToString());
            serializer.WriteTag(nameof(Source.Padding), Source.Padding.ToString());
            Source.Datas.Serialize(serializer, new FormDataSerialization());
        }

        private void FormData_Deserialize(Token token)
        {
            if (token is TagValues tagValues)
            {
                if (token.Name is nameof(Source.MinimumSize))
                    Source.MinimumSize = tagValues.Tag.ToSize(Source.MinimumSize);
                else if (token.Name is nameof(Source.Location))
                    Source.Location = tagValues.Tag.ToPoint(Source.Location);
                else if (token.Name is nameof(Source.WindowState))
                    Source.WindowState = tagValues.Tag.ToEnum<FormWindowState>();
                else if (token.Name is nameof(Source.Padding))
                    Source.Padding = tagValues.Tag.ToInt(Source.Padding);
            }
            else if (token is Scope scope)
            {
                if (token.Name is nameof(FormData))
                    Source.Datas.Add(SsDeserializeTool.Deserialize(new FormDataSerialization(), scope));
            }
        }
    }
}