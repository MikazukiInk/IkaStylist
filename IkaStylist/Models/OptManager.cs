using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Windows;
using System.Windows.Media;
using System.Linq;
using System.Text;
using System.Deployment.Application;
using Livet;
using IkaStylist.ViewModels;

namespace IkaStylist.Models
{
    public class Request : NotificationObject
    {
        private OptManager Opt;

        public Request()
        {
            Opt = OptManager.GetInstance();
            Point = 0;
            GearPowerID = 0;
        }

        #region Point変更通知プロパティ
        private int _Point;

        public int Point
        {
            get
            { return _Point; }
            set
            {
                if (_Point == value)
                    return;
                _Point = value;

                Opt.CalcRemainingPoint();
                RaisePropertyChanged();
            }
        }
        #endregion

        #region GearPowerID変更通知プロパティ
        private int _GearPowerID;

        public int GearPowerID
        {
            get
            { return _GearPowerID; }
            set
            {
                if (_GearPowerID == value)
                    return;
                _GearPowerID = value;
                isNotMainOnlyGearPower = true;
                if (!isNotMainOnlyGearPower)
                {
                    //メイン限定ギアパワーは 10 で固定する.
                    Point = 10;
                }
                else if (_GearPowerID == (int)GearPowerKind.None)
                {
                    //"なし"は 0 にする.
                    Point = 0;
                }
                else
                {
                    //その他のギアパワーは、前が未設定だと 10 になる.
                    if (this.Point == 0)
                    {
                        Point = 10;
                    }
                }
                RaisePropertyChanged();
            }
        }
        #endregion

        #region メイン限定ギアかどうか.
        public bool isNotMainOnlyGearPower
        {
            get
            {
                if (this.GearPowerID < 0 && this.GearPowerID > EnumExtension<GearPowerKind>.Length())
                {
                    return true;
                }
                if (IkaUtil.MainOnlyGear.Contains(EnumLiteralAttribute.GetLiteral((GearPowerKind)this.GearPowerID)))
                {
                    return false;
                }
                else if (this.GearPowerID == (int)GearPowerKind.None)
                {
                    //"なし"も、編集不可にする.
                    return false;
                }
                else
                {
                    return true;
                }
            }
            set
            {
                RaisePropertyChanged();
            }
        }
        #endregion
    }

    ///<summary>オプション値を管理するためのクラス</summary>
    public class OptManager : NotificationObject
    {
        //フォームのインスタンスを保持するフィールド
        private static readonly OptManager _Instance = new OptManager();
        ///<summary>検索条件の最大サイズ</summary>
        public static readonly int RequestSize = Constants.__REQUEST_SIZE__;

        private OptManager()
        {
            this.MaxReslutSize = Constants.__DEFAULT_RESULT_SIZE__;
            this.Requests = new Request[Constants.__REQUEST_SIZE__];
            for (int i = 0; i < this.Requests.Length; i++)
            {
                this.Requests[i] = new Request();
            }
            this.OnlyEnhanced = true;
            this.Tolerance = 0;
            this.isFestival = false;
            this.canSearch = true;
        }

        public static OptManager GetInstance()
        {
            return _Instance;
        }

        ///<summary>検索リクエストのリセット</summary>
        public void ResetRequest()
        {
            var temp = new Request[OptManager.RequestSize];
            for (int i = 0; i < temp.Length; i++)
            {
                temp[i] = new Request();
            }
            temp[0].GearPowerID = 1;//条件１は攻撃力に設定
            temp[0].Point = 10;//初期値10に設定
            this.Requests = temp;
            this.Tolerance = 0;
        }

        ///<summary>最大表示件数</summary>
        #region MaxReslutSize変更通知プロパティ
        private int _MaxReslutSize;
        public int MaxReslutSize
        {
            get
            { return _MaxReslutSize; }
            set
            {
                if (_MaxReslutSize == value)
                    return;
                _MaxReslutSize = value;
                RaisePropertyChanged();
            }
        }
        #endregion

        ///<summary>ギアパワーと要求Pt</summary>
        #region Requests変更通知プロパティ
        private Request[] _Requests;
        public Request[] Requests
        {
            get
            { return _Requests; }
            set
            {
                if (_Requests == value)
                    return;
                _Requests = value;

                CalcRemainingPoint();
                RaisePropertyChanged();
            }
        }
        #endregion

        ///<summary>強化済みのみを使用するフラグ</summary>
        #region OnlyEnhanced変更通知プロパティ
        private bool _OnlyEnhanced;
        public bool OnlyEnhanced
        {
            get
            { return _OnlyEnhanced; }
            set
            {
                if (_OnlyEnhanced == value)
                    return;
                _OnlyEnhanced = value;
                RaisePropertyChanged();
            }
        }
        #endregion

        ///<summary>許容誤差</summary>
        #region 許容誤差
        private int _Tolerance;
        public int Tolerance
        {
            get
            { return _Tolerance; }
            set
            {
                if (_Tolerance == value)
                    return;
                if (value < 0)
                {
                    return;
                }
                _Tolerance = value;
                RaisePropertyChanged();
            }
        }
        #endregion

        ///<summary>フェスTモード</summary>
        #region フェスTモード
        private bool _isFestival;
        public bool isFestival
        {
            get
            { return _isFestival; }
            set
            {
                if (_isFestival == value)
                    return;
                _isFestival = value;
                RaisePropertyChanged();
            }
        }
        #endregion

        public void CalcRemainingPoint()
        {
            int temp = 57;
            for (int i = 0; i < this.Requests.Length; i++)
            {
                if (this.Requests[i] != null)
                    temp -= this.Requests[i].Point;
            }
            RemainingPoint = temp;
        }

        #region RemainingPoint変更通知プロパティ
        private int _RemainingPoint;

        public int RemainingPoint
        {
            get
            { return _RemainingPoint; }
            set
            {
                if (_RemainingPoint == value)
                    return;
                _RemainingPoint = value;
                if (_RemainingPoint < 0)
                {
                    canSearch = false;
                    RemainingPointColor = Brushes.Red;
                }
                else
                {
                    canSearch = true;
                    RemainingPointColor = Brushes.Black;
                }
                RaisePropertyChanged();
            }
        }
        #endregion

        private bool _canSearch;
        public bool canSearch
        {
            get
            {
                return _canSearch;
            }
            set
            {
                _canSearch = value;
                RaisePropertyChanged();
            }
        }

        private System.Windows.Media.Brush _RemainingPointColor;
        public System.Windows.Media.Brush RemainingPointColor
        {
            get
            {
                return _RemainingPointColor;
            }
            set
            {
                if (_RemainingPointColor == value)
                {
                    return;
                }
                _RemainingPointColor = value;
                RaisePropertyChanged();
            }
        }
    }
}//namespace IkaStylist.Models
