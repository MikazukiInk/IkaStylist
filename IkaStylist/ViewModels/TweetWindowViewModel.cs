using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.IO;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Xml;
using System.Runtime.Serialization;
using System.Deployment.Application;

using Livet;
using Livet.Commands;
using Livet.Messaging;
using Livet.Messaging.IO;
using Livet.EventListeners;
using Livet.Messaging.Windows;

using IkaStylist.Models;

namespace IkaStylist.ViewModels
{
    public class TweetWindowViewModel : ViewModel
    {
        string fileName = @".\Setting\TwitterToken.xml";
        string folderName = @".\Setting";

        #region SelectionMgr変更通知プロパティ
        private SelectionManager _SelectionMgr;

        public SelectionManager SelectionMgr
        {
            get
            { return _SelectionMgr; }
            set
            {
                if (_SelectionMgr == value)
                    return;
                _SelectionMgr = value;
                RaisePropertyChanged();
            }
        }
        #endregion

        private CoreTweet.Tokens token;

        public TweetWindowViewModel(Coordinate coordinate)
        {
            //保存元のファイル名

            if (ApplicationDeployment.IsNetworkDeployed)//ClickOnceでのインストールなら
            {
                this.fileName = ApplicationDeployment.CurrentDeployment.DataDirectory + this.fileName;
                this.folderName = ApplicationDeployment.CurrentDeployment.DataDirectory + this.folderName;
            }

            if (System.IO.File.Exists(this.fileName))
            {
                //DataContractSerializerオブジェクトを作成
                DataContractSerializer serializer =
                    new DataContractSerializer(typeof(CoreTweet.Tokens));
                //読み込むファイルを開く
                XmlReader xr = XmlReader.Create(this.fileName);
                //XMLファイルから読み込み、逆シリアル化する
                this.token = (CoreTweet.Tokens)serializer.ReadObject(xr);
                //ファイルを閉じる
                xr.Close();
            }

            this.SelectionMgr = new SelectionManager();
            this.SelectionMgr.update(coordinate);
        }

        public void Initialize()
        {
            this.TweetText = " #イカスタイリスト #Splatoon goo.gl/8YRBYI";
        }

        #region TweetText変更通知プロパティ
        private string _TweetText;

        public string TweetText
        {
            get
            { return _TweetText; }
            set
            {
                if (_TweetText == value)
                    return;
                _TweetText = value;
                Count = 140 - 47 - _TweetText.Length;
                RaisePropertyChanged();
            }
        }
        #endregion

        #region Count変更通知プロパティ
        private int _Count;

        public int Count
        {
            get
            { return _Count; }
            set
            {
                if (_Count == value)
                    return;
                _Count = value;
                if (_Count < 0)
                {
                    this.CanTweet = false;
                    this.CountColor = Brushes.Red;
                }
                else
                {
                    this.CanTweet = true;
                    this.CountColor = Brushes.Black;
                }

                RaisePropertyChanged();
            }
        }
        #endregion

        #region CountColor変更通知プロパティ
        private Brush _CountColor;

        public Brush CountColor
        {
            get
            { return _CountColor; }
            set
            {
                if (_CountColor == value)
                    return;
                _CountColor = value;
                RaisePropertyChanged();
            }
        }
        #endregion

        #region CanTweet変更通知プロパティ
        private bool _CanTweet;

        public bool CanTweet
        {
            get
            { return _CanTweet; }
            set
            {
                if (_CanTweet == value)
                    return;
                _CanTweet = value;
                RaisePropertyChanged();
            }
        }
        #endregion

        #region TweetCommand
        private ListenerCommand<Object> _TweetCommand;

        public ListenerCommand<Object> TweetCommand
        {
            get
            {
                if (_TweetCommand == null)
                {
                    _TweetCommand = new ListenerCommand<Object>(Tweet);
                }
                return _TweetCommand;
            }
        }

        public void Tweet(Object parameter)
        {

            var desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            var ImagePath = System.IO.Path.Combine(this.folderName, "tempImage.png");
            var coordinate = (FrameworkElement)parameter;
            SaveImage(coordinate, ImagePath);


            try
            {
                CoreTweet.MediaUploadResult first = this.token.Media.Upload(media: new FileInfo(ImagePath));
                CoreTweet.Status s = this.token.Statuses.Update(
                                                                status: this.TweetText,
                                                                media_ids: new long[] { first.MediaId }
                                                            );
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return;
            }

            Messenger.Raise(new WindowActionMessage(WindowAction.Close, "Close"));

        }
        #endregion

        // 要素を画像として保存する
        private void SaveImage(FrameworkElement target, string path)
        {
            if (target == null) throw new ArgumentNullException("target");
            if (string.IsNullOrWhiteSpace(path)) throw new ArgumentException("pathが未設定");

            // レンダリング
            var bmp = new RenderTargetBitmap(
                (int)target.ActualWidth,
                (int)target.ActualHeight,
                96, 96, // DPI
                PixelFormats.Pbgra32);
            bmp.Render(target);

            // pngで保存
            var encoder = new PngBitmapEncoder();
            encoder.Frames.Add(BitmapFrame.Create(bmp));
            using (var fs = File.Open(path, FileMode.Create))
            {
                encoder.Save(fs);
            }
        }
    }


}
