using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Windows;
using System.Linq;
using System.Text;
using CsvHelper;


using Livet;

namespace IkaStylist.Models
{
    ///<summary>ギアを管理するためのクラス</summary>
    public class Gear : NotificationObject
    {
        static readonly int PowersCount = EnumExtension<Power>.Length();
        static readonly int PartsCount = EnumExtension<Parts>.Length();

        static public Hashtable NameToIdTable = new Hashtable();
        static public void initTable()
        {
            for (int ii = 0; ii < Gear.PowersCount; ii++)
            {
                NameToIdTable.Add(EnumLiteralAttribute.GetLiteral((Power)ii), ii);
            }
        }
        static public string IdToName(int id)
        {
            return EnumLiteralAttribute.GetLiteral((Gear.Power)(id));
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
                _Name = value;
                RaisePropertyChanged();
            }
        }
        #endregion

        #region MainPower変更通知プロパティ
        private string _MainPower;

        public string MainPower
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
        #endregion

        #region SubPower1変更通知プロパティ
        private string _SubPower1;

        public string SubPower1
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
        #endregion

        #region SubPower2変更通知プロパティ
        private string _SubPower2;

        public string SubPower2
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
        #endregion

        #region SubPower3変更通知プロパティ
        private string _SubPower3;

        public string SubPower3
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
        #endregion

        ///<summary>部位</summary>
        public enum Parts
        {
            [EnumLiteral("アタマ")]
            Head,
            [EnumLiteral("フク")]
            Cloth,
            [EnumLiteral("クツ")]
            Shoes,
        }

        #region ギアパワー列挙型
        public enum Power
        {
            [EnumLiteral("なし")]
            None,	//0
            [EnumLiteral("攻撃力アップ")]
            DamageUp,	//1
            [EnumLiteral("防御力アップ")]
            DefenseUp,	//2
            [EnumLiteral("インク効率アップ(メイン)")]
            InkSaverMain,	//3
            [EnumLiteral("インク効率アップ(サブ)")]
            InkSaverSub,	//4
            [EnumLiteral("インク回復力アップ")]
            InkRecoveryUp,	//5
            [EnumLiteral("ヒト移動速度アップ")]
            RunSpeedUp,	//6
            [EnumLiteral("イカダッシュ速度アップ")]
            SwimSpeedUp,	//7
            [EnumLiteral("スペシャル増加量アップ")]
            SpecialChargeUp,	//8
            [EnumLiteral("スペシャル時間延長")]
            SpecialDurationUp,	//9
            [EnumLiteral("復活時間短縮")]
            QuickRespawn,	//10
            [EnumLiteral("スペシャル減少量ダウン")]
            SpecialSaver,	//11
            [EnumLiteral("スーパージャンプ時間短縮")]
            QuickSuperJump,	//12
            [EnumLiteral("ボム飛距離アップ")]
            BombRangeUp,	//13
            [EnumLiteral("スタートダッシュ")]
            OpeningGambit,	//14
            [EnumLiteral("ラストスパート")]
            LastDitchEffort,	//15
            [EnumLiteral("逆境強化")]
            Tenacity,	//16
            [EnumLiteral("カムバック")]
            Comeback,	//17
            [EnumLiteral("マーキングガード")]
            ColdBlooded,	//18
            [EnumLiteral("イカニンジャ")]
            NinjaSquid,	//19
            [EnumLiteral("うらみ")]
            Haunt,	//20
            [EnumLiteral("スタートレーダー")]
            Recon,	//21
            [EnumLiteral("ボムサーチ")]
            BombSniffer,	//22
            [EnumLiteral("安全シューズ")]
            InkResistanceUp,	//23
            [EnumLiteral("ステルスジャンプ")]
            StealthJump,	//24

        }
        #endregion

        /// <summary>CSVファイルからギアのデータを取得する</summary>
        /// <param name="parts">部位の指定</param>
        /// <returns>ギアのリスト</returns>
        public static List<Gear> ReadCSV(Gear.Parts parts)
        {
            string fname = parts.ToString();
            return ReadCSV(fname);
        }

        /// <summary>CSVファイルからギアのデータを取得する</summary>
        /// <param name="fname">ファイル名（拡張子なし）</param>
        /// <returns></returns>
        public static List<Gear> ReadCSV(string fname)
        {
        	string fnameEX = fname + ".csv";
            if( !System.IO.Directory.Exists(".\\GearDataCsv\\") ){
                System.IO.Directory.CreateDirectory(".\\GearDataCsv\\");
            }
        	if( !System.IO.File.Exists(".\\GearDataCsv\\" + fnameEX) ) {
        		try
		        {
		            File.Copy(Path.Combine(".\\GearDataCsvDefault", fnameEX), Path.Combine(".\\GearDataCsv", fnameEX));
		        }
		        catch (IOException copyError)
		        {
                    System.Windows.Forms.MessageBox.Show(   "ギアファイルの生成に失敗しました。アプリケーションを終了します。\n" + copyError.Message,
                                                            "エラー",
                                                            System.Windows.Forms.MessageBoxButtons.OK,
                                                            System.Windows.Forms.MessageBoxIcon.Error);
		        }
                System.Windows.Forms.MessageBox.Show(   "ギアファイル [./GearDataCsv" + fnameEX + "] を生成しました。",
                                                        "Information",
                                                        System.Windows.Forms.MessageBoxButtons.OK,
                                                        System.Windows.Forms.MessageBoxIcon.Information);

        	}
            using (var r = new StreamReader(".\\GearDataCsv\\" + fnameEX, Encoding.GetEncoding("SHIFT_JIS")))
            using (var csv = new CsvHelper.CsvReader(r))
            {
                csv.Configuration.HasHeaderRecord = true;// ヘッダーありCSV
                // マッピングルールを登録
                csv.Configuration.RegisterClassMap<CsvMapper>();
                // データを読み出し
                var records = csv.GetRecords<Gear>().ToList();

                return records;
            }
        }

        public static bool WriteCSV(List<Gear> list, string fname)
        {
            var result = true;
            var path = ".\\GearDataCsv\\" + fname + ".csv";
            try
            {
                using (var sw = new System.IO.StreamWriter(path, false, System.Text.Encoding.GetEncoding("Shift_JIS")))
                {
                    //ダブルクォーテーションで囲む
                    Func<string, string> dqot = (str) => { return "\"" + str.Replace("\"", "\"\"") + "\""; };
                    sw.WriteLine("ナマエ" + "," + "メイン" + "," + "サブ１" + "," + "サブ２" + "," + "サブ３");
                    foreach (Gear d in list)
                        sw.WriteLine(dqot(d.Name) + "," + dqot(d.MainPower) + "," + dqot(d.SubPower1) + "," + dqot(d.SubPower2) + "," + dqot(d.SubPower3));
                }

            }
            catch (SystemException ex)
            {
                System.Windows.Forms.MessageBox.Show(ex.Message);
                result = false;
            }
            return result;
        }

        /// <summary>ギアパワーのポイントを格納するクラス</summary>
        public class Points
        {
            public Points()
            {
                this.points = new int[Gear.PowersCount];
            }

            /// <summary>コンストラクタ２</summary>
            /// <param name="gear">ギア</param>
            public Points(Gear gear)
            {
                points[(int)NameToIdTable[gear.MainPower]] += 10;   //メインは10ポイント
                points[(int)NameToIdTable[gear.SubPower1]] += 3;    //サブは3ポイントとして計算
                points[(int)NameToIdTable[gear.SubPower2]] += 3;
                points[(int)NameToIdTable[gear.SubPower3]] += 3;
            }

            /// <summary>各ギアパワーの値を格納する</summary>
            public int[] points = new int[Gear.PowersCount];

            /// <summary>ギアパワーの値を加算</summary>
            /// <param name="c1">ギアパワー値配列１</param>
            /// <param name="c2">ギアパワー値配列２</param>
            /// <returns>加算結果</returns>
            public static Points operator +(Points c1, Points c2)
            {
                var result = new Points();
                for (int i = 0; i < PowersCount; i++)
                {
                    result.points[i] = c1.points[i] + c2.points[i];
                }
                return result;
            }
        }

        /// <summary>部位ごとのギアパワーのポイントを格納し、ギアのIDを記録しておくクラス</summary>
        public class TotalPoints : Points
        {
            public TotalPoints(Points p)
            {
                this.points = p.points;
                this.HeadID = 0;
                this.ClothID = 0;
                this.ShoesID = 0;
            }

            #region HeadIDプロパティ
            private int _HeadID;
            ///<summary>頭ギアのID</summary>
            public int HeadID
            {
                get
                { return _HeadID; }
                set
                {
                    if (_HeadID == value)
                        return;
                    _HeadID = value;
                }
            }
            #endregion

            #region ClothID変更通知プロパティ
            private int _ClothID;
            ///<summary>服ギアのID</summary>
            public int ClothID
            {
                get
                { return _ClothID; }
                set
                {
                    if (_ClothID == value)
                        return;
                    _ClothID = value;
                }
            }
            #endregion

            #region ShoesID変更通知プロパティ
            private int _ShoesID;

            ///<summary>靴ギアのID</summary>
            public int ShoesID
            {
                get
                { return _ShoesID; }
                set
                {
                    if (_ShoesID == value)
                        return;
                    _ShoesID = value;
                }
            }
            #endregion
        }

        ///<summary>結果表示用に部位ごとの名前を格納するクラス</summary>
        public class Equipment : NotificationObject
        {

            public Equipment()
            {
                this.HeadGearName = "";
                this.ClothGearName = "";
                this.ShoesGearName = "";
            }
            public Equipment(string h, string c, string s, string t)
            {
                this.HeadGearName = h;
                this.ClothGearName = c;
                this.ShoesGearName = s;
                this.PowersText = t;
            }

            public static string ResultToText(Gear.TotalPoints result)
            {
                string text = string.Empty; ;

                for (int i = 1; i < result.points.Length; i++)
                {
                    if (1 < result.points[i])
                    {
                        text += EnumLiteralAttribute.GetLiteral((Gear.Power)i);
                        text += string.Format("={0} ", result.points[i]);
                    }
                }

                return text;
            }

            #region HeadGearName変更通知プロパティ
            private string _HeadGearName;

            public string HeadGearName
            {
                get
                { return _HeadGearName; }
                set
                {
                    if (_HeadGearName == value)
                        return;
                    _HeadGearName = value;
                    RaisePropertyChanged();
                }
            }
            #endregion

            #region ClothGearName変更通知プロパティ
            private string _ClothGearName;

            public string ClothGearName
            {
                get
                { return _ClothGearName; }
                set
                {
                    if (_ClothGearName == value)
                        return;
                    _ClothGearName = value;
                    RaisePropertyChanged();
                }
            }
            #endregion

            #region ShoesGearName変更通知プロパティ
            private string _ShoesGearName;

            public string ShoesGearName
            {
                get
                { return _ShoesGearName; }
                set
                {
                    if (_ShoesGearName == value)
                        return;
                    _ShoesGearName = value;
                    RaisePropertyChanged();
                }
            }
            #endregion

            #region PowersText変更通知プロパティ
            private string _PowersText;

            public string PowersText
            {
                get
                { return _PowersText; }
                set
                {
                    if (_PowersText == value)
                        return;
                    _PowersText = value;
                    RaisePropertyChanged();
                }
            }
            #endregion

        }
    }

    ///<summary>CSVファイルへの格納ルール</summary>
    class CsvMapper : CsvHelper.Configuration.CsvClassMap<Gear>
    {
        public CsvMapper()
        {
            Map(x => x.Name).Index(0);
            Map(x => x.MainPower).Index(1);
            Map(x => x.SubPower1).Index(2);
            Map(x => x.SubPower2).Index(3);
            Map(x => x.SubPower3).Index(4);
        }
    }


}
