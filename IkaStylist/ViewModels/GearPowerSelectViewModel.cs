using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Data;

using Livet;
using Livet.Commands;
using Livet.Messaging;
using Livet.Messaging.IO;
using Livet.EventListeners;
using Livet.Messaging.Windows;

using IkaStylist.Models;

namespace IkaStylist.ViewModels
{
    public class GearPowerSelectViewModel : ViewModel
    {
        const int SubPowerSize = 3;
        private Gear _Origin;

        public GearPowerSelectViewModel(Gear gear)
        {
            this.Count = 0;
            this.SubPower = new string[SubPowerSize];
            this.GearName += gear.Name;
            _Origin = gear;
        }

        public void Initialize()
        {
            Message = string.Format("{0}つめのギアパワーをえらんでください", this.Count + 1);

        }

        #region GearName変更通知プロパティ
        private string _GearName;

        public string GearName
        {
            get
            { return _GearName; }
            set
            {
                if (_GearName == value)
                    return;
                _GearName = value;
                RaisePropertyChanged();
            }
        }
        #endregion

        #region SubPower変更通知プロパティ
        private string[] _SubPower = new string[SubPowerSize];

        public string[] SubPower
        {
            get
            { return _SubPower; }
            set
            {
                if (_SubPower == value)
                    return;
                _SubPower = value;
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
                updateSelectGearPowerImg();
                RaisePropertyChanged();
            }
        }
        #endregion

        #region SelectCommand
        private ListenerCommand<string> _SelectCommand;

        public ListenerCommand<string> SelectCommand
        {
            get
            {
                if (_SelectCommand == null)
                {
                    _SelectCommand = new ListenerCommand<string>(Select);
                }
                return _SelectCommand;
            }
        }

        public void Select(string id)
        {
            string[] temp = (string[])this.SubPower.Clone();
            temp[this.Count] = Gear.IdToName(int.Parse(id));
            this.SubPower = temp;
            this.Count++;

            if (SubPowerSize <= this.Count)
            {
                _Origin.SubPower1.Name = this.SubPower[0];
                _Origin.SubPower2.Name = this.SubPower[1];
                _Origin.SubPower3.Name = this.SubPower[2];

                if ((this.SubPower[0] == "なし")
                 && (this.SubPower[1] == "なし")
                 && (this.SubPower[2] == "なし"))
                {
                    _Origin.LastUpdated = string.Empty;
                }
                else
                {
                    _Origin.LastUpdated = DateTime.Now.ToString();
                }

                Messenger.Raise(new WindowActionMessage(WindowAction.Close, "Close"));
            }
            else
            {
                Message = string.Format("{0}つめのギアパワーをえらんでください", this.Count + 1);
            }
        }
        #endregion

        #region CancelCommand
        private ListenerCommand<string> _CancelCommand;

        public ListenerCommand<string> CancelCommand
        {
            get
            {
                if (_CancelCommand == null)
                {
                    _CancelCommand = new ListenerCommand<string>(Cancel);
                }
                return _CancelCommand;
            }
        }

        public void Cancel(string parameter)
        {
            this.Count -= int.Parse(parameter);
            if (this.Count < 0)
            {
                this.Count = 0;
            }
            Message = string.Format("{0}つめのギアパワーをえらんでください", this.Count + 1);
        }
        #endregion

        #region Message変更通知プロパティ
        private string _Message;

        public string Message
        {
            get
            { return _Message; }
            set
            {
                if (_Message == value)
                    return;
                _Message = value;
                RaisePropertyChanged("Message");
            }
        }
        #endregion

        #region ギア画像変更通知プロパティ
        public BitmapImage GearImg
        {
            get
            {
                if (_Origin != null)
                {
                    return IkaUtil.getGearBitmap(_Origin.imgName);
                }
                else
                {
                    return IkaUtil.getGearBitmap(null);//本来ない.
                }
            }
            set
            {
                RaisePropertyChanged();
            }
        }
        #endregion

        #region 選択中のギア画像を表示/更新する.
        private void updateSelectGearPowerImg()
        {
            SelectGearPowerImg1 = new BitmapImage();
            SelectGearPowerImg2 = new BitmapImage();
            SelectGearPowerImg3 = new BitmapImage();
            SelectGearPowerBorderBrush1 = Brushes.SteelBlue;
            SelectGearPowerBorderBrush2 = Brushes.SteelBlue;
            SelectGearPowerBorderBrush3 = Brushes.SteelBlue;
            SelectGearPowerOpacity1 = 1.0;
            SelectGearPowerOpacity2 = 1.0;
            SelectGearPowerOpacity3 = 1.0;
            SelectGearPowerBorderThickness1 = 1;
            SelectGearPowerBorderThickness2 = 1;
            SelectGearPowerBorderThickness3 = 1;
        }

        private int getGearPowerId( string name )
        {
            int Id = 0;
            if (IkaUtil.GearPowerDict.ContainsValue(name))
            {
                Id = (int)IkaUtil.GearPowerDict.First(x => x.Value == name).Key;
            }
            return Id;
        }

        public BitmapImage SelectGearPowerImg1
        {
            get
            {
                if (this.Count > 0)
                {
                    return IkaUtil.getGearPowerBitmap(getGearPowerId(this.SubPower[0]));
                }
                else
                {
                    return IkaUtil.getGearPowerBitmap(getGearPowerId(_Origin.SubPower1.Name));
                }
            }
            set
            {
                RaisePropertyChanged();
            }
        }

        public BitmapImage SelectGearPowerImg2
        {
            get
            {
                if (this.Count > 1)
                {
                    return IkaUtil.getGearPowerBitmap(getGearPowerId(this.SubPower[1]));
                }
                else
                {
                    return IkaUtil.getGearPowerBitmap(getGearPowerId(_Origin.SubPower2.Name));
                }
            }
            set
            {
                RaisePropertyChanged();
            }
        }

        public BitmapImage SelectGearPowerImg3
        {
            get
            {
                if (this.Count > 2)
                {
                    return IkaUtil.getGearPowerBitmap(getGearPowerId(this.SubPower[2]));
                }
                else
                {
                    return IkaUtil.getGearPowerBitmap(getGearPowerId(_Origin.SubPower3.Name));
                }
            }
            set
            {
                RaisePropertyChanged();
            }
        }

        public System.Windows.Media.Brush SelectGearPowerBorderBrush1
        {
            get
            {
                if (this.Count == 0)
                {
                    return Brushes.PaleVioletRed;
                }
                else if (this.Count > 0)
                {
                    return Brushes.SteelBlue;
                }
                else
                {
                    return Brushes.Silver;
                }
            }
            set
            {
                RaisePropertyChanged();
            }
        }

        public System.Windows.Media.Brush SelectGearPowerBorderBrush2
        {
            get
            {
                if (this.Count == 1)
                {
                    return Brushes.PaleVioletRed;
                }
                else if (this.Count > 1)
                {
                    return Brushes.SteelBlue;
                }
                else
                {
                    return Brushes.Silver;
                }
            }
            set
            {
                RaisePropertyChanged();
            }
        }

        public System.Windows.Media.Brush SelectGearPowerBorderBrush3
        {
            get
            {
                if (this.Count == 2)
                {
                    return Brushes.PaleVioletRed;
                }
                else if (this.Count > 2)
                {
                    return Brushes.SteelBlue;
                }
                else
                {
                    return Brushes.Silver;
                }
            }
            set
            {
                RaisePropertyChanged();
            }
        }

        public double SelectGearPowerOpacity1
        {
            get
            {
                if (this.Count > 0)
                {
                    return 1.0;
                }
                else
                {
                    return 0.2;
                }
            }
            set
            {
                RaisePropertyChanged();
            }
        }

        public double SelectGearPowerOpacity2
        {
            get
            {
                if (this.Count > 1)
                {
                    return 1.0;
                }
                else
                {
                    return 0.2;
                }
            }
            set
            {
                RaisePropertyChanged();
            }
        }

        public double SelectGearPowerOpacity3
        {
            get
            {
                if (this.Count > 2)
                {
                    return 1.0;
                }
                else
                {
                    return 0.2;
                }
            }
            set
            {
                RaisePropertyChanged();
            }
        }

        public int SelectGearPowerBorderThickness1
        {
            get
            {
                if (this.Count == 0)
                {
                    return 4;
                }
                else
                {
                    return 2;
                }
            }
            set
            {
                RaisePropertyChanged();
            }
        }

        public int SelectGearPowerBorderThickness2
        {
            get
            {
                if (this.Count == 1)
                {
                    return 4;
                }
                else
                {
                    return 2;
                }
            }
            set
            {
                RaisePropertyChanged();
            }
        }

        public int SelectGearPowerBorderThickness3
        {
            get
            {
                if (this.Count == 2)
                {
                    return 4;
                }
                else
                {
                    return 2;
                }
            }
            set
            {
                RaisePropertyChanged();
            }
        }
        #endregion
    }
}
