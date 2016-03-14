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
    		MaxReslutSize = Constants.__DEFAULT_RESULT_SIZE__;
    		Requests = new Request[Constants.__REQUEST_SIZE__];
    		OnlyEnhanced = true;
    		Tolerance = 0;
    	}
    	
    	///<summary>検索条件の最大サイズ</summary>
        #region 
        private int _RequestSize = Constants.__REQUEST_SIZE__;
        public int RequestSize
        {
            get
            { return _RequestSize; }
            set
            { /* set 不可 */}
        }
        #endregion

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
    }
}//namespace IkaStylist.Models
