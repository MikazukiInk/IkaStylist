using System;
using System.Collections;
using System.Collections.Generic;
using System.Windows;
using System.Drawing;
using System.Windows.Media.Imaging;
using System.Linq;
using System.Text;

using Livet;

namespace IkaStylist.Models
{
    ///<summary>検索結果で選択されたデータを管理するためのクラス</summary>
    public class SelectionManager : NotificationObject
    {
        public SelectionManager()
        {
        	init();
            myAssembly = System.Reflection.Assembly.GetExecutingAssembly();
        }

        public void update( Coordinate current )
        {
			selectItem = current;
        }

        public void init()
        {
			selectItem = null;
        }

        private System.Reflection.Assembly myAssembly;

        // メンバ変数.
        private Coordinate _selectItem;
        public  Coordinate selectItem
        {
        	get
        	{
        		return _selectItem;
        	}
        	set
        	{
        		if (_selectItem == value)
                    return;
                if (value == null)
                {
                    _selectItem = null;
                }
                else
                {
                    _selectItem = new Coordinate(value);
                }
                updateAllData();
                RaisePropertyChanged();
        	}
        }

        private void updateAllData()
        {
            HeadGearName = "";
            ClothGearName = "";
            ShoesGearName = "";

            HeadGearImg     = new BitmapImage();
            HeadMainGearImg = new BitmapImage();
            HeadSubGear1Img = new BitmapImage();
            HeadSubGear2Img = new BitmapImage();
            HeadSubGear3Img = new BitmapImage();

            ClothGearImg     = new BitmapImage();
            ClothMainGearImg = new BitmapImage();
            ClothSubGear1Img = new BitmapImage();
            ClothSubGear2Img = new BitmapImage();
            ClothSubGear3Img = new BitmapImage();

            ShoesGearImg     = new BitmapImage();
            ShoesMainGearImg = new BitmapImage();
            ShoesSubGear1Img = new BitmapImage();
            ShoesSubGear2Img = new BitmapImage();
            ShoesSubGear3Img = new BitmapImage();
        }

        //呼び出し専用.
        
        public string HeadGearName
        {
        	get
        	{
        		string ret = "アタマ：";
        		if( _selectItem != null){
        			ret = ret + _selectItem.HeadGear.Name;
        		}
        		return ret;
        	}
        	set
            {
                RaisePropertyChanged();
            }
        }

        public string ClothGearName
        {
        	get
        	{
        		string ret = "フク：";
        		if( _selectItem != null){
        			ret = ret + _selectItem.ClothGear.Name;
        		}
        		return ret;
        	}
            set
            {
                RaisePropertyChanged();
            }
        }

        public string ShoesGearName
        {
        	get
        	{
        		string ret = "クツ：";
        		if( _selectItem != null){
        			ret = ret + _selectItem.ShoesGear.Name;
        		}
        		return ret;
        	}
            set
            {
                RaisePropertyChanged();
            }
        }

        //******************* ギアの埋め込みデータ読み込み *******************//

        private BitmapImage getGearBitmap(string filename)
        {
            string projectName = myAssembly.GetName().Name + ".Resources.";
            string pngFile = null;

            if (filename != null)
            {
                pngFile = projectName + filename + ".png";
            }
            return getBitmap(pngFile);
        }

        private BitmapImage getGearPowerBitmap(int id)
        {
            string projectName = myAssembly.GetName().Name + ".Resources.";
            string pngFile = null;
            if (id != -1)
            {
                pngFile = projectName + EnumLiteralAttribute.GetLiteral((GearPowerImg)(id)) + "2.png";
            }
            return getBitmap(pngFile);
        }

        private BitmapImage getBitmap(string filePath)
        {
            if (filePath == null)
            {
                return new BitmapImage();//何も表示しない.
            }
            System.IO.Stream resourcePath = myAssembly.GetManifestResourceStream(filePath);
            BitmapImage bitmap = new BitmapImage();
            bitmap.BeginInit();
            bitmap.StreamSource = resourcePath;
            bitmap.EndInit();
            return bitmap;
        }

        //************** フク ******************//
        public BitmapImage HeadGearImg
        {
        	get
            {
                if (_selectItem != null)
                {
                    return getGearBitmap(_selectItem.HeadGear.imgName);
                }
                else
                {
                    return getGearBitmap(null);
                }
            }
            set
            {
                RaisePropertyChanged();
            }
        }

        public BitmapImage HeadMainGearImg
        {
            get
            {
                if (_selectItem != null)
                {
                    return getGearPowerBitmap(_selectItem.HeadGear.MainPower.Id);
                }
                else
                {
                    return getGearPowerBitmap(-1);
                }
            }
            set
            {
                RaisePropertyChanged();
            }
        }

        public BitmapImage HeadSubGear1Img
        {
            get
            {
                if (_selectItem != null)
                {
                    return getGearPowerBitmap(_selectItem.HeadGear.SubPower1.Id);
                }
                else
                {
                    return getGearPowerBitmap(-1);
                }
            }
            set
            {
                RaisePropertyChanged();
            }
        }

        public BitmapImage HeadSubGear2Img
        {
            get
            {
                if (_selectItem != null)
                {
                    return getGearPowerBitmap(_selectItem.HeadGear.SubPower2.Id);
                }
                else
                {
                    return getGearPowerBitmap(-1);
                }
            }
            set
            {
                RaisePropertyChanged();
            }
        }

        public BitmapImage HeadSubGear3Img
        {
            get
            {
                if (_selectItem != null)
                {
                    return getGearPowerBitmap(_selectItem.HeadGear.SubPower3.Id);
                }
                else
                {
                    return getGearPowerBitmap(-1);
                }
            }
            set
            {
                RaisePropertyChanged();
            }
        }

        //************** フク ******************//
        public BitmapImage ClothGearImg
        {
            get
            {
                if (_selectItem != null)
                {
                    return getGearBitmap(_selectItem.ClothGear.imgName);
                }
                else
                {
                    return getGearBitmap(null);
                }
            }
            set
            {
                RaisePropertyChanged();
            }
        }

        public BitmapImage ClothMainGearImg
        {
            get
            {
                if (_selectItem != null)
                {
                    return getGearPowerBitmap(_selectItem.ClothGear.MainPower.Id);
                }
                else
                {
                    return getGearPowerBitmap(-1);
                }
            }
            set
            {
                RaisePropertyChanged();
            }
        }

        public BitmapImage ClothSubGear1Img
        {
            get
            {
                if (_selectItem != null)
                {
                    return getGearPowerBitmap(_selectItem.ClothGear.SubPower1.Id);
                }
                else
                {
                    return getGearPowerBitmap(-1);
                }
            }
            set
            {
                RaisePropertyChanged();
            }
        }

        public BitmapImage ClothSubGear2Img
        {
            get
            {
                if (_selectItem != null)
                {
                    return getGearPowerBitmap(_selectItem.ClothGear.SubPower2.Id);
                }
                else
                {
                    return getGearPowerBitmap(-1);
                }
            }
            set
            {
                RaisePropertyChanged();
            }
        }

        public BitmapImage ClothSubGear3Img
        {
            get
            {
                if (_selectItem != null)
                {
                    return getGearPowerBitmap(_selectItem.ClothGear.SubPower3.Id);
                }
                else
                {
                    return getGearPowerBitmap(-1);
                }
            }
            set
            {
                RaisePropertyChanged();
            }
        }

        //************** シューズ ******************//
        public BitmapImage ShoesGearImg
        {
            get
            {
                if (_selectItem != null)
                {
                    return getGearBitmap(_selectItem.ShoesGear.imgName);
                }
                else
                {
                    return getGearBitmap(null);
                }
            }
            set
            {
                RaisePropertyChanged();
            }
        }
        
        public BitmapImage ShoesMainGearImg
        {
            get
            {
                if (_selectItem != null)
                {
                    return getGearPowerBitmap(_selectItem.ShoesGear.MainPower.Id);
                }
                else
                {
                    return getGearPowerBitmap(-1);
                }
            }
            set
            {
                RaisePropertyChanged();
            }
        }

        public BitmapImage ShoesSubGear1Img
        {
            get
            {
                if (_selectItem != null)
                {
                    return getGearPowerBitmap(_selectItem.ShoesGear.SubPower1.Id);
                }
                else
                {
                    return getGearPowerBitmap(-1);
                }
            }
            set
            {
                RaisePropertyChanged();
            }
        }

        public BitmapImage ShoesSubGear2Img
        {
            get
            {
                if (_selectItem != null)
                {
                    return getGearPowerBitmap(_selectItem.ShoesGear.SubPower2.Id);
                }
                else
                {
                    return getGearPowerBitmap(-1);
                }
            }
            set
            {
                RaisePropertyChanged();
            }
        }

        public BitmapImage ShoesSubGear3Img
        {
            get
            {
                if (_selectItem != null)
                {
                    return getGearPowerBitmap(_selectItem.ShoesGear.SubPower3.Id);
                }
                else
                {
                    return getGearPowerBitmap(-1);
                }
            }
            set
            {
                RaisePropertyChanged();
            }
        }
    }
}//namespace IkaStylist.Models
