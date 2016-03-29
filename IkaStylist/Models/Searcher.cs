using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

using Livet;

namespace IkaStylist.Models
{
    ///<summary>コーデの検索を行うクラス</summary>
    public class Searcher : NotificationObject
    {
        private OptManager OptMgr;
        public Searcher(OptManager opt)
        {
        	OptMgr = opt;
        }
        
        ///<summary> </summary>

        public List<Coordinate> CoordinateList     = new List<Coordinate>();
        public List<Coordinate> CoordinateList_Fes = new List<Coordinate>(); //フェスモード用.
        private List<Gear> HeadGear;
        private List<Gear> ClothGear;
        private List<Gear> ShoesGear;
        private List<Gear> FesTGear;
        Thread thread_normal;
        Thread thread_fes;

        /// <summary>検索クラスの初期化
        /// 全パターンのギアパワー値を計算して持っておく
        /// </summary>
        /// <param name="head">アタマのギアリスト</param>
        /// <param name="cloth">フクのギアリスト</param>
        /// <param name="shoes">クツのギアリスト</param>
        public void Init()
        {
            this.CoordinateList.Clear();
            this.CoordinateList_Fes.Clear();

            this.HeadGear  = IOmanager.ReadCSV(GearKind.Head);
            this.ClothGear = IOmanager.ReadCSV(GearKind.Cloth);
            this.ShoesGear = IOmanager.ReadCSV(GearKind.Shoes);
            this.FesTGear  = IOmanager.ReadCSV(GearKind.FesT);
        }

        public void remakeCoordinateListOnThread()
        {
            this.thread_normal = new Thread(new ThreadStart(makeCoordinateList_normal));
            this.thread_normal.IsBackground = true;
            this.thread_normal.Start();

            this.thread_fes = new Thread(new ThreadStart(makeCoordinateList_fes));
            this.thread_fes.IsBackground = true;
            this.thread_fes.Start();
        }

        private void makeCoordinateList_normal()
        {
            makeCoordinateList(this.HeadGear, this.ClothGear, this.ShoesGear, this.CoordinateList);
        }

        private void makeCoordinateList_fes()
        {
            makeCoordinateList(this.HeadGear, this.FesTGear, this.ShoesGear, this.CoordinateList_Fes);
        }

        private void makeCoordinateList( List<Gear> head, List<Gear> cloth, List<Gear> shoes, List<Coordinate> coordinateL )
        {
            for (int h = 0; h < head.Count; h++)
            {
                for (int c = 0; c < cloth.Count; c++)
                {
                    for (int s = 0; s < shoes.Count; s++)
                    {
                        var temp = new Coordinate(head[h], cloth[c], shoes[s]);
                        coordinateL.Add(temp);
                    }
                }
            }
        }

        private List<Coordinate> getCoordinateList4Search()
        {
            if (OptMgr.isFestival)
            {
                if (this.CoordinateList_Fes.Count == 0)
                {
                    //万が一作れていなかった時のため.
                    this.thread_fes.Abort();
                    makeCoordinateList( this.HeadGear, this.FesTGear, this.ShoesGear, this.CoordinateList_Fes );
                }
                return this.CoordinateList_Fes;
            }
            else
            {
                if (this.CoordinateList.Count == 0)
                {
                    //万が一作れていなかった時のため.
                    this.thread_normal.Abort();
                    makeCoordinateList(this.HeadGear, this.ClothGear, this.ShoesGear, this.CoordinateList);
                }
                return this.CoordinateList;
            }
        }

        /// <summary>検索処理実行メソッド</summary>
        /// <param name="request">検索の要求値</param>
        /// <param name="onlyEnhanced">強化済みのみを使用するか？</param>
        /// <returns>ギアパワー配列のリスト</returns>
        public List<Coordinate> Start()
        {
            if (OptMgr.isFestival)
            {
                this.thread_fes.Join();
            }
            else
            {
                this.thread_normal.Join();
            }
            var candidate = new List<Coordinate>(getCoordinateList4Search());

            //「なし」2個までは許容する。それ以上は除外する。
            if (OptMgr.OnlyEnhanced)
            {
                candidate.RemoveAll(x => x.points[0] > 8);
            }

            for (int i = 0; i < OptMgr.Requests.Length; i++)
            {
                if (OptMgr.Requests[i].GearPowerID != 0)
                {
                    candidate.RemoveAll(x => x.points[OptMgr.Requests[i].GearPowerID] < (OptMgr.Requests[i].Point - OptMgr.Tolerance));
                    for (int ci = candidate.Count - 1; ci >= 0; ci--)
                    {
                        if (OptMgr.Tolerance > 0)
                        {
                            if (candidate[ci].points[OptMgr.Requests[i].GearPowerID] < OptMgr.Requests[i].Point)
                            {
                                candidate[ci].IsTolerance[OptMgr.Requests[i].GearPowerID] = true;
                            }
                            else
                            {
                                candidate[ci].IsTolerance[OptMgr.Requests[i].GearPowerID] = false;
                            }
                        }
                        else
                        {
                            candidate[ci].IsTolerance = Enumerable.Repeat<bool>(false, Enum.GetNames(typeof(GearPowerKind)).Length).ToArray();
                        }
                    }

                }
                if (candidate.Count <= 1)
                {
                    break;
                }
            }
            return candidate;
        }

    }
}
