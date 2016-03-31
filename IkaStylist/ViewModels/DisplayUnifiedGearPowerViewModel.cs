using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Windows.Media;
using System.Windows.Media.Imaging;

using Livet;
using Livet.Commands;
using Livet.Messaging;
using Livet.Messaging.IO;
using Livet.EventListeners;
using Livet.Messaging.Windows;

using IkaStylist.Models;

namespace IkaStylist.ViewModels
{
    public class DisplayUnifiedGearPowerViewModel : ViewModel
    {
        public DisplayUnifiedGearPowerViewModel()
        {
        	
        }

        ///<summary>オプション管理クラスの実体</summary>
        private OptManager _OptMgr;
        public OptManager OptMgr
        {
            get
            {
                if (_OptMgr == null)
                {
                    _OptMgr = OptManager.GetInstance();
                }
                return _OptMgr;
            }
            set
            {
                if (_OptMgr == value)
                    return;
                _OptMgr = value;
            }
        }

        public void Initialize()
        {
            makePartsNames();
            updateGearImg();
        }

        private void makePartsNames()
        {
            //makePartsNames初期化
            var temp = new List<string>();
            temp.Add("アタマ");
            temp.Add("フク");
            temp.Add("クツ");
            this.PartsNames = temp;
        }

        #region BrandNames変更通知プロパティ
        private List<string> _PartsNames;
        //コンボボックスにギアパワー名を表示するためのなにか
        public List<string> PartsNames
        {
            get
            { return _PartsNames; }
            set
            {
                if (_PartsNames == value)
                    return;
                _PartsNames = value;
                RaisePropertyChanged();
            }
        }
        #endregion

        #region
        public int selectParts
        {
            get
            { return OptMgr.UnifiedGP_selectParts; }
            set
            {
                if (OptMgr.UnifiedGP_selectParts == value)
                    return;
                OptMgr.UnifiedGP_selectParts = value;
                RaisePropertyChanged();
            }
        }
        #endregion

        public bool pureBrand
        {
            get
            {
                return OptMgr.UnifiedGP_displayPureGear;
            }
            set
            {
                if (OptMgr.UnifiedGP_displayPureGear == value)
                    return;
                OptMgr.UnifiedGP_displayPureGear = value;
                RaisePropertyChanged();
            }
        }

        public int pureBrandNum
        {
            get
            {
                return OptMgr.UnifiedGP_pureGearNum;
            }
            set
            {
                if (OptMgr.UnifiedGP_pureGearNum == value)
                    return;
                if (value < 2 || value > 3)
                    return;
                OptMgr.UnifiedGP_pureGearNum = value;
                updateGearImg();
                RaisePropertyChanged();
            }
        }

        public bool fakeBrand
        {
            get
            {
                return OptMgr.UnifiedGP_displayFakeGear;
            }
            set
            {
                if (OptMgr.UnifiedGP_displayFakeGear == value)
                    return;
                OptMgr.UnifiedGP_displayFakeGear = value;
                RaisePropertyChanged();
            }
        }

        public int fakeBrandNum
        {
            get
            {
                return OptMgr.UnifiedGP_fakeGearNum;
            }
            set
            {
                if (OptMgr.UnifiedGP_fakeGearNum == value)
                    return;
                if (value < 1 || value > 3)
                    return;
                OptMgr.UnifiedGP_fakeGearNum = value;
                updateGearImg();
                RaisePropertyChanged();
            }
        }

        #region DisplayResultCommand
        private ListenerCommand<string> _DisplayResultCommand;

        public ListenerCommand<string> DisplayResultCommand
        {
            get
            {
                if (_DisplayResultCommand == null)
                {
                    _DisplayResultCommand = new ListenerCommand<string>(DisplayResult);
                }
                return _DisplayResultCommand;
            }
        }

        public void DisplayResult(string parameter)
        {
            using (var vm = new DisplayResultUnifiedGearViewModel())
            {
                Messenger.Raise(new TransitionMessage(vm, "DisplayResultCommand"));
            }
        }
        #endregion

        private void updateGearImg()
        {
            FakeMainGearPowerImg = new BitmapImage();
            FakeSubGearPowerImg1 = new BitmapImage();
            FakeSubGearPowerImg2 = new BitmapImage();
            FakeSubGearPowerImg3 = new BitmapImage();
            PureMainGearPowerImg = new BitmapImage();
            PureSubGearPowerImg1 = new BitmapImage();
            PureSubGearPowerImg2 = new BitmapImage();
            PureSubGearPowerImg3 = new BitmapImage();

            FakeMainGearPowerBorderBrush = Brushes.Silver;
            FakeSubGearPowerBorderBrush1 = Brushes.Silver;
            FakeSubGearPowerBorderBrush2 = Brushes.Silver;
            FakeSubGearPowerBorderBrush3 = Brushes.Silver;
            PureMainGearPowerBorderBrush = Brushes.Silver;
            PureSubGearPowerBorderBrush1 = Brushes.Silver;
            PureSubGearPowerBorderBrush2 = Brushes.Silver;
            PureSubGearPowerBorderBrush3 = Brushes.Silver;
        }

        public BitmapImage FakeMainGearPowerImg
        {
            get
            {
                return IkaUtil.getGearPowerBitmap(1);
            }
            set
            {
                RaisePropertyChanged();
            }
        }

        public BitmapImage FakeSubGearPowerImg1
        {
            get
            {
                return IkaUtil.getGearPowerBitmap(1);
            }
            set
            {
                RaisePropertyChanged();
            }
        }

        public BitmapImage FakeSubGearPowerImg2
        {
            get
            {
                if (OptMgr.UnifiedGP_fakeGearNum < 2)
                {
                    return IkaUtil.getGearPowerBitmap(0);
                }
                else
                {
                    return IkaUtil.getGearPowerBitmap(1);
                }
            }
            set
            {
                RaisePropertyChanged();
            }
        }

        public BitmapImage FakeSubGearPowerImg3
        {
            get
            {
                if (OptMgr.UnifiedGP_fakeGearNum < 3)
                {
                    return IkaUtil.getGearPowerBitmap(0);
                }
                else
                {
                    return IkaUtil.getGearPowerBitmap(1);
                }
            }
            set
            {
                RaisePropertyChanged();
            }
        }

        public BitmapImage PureMainGearPowerImg
        {
            get
            {
                return IkaUtil.getGearPowerBitmap(1);
            }
            set
            {
                RaisePropertyChanged();
            }
        }

        public BitmapImage PureSubGearPowerImg1
        {
            get
            {
                return IkaUtil.getGearPowerBitmap(3);
            }
            set
            {
                RaisePropertyChanged();
            }
        }

        public BitmapImage PureSubGearPowerImg2
        {
            get
            {
                return IkaUtil.getGearPowerBitmap(3);
            }
            set
            {
                RaisePropertyChanged();
            }
        }

        public BitmapImage PureSubGearPowerImg3
        {
            get
            {
                if (OptMgr.UnifiedGP_pureGearNum < 3)
                {
                    return IkaUtil.getGearPowerBitmap(0);
                }
                else
                {
                    return IkaUtil.getGearPowerBitmap(3);
                }
            }
            set
            {
                RaisePropertyChanged();
            }
        }

        public System.Windows.Media.Brush FakeMainGearPowerBorderBrush
        {
            get
            {
                return Brushes.DodgerBlue;
            }
            set
            {
                RaisePropertyChanged();
            }
        }

        public System.Windows.Media.Brush FakeSubGearPowerBorderBrush1
        {
            get
            {
                return Brushes.DodgerBlue;
            }
            set
            {
                RaisePropertyChanged();
            }
        }

        public System.Windows.Media.Brush FakeSubGearPowerBorderBrush2
        {
            get
            {
                if (OptMgr.UnifiedGP_fakeGearNum < 2)
                {
                    return Brushes.Silver;
                }
                else
                {
                    return Brushes.DodgerBlue;
                }
            }
            set
            {
                RaisePropertyChanged();
            }
        }

        public System.Windows.Media.Brush FakeSubGearPowerBorderBrush3
        {
            get
            {
                if (OptMgr.UnifiedGP_fakeGearNum < 3)
                {
                    return Brushes.Silver;
                }
                else
                {
                    return Brushes.DodgerBlue;
                }
            }
            set
            {
                RaisePropertyChanged();
            }
        }

        public System.Windows.Media.Brush PureMainGearPowerBorderBrush
        {
            get
            {
                return Brushes.DodgerBlue;
            }
            set
            {
                RaisePropertyChanged();
            }
        }

        public System.Windows.Media.Brush PureSubGearPowerBorderBrush1
        {
            get
            {
                return Brushes.DodgerBlue;
            }
            set
            {
                RaisePropertyChanged();
            }
        }

        public System.Windows.Media.Brush PureSubGearPowerBorderBrush2
        {
            get
            {
                return Brushes.DodgerBlue;
            }
            set
            {
                RaisePropertyChanged();
            }
        }

        public System.Windows.Media.Brush PureSubGearPowerBorderBrush3
        {
            get
            {
                if (OptMgr.UnifiedGP_pureGearNum < 3)
                {
                    return Brushes.Silver;
                }
                else
                {
                    return Brushes.DodgerBlue;
                }
            }
            set
            {
                RaisePropertyChanged();
            }
        }
    }
}
