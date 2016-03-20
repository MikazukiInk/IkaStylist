using System;
using System.Collections;
using System.Collections.Generic;
using System.Windows;
using System.Linq;
using System.Text;
using System.Windows.Media.Imaging;

using Livet;

namespace IkaStylist.Models
{
    ///<summary>ブランドを管理するためのクラス</summary>
    public class Brand : NotificationObject
    {
        public Brand()
        {
            this.Name    = "";
            this.imgName = "";
            this.Id      = 0;
        }

        public Brand( Brand input )
        {
            this.Name    = input.Name;
            this.imgName = input.imgName;
            this.Id      = input.Id;
        }

        public Brand(string _name, string _img, int _id)
        {
            this.Name    = _name;
            this.imgName = _img;
            this.Id      = _id;
        }
        
        private string _Name;
        public string Name
        {
            get
            { return _Name; }
            set
            {
                if (_Name == value)
                    return;
                _Name = value;
                RaisePropertyChanged();
            }
        }
        
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

        public BitmapImage BitmapImg
        {
            get
            {
                return IkaUtil.getBrandBitmapByImgName( imgName );
            }
            set
            {
                RaisePropertyChanged();
            }
        }
    }
}//namespace IkaStylist.Models
