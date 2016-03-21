using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Windows;
using System.Linq;
using System.Text;
using System.Deployment.Application;
using Livet;

namespace IkaStylist.Models
{
    public class Request : NotificationObject
    {
        public Request()
        {
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
                RaisePropertyChanged();
            }
        }
        #endregion
    }

    ///<summary>オプション値を管理するためのクラス</summary>
    public class OptManager : NotificationObject
    {
        public OptManager()
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

        ///<summary>検索条件の最大サイズ</summary>
        public static readonly int RequestSize = Constants.__REQUEST_SIZE__;

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
    }
}//namespace IkaStylist.Models
