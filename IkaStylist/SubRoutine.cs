using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Deployment.Application;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows;
using System.IO;
using Microsoft.Win32;
using System.Xml;
using System.Data;

namespace IkaStylist
{
    /// <summary>配列の要素数を取得する    /// </summary>
    /// <typeparam name="T">要素数を求めるEnum型</typeparam>
    // 使い方：EnumExtension<xxx>.Length();
    public static class EnumExtension<T> where T : struct, IConvertible
    {
        public static int Length()
        {
            return Enum.GetValues(typeof(T)).Length;
        }
    }

    /// <summary>よく利用するメソッドをまとめたクラス</summary>
    public class SubRoutine
    {
        /// <summary>
        /// DataTableの内容をCSVファイルに保存する
        /// </summary>
        /// <param name="dt">CSVに変換するDataTable</param>
        /// <param name="csvPath">保存先のCSVファイルのパス</param>
        /// <param name="writeHeader">ヘッダを書き込む時はtrue。</param>
        public static void ConvertDataTableToCsv(
            DataTable dt, string csvPath, bool writeHeader)
        {
            //CSVファイルに書き込むときに使うEncoding
            System.Text.Encoding enc =
                System.Text.Encoding.GetEncoding("Shift_JIS");

            //書き込むファイルを開く
            System.IO.StreamWriter sr =
                new System.IO.StreamWriter(csvPath, false, enc);

            int colCount = dt.Columns.Count;
            int lastColIndex = colCount - 1;

            //ヘッダを書き込む
            if (writeHeader)
            {
                for (int i = 0; i < colCount; i++)
                {
                    //ヘッダの取得
                    string field = dt.Columns[i].Caption;
                    //"で囲む
                    field = EncloseDoubleQuotesIfNeed(field);
                    //フィールドを書き込む
                    sr.Write(field);
                    //カンマを書き込む
                    if (lastColIndex > i)
                    {
                        sr.Write(',');
                    }
                }
                //改行する
                sr.Write("\r\n");
            }

            //レコードを書き込む
            foreach (DataRow row in dt.Rows)
            {
                for (int i = 0; i < colCount; i++)
                {
                    //フィールドの取得
                    string field = row[i].ToString();
                    //"で囲む
                    field = EncloseDoubleQuotesIfNeed(field);
                    //フィールドを書き込む
                    sr.Write(field);
                    //カンマを書き込む
                    if (lastColIndex > i)
                    {
                        sr.Write(',');
                    }
                }
                //改行する
                sr.Write("\r\n");
            }

            //閉じる
            sr.Close();
        }

        /// <summary>
        /// 必要ならば、文字列をダブルクォートで囲む
        /// </summary>
        private static string EncloseDoubleQuotesIfNeed(string field)
        {
            if (NeedEncloseDoubleQuotes(field))
            {
                return EncloseDoubleQuotes(field);
            }
            return field;
        }

        /// <summary>
        /// 文字列をダブルクォートで囲む
        /// </summary>
        private static string EncloseDoubleQuotes(string field)
        {
            if (field.IndexOf('"') > -1)
            {
                //"を""とする
                field = field.Replace("\"", "\"\"");
            }
            return "\"" + field + "\"";
        }

        /// <summary>
        /// 文字列をダブルクォートで囲む必要があるか調べる
        /// </summary>
        private static bool NeedEncloseDoubleQuotes(string field)
        {
            return field.IndexOf('"') > -1 ||
                field.IndexOf(',') > -1 ||
                field.IndexOf('\r') > -1 ||
                field.IndexOf('\n') > -1 ||
                field.StartsWith(" ") ||
                field.StartsWith("\t") ||
                field.EndsWith(" ") ||
                field.EndsWith("\t");
        }

        /// <summary>アプリケーションの発行バージョンを取得するためのメソッド</summary>
        /// <returns>発行バージョンの文字列</returns>
        public static string GetAppliVersion()
        {
            if (!ApplicationDeployment.IsNetworkDeployed) return String.Empty;

            var version = ApplicationDeployment.CurrentDeployment.CurrentVersion;
            return (
              "Ver." +
              version.Major.ToString() + "." +
              version.Minor.ToString() + "." +
              version.Build.ToString() + "." +
              version.Revision.ToString()
            );
        }

        public static int ReformInt(int input, int min, int max)
        {
            if (input < min)
            {
                input = min;
            }
            if (max < input)
            {
                input = max;
            }
            return input;
        }
        /// <summary>ダイアログで指定した場所に引数の文字列をファイル出力する</summary>
        /// <param name="writeData">書き込む文字列</param>
        /// <param name="fileName">ファイル名のデフォルト値</param>
        /// <returns>成功したらTrue</returns>
        public static bool SaveStringToTextFile(string writeData, string fileName = "")
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.FilterIndex = 1;
            saveFileDialog.Filter = "csv File(.csv)|*.csv|テキスト ファイル(.txt)|*.txt|HTML File(*.html, *.htm)|*.html;*.htm|All Files (*.*)|*.*";
            saveFileDialog.FileName = fileName;
            bool? result = saveFileDialog.ShowDialog();
            if (result == true)
            {
                using (Stream fileStream = saveFileDialog.OpenFile())
                using (StreamWriter sw = new StreamWriter(fileStream, System.Text.Encoding.GetEncoding("shift_jis")))
                {
                    sw.Write(writeData);
                }
            }

            return (result == true);
        }

        /// <summary>HSV表色系からRGBに変換するためのクラス</summary>
        public static class HsvClass
        {
            /// <summary>HSV→RGB変換メソッド</summary>
            /// <param name="h">色相</param>
            /// <param name="s">彩度</param>
            /// <param name="v">明度</param>
            public static Color getHSVtoRGB(double h, double s, double v)
            {
                // 明度から各RGB値の最大値を取得
                double RGBmax = 255.0 * v;
                // 色相からRGBの取得
                double R = RGBmax * getPercent(h + 240);
                double G = RGBmax * getPercent(h + 120);
                double B = RGBmax * getPercent(h);
                // 各RGB値に彩度を設定する
                R += (RGBmax - R) * (1.0 - s);
                G += (RGBmax - G) * (1.0 - s);
                B += (RGBmax - B) * (1.0 - s);
                return Color.FromArgb(255, (byte)R, (byte)G, (byte)B);
            }

            /// <summary>色相を適正値に変換</summary>
            public static double getPercent(double d)
            {
                // 角度が負の場合は、正の値に直して代入する
                d = d < 0 ? 360 + (d % 360d) : d;
                // 角度の値が360以上の場合、値の範囲を0以上360未満にして代入
                d %= 360;
                if (d >= 0 && d < 120) return 0.0;
                else if (d >= 120 && d < 180) return (d - 120d) / 60d;
                else if (d >= 300 && d < 360) return 1d - ((d - 300d) / 60d);
                else return 1.0;
            }
        }

        /// <summary>テキストボックスにintにパースできない値は受け付けない。</summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        static public void NumOnlyTextBox(ref object sender)
        {
            // 数字以外は受け付けない簡単な方法
            TextBox tb = (TextBox)sender;
            int dTest;
            if (!int.TryParse(tb.Text, out dTest))
            {
                tb.Text = System.Text.RegularExpressions.Regex.Replace(tb.Text, "[^0-9+-.]", "");

                // 置き換えても対応できない場合、強制的に初期値に
                if (!int.TryParse(tb.Text, out dTest))
                {
                    tb.Text = "";
                }
            }
        }

        /// <summary>コントロールの配列を取得する </summary>
        /// <param name="frm">コントロールのあるフォーム</param>
        /// <param name="name">後ろの数字を除いたコントロールの名前</param>
        /// <returns>コントロールの配列。
        /// 取得できなかった時はnull</returns>
        /// <remarks>
        /// フォーム上に1から始まる連番のコントロール（例：textBox1,textBox2・・・）があった時、
        /// 最後の番号までのコントロールを取得し、配列にして返す。
        /// </remarks>
        public static object GetControlArrayByName(Window frm, string name)
        {
            System.Collections.ArrayList ctrs =
                new System.Collections.ArrayList();
            object obj;
            for (int i = 1;
                (obj = FindControlByFieldName(frm, name + i.ToString())) != null;
                i++)
                ctrs.Add(obj);

            if (ctrs.Count == 0)
                return null;
            else
                return ctrs.ToArray(ctrs[0].GetType());
        }

        /// <summary>フォームに配置されているコントロールを名前で探す
        /// （フォームクラスのフィールドをフィールド名で探す）
        /// </summary>
        /// <param name="frm">コントロールを探すフォーム</param>
        /// <param name="name">コントロール（フィールド）の名前</param>
        /// <returns>見つかった時は、コントロールのオブジェクト。
        /// 見つからなかった時は、nullを返す
        /// </returns>
        public static object FindControlByFieldName(Window frm, string name)
        {
            System.Type t = frm.GetType();

            System.Reflection.FieldInfo fi = t.GetField(
                name,
                System.Reflection.BindingFlags.Public |
                System.Reflection.BindingFlags.NonPublic |
                System.Reflection.BindingFlags.Instance |
                System.Reflection.BindingFlags.DeclaredOnly);

            if (fi == null)
                return null;

            return fi.GetValue(frm);
        }

        /// <summary>アセンブリバージョンの取得</summary>
        /// <returns>アセンブリバージョンの文字列</returns>
        /// <remarks>
        /// Propertiesのアセンブリ情報で設定したアセンブリバージョンを取得して文字列として返します。
        /// </remarks>
        static public string GetAssemblyVersion()
        {
            return "Ver." + System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString();
        }

        /// <summary>自分自身の実行ファイルのフルパスを取得する</summary>
        /// <returns>実行ファイルのフルパス</returns>
        static public string GetAppliPath()
        {
            return System.Reflection.Assembly.GetExecutingAssembly().Location;
        }

        /// <summary>自分自身の実行ファイルがあるディレクトリのパスを取得する</summary>
        /// <returns>実行ファイルがあるディレクトリのパス</returns>
        static public string GetDirectoryName()
        {
            return System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
        }
    }

    /// <summary>列挙型のフィールドに日本語リテラルを付加するためのカスタム属性</summary>
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false, Inherited = false)]
    public class EnumLiteralAttribute : Attribute
    {
        /// <summary>リテラル</summary>
        private string literal;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="literal">リテラル</param>
        public EnumLiteralAttribute(string literal)
        {
            this.literal = literal;
        }

        /// <summary>
        /// 属性で指定されたEnumのリテラルを取得します。
        /// </summary>
        /// <param name="value">Enum型の値</param>
        /// <returns>リテラル</returns>
        public static string GetLiteral(Enum value)
        {
            Type t = value.GetType();
            string name = Enum.GetName(t, value);
            System.Reflection.FieldInfo fi = t.GetField(name);
            EnumLiteralAttribute[] items = (EnumLiteralAttribute[])fi.GetCustomAttributes(typeof(EnumLiteralAttribute), false);
            if (items.Length == 0)
                return "未定義";
            else
                return items[0].literal;
        }

    }
}
