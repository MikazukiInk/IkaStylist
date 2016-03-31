using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Forms;
using System.Drawing;
using System.Drawing.Imaging;

namespace IkaStylist.Views
{
    /* 
     * ViewModelからの変更通知などの各種イベントを受け取る場合は、PropertyChangedWeakEventListenerや
     * CollectionChangedWeakEventListenerを使うと便利です。独自イベントの場合はLivetWeakEventListenerが使用できます。
     * クローズ時などに、LivetCompositeDisposableに格納した各種イベントリスナをDisposeする事でイベントハンドラの開放が容易に行えます。
     *
     * WeakEventListenerなので明示的に開放せずともメモリリークは起こしませんが、できる限り明示的に開放するようにしましょう。
     */

    /// <summary>
    /// GearPowerSelect.xaml の相互作用ロジック
    /// </summary>
    public partial class DisplayResultUnifiedGear : Window
    {
        public DisplayResultUnifiedGear()
        {
            InitializeComponent();
        }

        private Rect GetRectOnScreen(UIElement control)
        {
            System.Windows.Point targetLeftTop = control.PointToScreen(new System.Windows.Point(0.0, 0.0));
            return new Rect(targetLeftTop.X, targetLeftTop.Y, control.RenderSize.Width, control.RenderSize.Height);
        }

        public void Button_Click(object sender, RoutedEventArgs e)
        {
            IkaStylist.Models.OptManager OptMgr = IkaStylist.Models.OptManager.GetInstance();
            string defaultFileName = "myGears_";
            if (OptMgr.UnifiedGP_selectParts == 0)
            {
                defaultFileName = defaultFileName + IkaStylist.Models.GearKind.Head.ToString();
            }
            else if (OptMgr.UnifiedGP_selectParts == 1)
            {
                defaultFileName = defaultFileName + IkaStylist.Models.GearKind.Cloth.ToString();
            }
            else
            {
                defaultFileName = defaultFileName + IkaStylist.Models.GearKind.Shoes.ToString();
            }
            defaultFileName = defaultFileName + ".png";

            System.Windows.Forms.SendKeys.SendWait("%{PRTSC}");

            SaveFileDialog sfd = new SaveFileDialog();
            sfd.FileName = defaultFileName;
            sfd.InitialDirectory = System.Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory);
            sfd.Filter = "PNGファイル(*.png)|*.png";
            sfd.FilterIndex = 1;
            sfd.Title = "保存先のファイルを指定してください";
            sfd.RestoreDirectory = true;
            sfd.OverwritePrompt = true;
            sfd.CheckPathExists = true;

            //ダイアログを表示する
            if (sfd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                System.Windows.IDataObject dobj = System.Windows.Clipboard.GetDataObject();
                if (dobj.GetDataPresent(System.Windows.DataFormats.Bitmap) == true)
                {
                    System.Windows.Interop.InteropBitmap ibmp
                      = (System.Windows.Interop.InteropBitmap)dobj.GetData(System.Windows.DataFormats.Bitmap);

                    PngBitmapEncoder enc = new PngBitmapEncoder();
                    enc.Frames.Add(BitmapFrame.Create(ibmp));
                    System.IO.FileStream fs = new System.IO.FileStream(sfd.FileName,
                      System.IO.FileMode.Create, System.IO.FileAccess.Write);
                    enc.Save(fs);
                    fs.Close();
                    fs.Dispose();
                    this.Close();
                }
            }
        }
    }
}