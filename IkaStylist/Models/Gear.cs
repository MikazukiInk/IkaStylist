using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Windows;
using System.Linq;
using System.Text;
using System.Windows.Media.Imaging;

using Livet;

namespace IkaStylist.Models
{
    ///<summary>ギアを管理するためのクラス</summary>
    public class Gear : NotificationObject
    {
        public Gear()
        {
            this._Id         = -1;
            this._Name       = "no input";
            this._MainPower  = new GearPower();
            this._SubPower1  = new GearPower();
            this._SubPower2  = new GearPower();
            this._SubPower3  = new GearPower();
            this.LastUpdated = "";
            this._imgName    = "";
            this.Brand       = new Brand();
            this.visibleGear = true;
        }

        public Gear( Gear input )
        {
            this._Id         = input.Id;
            this._Name       = input.Name;
            this._MainPower  = new GearPower(input.MainPower);
            this._SubPower1  = new GearPower(input.SubPower1);
            this._SubPower2  = new GearPower(input.SubPower2);
            this._SubPower3  = new GearPower(input.SubPower3);
            this.LastUpdated = input.LastUpdated;
            this._imgName    = input.imgName;
            this.Brand       = input.Brand;
            this.visibleGear = input.visibleGear;
        }
        
        static public string IdToName(int id)
        {
            return EnumLiteralAttribute.GetLiteral( (GearPowerKind)(id) );
        }

        #region Id変更通知プロパティ
        private int _Id;

        public int Id
        {
            get
            { return _Id; }
            set
            {
                if (_Id == value)
                    return;
                _Id = value;
                RaisePropertyChanged();
            }
        }
        #endregion

        #region Name変更通知プロパティ
        private string _Name;

        public string Name
        {
            get
            { return _Name; }
            set
            {
                if (_Name == value)
                    return;
                _Name   = value;
                //Id
                this.Id = _Name.GetHashCode();
                //画像ファイル名.
                if (IkaUtil.GearImgDict.ContainsKey(_Name))
                {
                    imgName = IkaUtil.GearImgDict[_Name];
                }
                else
                {
                    imgName = "None";
                }
                //ブランド.
                if (IkaUtil.BrandImgDict.ContainsKey(_Name))
                {
                    Brand = IkaUtil.BrandImgDict[_Name];
                }
                else
                {
                    Brand = new Brand();
                }
                RaisePropertyChanged();
            }
        }
        #endregion

        #region MainPower変更通知プロパティ
        private GearPower _MainPower;

        public GearPower MainPower
        {
            get
            { return _MainPower; }
            set
            {
                if (_MainPower == value)
                    return;
                _MainPower = value;
                RaisePropertyChanged();
            }
        }

        public string MainPowerName
        {
            get
            { return _MainPower.Name; }
            set
            {
                if (_MainPower.Name == value)
                    return;
                _MainPower.Name = value;
                RaisePropertyChanged();
            }
        }
        #endregion

        #region SubPower1変更通知プロパティ
        private GearPower _SubPower1;

        public GearPower SubPower1
        {
            get
            { return _SubPower1; }
            set
            {
                if (_SubPower1 == value)
                    return;
                _SubPower1 = value;
                RaisePropertyChanged();
            }
        }

        public string SubPower1Name
        {
            get
            { return _SubPower1.Name; }
            set
            {
                if (_SubPower1.Name == value)
                    return;
                _SubPower1.Name = value;
                RaisePropertyChanged();
            }
        }
        #endregion

        #region SubPower2変更通知プロパティ
        private GearPower _SubPower2;

        public GearPower SubPower2
        {
            get
            { return _SubPower2; }
            set
            {
                if (_SubPower2 == value)
                    return;
                _SubPower2 = value;
                RaisePropertyChanged();
            }
        }

        public string SubPower2Name
        {
            get
            { return _SubPower2.Name; }
            set
            {
                if (_SubPower2.Name == value)
                    return;
                _SubPower2.Name = value;
                RaisePropertyChanged();
            }
        }
        #endregion

        #region SubPower3変更通知プロパティ
        private GearPower _SubPower3;

        public GearPower SubPower3
        {
            get
            { return _SubPower3; }
            set
            {
                if (_SubPower3 == value)
                    return;
                _SubPower3 = value;
                RaisePropertyChanged();
            }
        }

        public string SubPower3Name
        {
            get
            { return _SubPower3.Name; }
            set
            {
                if (_SubPower3.Name == value)
                    return;
                _SubPower3.Name = value;
                RaisePropertyChanged();
            }
        }
        #endregion

        #region LastUpdated変更通知プロパティ
        private string _LastUpdated;

        public string LastUpdated
        {
            get
            { return _LastUpdated; }
            set
            { 
                if (_LastUpdated == value)
                    return;
                _LastUpdated = value;
                RaisePropertyChanged();
            }
        }
        #endregion

        #region 画像データ名変更通知プロパティ
        private string _imgName;

        public string imgName
        {
            get
            { return _imgName; }
            set
            { 
                if (_imgName == value)
                    return;
                _imgName = value;
                RaisePropertyChanged();
            }
        }
        #endregion

        #region
        public BitmapImage BitmapImg
        {
            get
            {
                return IkaUtil.getGearBitmap( imgName );
            }
            set
            {
                RaisePropertyChanged();
            }
        }
        #endregion

        #region ブランド変更通知プロパティ
        private Brand _brand;

        public Brand Brand
        {
            get
            { return _brand; }
            set
            {
                if (_brand == value)
                    return;
                _brand = value;
                RaisePropertyChanged();
            }
        }
        #endregion

        #region
        private bool _visibleGear;
        public bool visibleGear
        {
            get
            {
                return _visibleGear;
            }
            set
            {
                if (_visibleGear == value)
                    return;
                _visibleGear = value;
                RaisePropertyChanged();
            }
        }
        #endregion
    }
}//namespace IkaStylist.Models
