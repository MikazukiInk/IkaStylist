﻿using System;
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
    ///<summary>CSVファイルへの格納ルール</summary>
    class CsvMapper : CsvHelper.Configuration.CsvClassMap<Gear>
    {
        public CsvMapper()
        {
            Map(x => x.Name).Index(0);
            Map(x => x.MainPowerName).Index(1);
            Map(x => x.SubPower1Name).Index(2);
            Map(x => x.SubPower2Name).Index(3);
            Map(x => x.SubPower3Name).Index(4);
            Map(x => x.LastUpdated).Index(5);
        }
    }

    ///<summary>ギアを管理するためのクラス</summary>
    public class IOmanager
    {
        private const string gearFileDefaultFolder = ".\\GearDataCsvDefault";
        private const string gearFileFolder = ".\\GearDataCsv\\";
        private const string gearFileExtension = ".csv";

        /// <summary>CSVファイルからギアのデータを取得する</summary>
        /// <param name="parts">部位の指定</param>
        /// <returns>ギアのリスト</returns>
        public static List<Gear> ReadCSV(GearKind parts)
        {
            string fname = parts.ToString();
            return ReadCSV(fname);
        }

        /// <summary>CSVファイルからギアのデータを取得する</summary>
        /// <param name="fname">ファイル名（拡張子なし）</param>
        /// <returns></returns>
        public static List<Gear> ReadCSV(string fname)
        {
            string fnameEX = fname + gearFileExtension;//ファイル名
            string FolderPath;


            if (!ApplicationDeployment.IsNetworkDeployed)//ClickOnceでのインストールではない
            {
                FolderPath = gearFileFolder;
            }
            else//ClickOnceでのインストール
            {
                FolderPath = ApplicationDeployment.CurrentDeployment.DataDirectory + gearFileFolder;
            }

            if (!System.IO.Directory.Exists(FolderPath))
            {
                System.IO.Directory.CreateDirectory(FolderPath);
            }
            if (!System.IO.File.Exists(FolderPath + fnameEX))
            {
                try
                {
                    File.Copy(Path.Combine(gearFileDefaultFolder, fnameEX), Path.Combine(FolderPath, fnameEX));
                }
                catch (IOException copyError)
                {
                    System.Windows.Forms.MessageBox.Show("ギアファイルの生成に失敗しました。アプリケーションを終了します。\n" + copyError.Message,
                                                            "エラー",
                                                            System.Windows.Forms.MessageBoxButtons.OK,
                                                            System.Windows.Forms.MessageBoxIcon.Error);
                }
                System.Windows.Forms.MessageBox.Show("ギアファイル [./GearDataCsv" + fnameEX + "] を生成しました。",
                                                        "Information",
                                                        System.Windows.Forms.MessageBoxButtons.OK,
                                                        System.Windows.Forms.MessageBoxIcon.Information);

            }


            using (var r = new StreamReader(FolderPath + fnameEX, Encoding.GetEncoding("SHIFT_JIS")))
            using (var csv = new CsvHelper.CsvReader(r))
            {
                csv.Configuration.HasHeaderRecord = true;// ヘッダーありCSV
                csv.Configuration.WillThrowOnMissingField = false;
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
            var path = gearFileFolder + fname + gearFileExtension;

            if (!ApplicationDeployment.IsNetworkDeployed)//ClickOnceでのインストールではない
            {
                path = gearFileFolder + fname + gearFileExtension;
            }
            else//ClickOnceでのインストール
            {
                path = ApplicationDeployment.CurrentDeployment.DataDirectory + gearFileFolder + fname + gearFileExtension; ;
            }

            try
            {
                using (var sw = new System.IO.StreamWriter(path, false, System.Text.Encoding.GetEncoding("Shift_JIS")))
                {
                    //ダブルクォーテーションで囲む
                    Func<string, string> dqot = (str) => { return "\"" + str.Replace("\"", "\"\"") + "\""; };
                    sw.WriteLine("ナマエ" + "," + "メイン" + "," + "サブ１" + "," + "サブ２" + "," + "サブ３" + "," + "最終更新日");
                    foreach (Gear d in list)
                        sw.WriteLine(dqot(d.Name) + "," + dqot(d.MainPower.Name) + "," + dqot(d.SubPower1.Name)
                            + "," + dqot(d.SubPower2.Name) + "," + dqot(d.SubPower3.Name) + "," + dqot(d.LastUpdated));
                }

            }
            catch (SystemException ex)
            {
                System.Windows.Forms.MessageBox.Show(ex.Message);
                result = false;
            }
            return result;
        }
    }
}//namespace IkaStylist.Models
