﻿using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using System.Windows.Forms;
using System.Configuration;
using System.Linq;

namespace FootBall
{
    public partial class Form1 : Form
    {
        public const int LevelCount= 20;
        List<FootBallTeams> ListFootBallTeams = new List<FootBallTeams>();
        FootBallTeams footBallTeams2019 = new FootBallTeams();
        HtmlWeb webClient = new HtmlWeb();
        int endYear = 2022;
        public Form1()
        {
            InitializeComponent();
        }
      

        private void button1_Click(object sender, EventArgs e)
        {
            double AvgWinBall = default;
            double AvgLoseBall = default;
            int RealWinGameCount = default;
            double ClearAvg = default;
            double WinRate = default;
            //取得下拉選單值進行搜尋
            int? cbYearValue = null;
            string cbTeamValue = null;

            if (cbYears.SelectedIndex!=0)
                cbYearValue =Convert.ToInt32(cbYears.SelectedItem);
            if (cbTeam.SelectedIndex != 0)
                cbTeamValue = cbTeam.SelectedItem.ToString();
            


            var query = (from lfb in ListFootBallTeams
                         where (cbYearValue==null || lfb.Years== cbYearValue) && (cbTeamValue == null || lfb.TeamName == cbTeamValue)
                         orderby lfb.TeamName, lfb.Years
                         select lfb).ToList();
           

            foreach (var q in query)
            {
                AvgWinBall = Math.Round(Convert.ToDouble(q.WinBall) /Convert.ToDouble(q.TotalGames),2);
                AvgLoseBall = Math.Round(Convert.ToDouble(q.LoseBall) / Convert.ToDouble(q.TotalGames), 2);
                RealWinGameCount = q.WinGames - (q.TieGames + q.LoseGames);
                ClearAvg = Math.Round(Convert.ToDouble(q.SubtractBall) / Convert.ToDouble(q.TotalGames),2);
                WinRate = Math.Round(Convert.ToDouble(q.WinGames) /Convert.ToDouble(q.TotalGames),2)*100;
                lvShow.Items.Add(new ListViewItem(new string[] { q.Years.ToString(), q.Level.ToString(), q.TeamName,q.TotalGames.ToString(),q.WinGames.ToString(),q.LoseGames.ToString(),q.TieGames.ToString(), RealWinGameCount.ToString(), q.WinBall.ToString(),q.LoseBall.ToString(),q.SubtractBall.ToString(),AvgWinBall.ToString(),AvgLoseBall.ToString(), ClearAvg.ToString(), WinRate.ToString()+"%"
                    , q.Years.ToString() }));
            }

            //抓出所有不重複隊伍
            var DistinctTeams = (from m in query
                                 orderby m.TeamName, m.Years
                                 select new
                                 {
                                     m.TeamName
                                 }
               ).Distinct().ToList();

            string TeamName = default;
            double AvgLv = default;
            int TeamTotalGames = default;
            
            int TeamWinGames = default;
            int TeamTieGames = default;
            int TeamLoseGames = default;
            int TeamWinBall = default;
            int TeamLoseBall = default;
            int TeamClerWin = default;
            double AvgTeamWinBall = default;
            double AvgTeamLoseBall = default;
            int RealTeamWinGameCount = default;
            double TeamClearAvg = default;
            double TeamWinRate = default;

            //把不重複隊伍進行groupby計算
            foreach (var d in DistinctTeams)
            {
                TeamName = d.TeamName;
                AvgLv = ListFootBallTeams.Where(x => x.TeamName == d.TeamName).Average(y => y.Level);
                AvgLv = Math.Round(AvgLv,2);
                TeamTotalGames= ListFootBallTeams.Where(x => x.TeamName == d.TeamName).Sum(y => y.TotalGames);
                TeamWinGames = ListFootBallTeams.Where(x => x.TeamName == d.TeamName).Sum(y => y.WinGames);
                TeamTieGames = ListFootBallTeams.Where(x => x.TeamName == d.TeamName).Sum(y => y.TieGames);
                TeamLoseGames = ListFootBallTeams.Where(x => x.TeamName == d.TeamName).Sum(y => y.LoseGames);
                TeamWinBall = ListFootBallTeams.Where(x => x.TeamName == d.TeamName).Sum(y => y.WinBall);
                TeamLoseBall = ListFootBallTeams.Where(x => x.TeamName == d.TeamName).Sum(y => y.LoseBall);
                TeamClerWin= ListFootBallTeams.Where(x => x.TeamName == d.TeamName).Sum(y =>Convert.ToInt32(y.SubtractBall));
                AvgTeamWinBall = Math.Round(Convert.ToDouble(TeamWinBall) / Convert.ToDouble(TeamTotalGames), 2);
                AvgTeamLoseBall = Math.Round(Convert.ToDouble(TeamLoseBall) / Convert.ToDouble(TeamTotalGames), 2);
                RealTeamWinGameCount = TeamWinGames - (TeamTieGames+ TeamLoseGames);
                TeamClearAvg =Math.Round(Convert.ToDouble(TeamClerWin) / Convert.ToDouble(TeamTotalGames),2);
                TeamWinRate= Math.Round(Convert.ToDouble(TeamWinGames) / Convert.ToDouble(TeamTotalGames), 2)*100;
                //TeamClearRate=Math.Round(Convert.ToDouble(RealTeamWinGameCount) / Convert.ToDouble(TeamTotalGames), 2)*100;
                lvStatistics.Items.Add(new ListViewItem(new string[] { TeamName, AvgLv.ToString()
                    , TeamTotalGames.ToString(),TeamWinGames.ToString(),TeamLoseGames.ToString(),TeamTieGames.ToString(),RealTeamWinGameCount.ToString(),
                TeamWinBall.ToString(),TeamLoseBall.ToString(),TeamClerWin.ToString(),AvgTeamWinBall.ToString(),AvgTeamLoseBall.ToString(),TeamClearAvg.ToString(),TeamWinRate.ToString()+"%"}));
            }

        }

        private void Form1_Load(object sender, EventArgs e)
        {

            lvShow.View = View.Details;
            lvShow.GridLines = true;
            lvShow.FullRowSelect = true;
            lvShow.Columns.Add("年份", 100);
            lvShow.Columns.Add("排名", 100);
            lvShow.Columns.Add("隊名", 100);
            lvShow.Columns.Add("總場", 100);
            lvShow.Columns.Add("贏局", 100);
            lvShow.Columns.Add("輸局", 100);
            lvShow.Columns.Add("和局", 100);
            lvShow.Columns.Add("勝-(和+負)", 150);
            lvShow.Columns.Add("進球", 100);
            lvShow.Columns.Add("失球", 100);
            lvShow.Columns.Add("淨勝", 100);
            lvShow.Columns.Add("平均每場得分", 150);
            lvShow.Columns.Add("平均每場失分", 150);
            lvShow.Columns.Add("平均淨勝", 150);
            lvShow.Columns.Add("勝率", 100);
            lvShow.Columns.Add("年份", 100);

            lvStatistics.View= View.Details;
            lvStatistics.GridLines = true;
            lvStatistics.Columns.Add("隊名", 100);
            lvStatistics.Columns.Add("平均名次", 100);
            lvStatistics.Columns.Add("歷年總場", 100);
            lvStatistics.Columns.Add("歷年贏局", 100);            
            lvStatistics.Columns.Add("歷年輸局", 100);
            lvStatistics.Columns.Add("歷年和局", 100);
            lvStatistics.Columns.Add("歷年勝-(和+負)", 150);
            lvStatistics.Columns.Add("歷年進球", 100);
            lvStatistics.Columns.Add("歷年失球", 100);
            lvStatistics.Columns.Add("歷年淨勝", 100);
            lvStatistics.Columns.Add("歷年平均得分", 150);
            lvStatistics.Columns.Add("歷年平均失分", 150);
            lvStatistics.Columns.Add("歷年平均淨勝", 150);
            lvStatistics.Columns.Add("歷年勝率", 100);


            for (int beginYear = 2019; beginYear < endYear; beginYear++)
                LoadYearsUrl(beginYear);

            var DistinctYears = (from m in ListFootBallTeams
                                 orderby m.TeamName, m.Years
                                 select new
                                 {
                                     m.Years
                                 }
           ).Distinct().ToList();

            var DistinctTeams = (from m in ListFootBallTeams
                                 orderby m.TeamName, m.Years
                                 select new
                                 {
                                     m.TeamName
                                 }
              ).Distinct().ToList();

            cbYears.Items.Add("全部");
            cbTeam.Items.Add("全部");
            foreach (var y in DistinctYears)
                cbYears.Items.Add(y.Years);
            foreach (var t in DistinctTeams)
            cbTeam.Items.Add(t.TeamName);
            cbYears.SelectedIndex = 0;
            cbTeam.SelectedIndex = 0;
        }
        private void LoadYearsUrl(int year)
        {
            HtmlAgilityPack.HtmlDocument doc = default;
            if (year == 2019)
            {
                 doc = webClient.Load(ConfigurationManager.AppSettings["PremierLeague2019"]);
                
            }
            else if(year == 2020)
            {
                doc = webClient.Load(ConfigurationManager.AppSettings["PremierLeague2020"]);
            }
            else if(year==2021)
            {
                doc = webClient.Load(ConfigurationManager.AppSettings["PremierLeague2021"]);
            }
            GetGameData(doc, year);
        }

        
        private void GetGameData(HtmlAgilityPack.HtmlDocument doc,int year)
        {

            int Years, Level, TotalGames, WinGames, LoseGames, TieGames, WinBall, LoseBall;
            string TeamName;
            string SubtractBall;           
            int getXpathIndex2021 =0;
            //取2021位置不同
            if (year==2021)
                getXpathIndex2021 = 2;
            for (int i = 2; i < LevelCount + 2; i++)
            {
                Years = year;
                                                                       //2021//*[@id="mw-content-text"]/div[1]/table[7]/tbody/tr[2]
                Level = Convert.ToInt32(doc.DocumentNode.SelectNodes($"//*[@id='mw-content-text']/div[1]/table[{5+ getXpathIndex2021}]/tbody/tr[{i}]/td")[0].InnerHtml);
                                                          //2021//*[@id="mw-content-text"]/div[1]/table[7]/tbody/tr[2]/th
                TeamName = doc.DocumentNode.SelectNodes($"//*[@id='mw-content-text']/div[1]/table[{5 + getXpathIndex2021}]/tbody/tr[{i}]/th")[0].InnerText;
                TeamName = TeamName.Replace("(C)", "").Replace("(R)", "").Replace("\n","").Trim();
                TotalGames = Convert.ToInt32(doc.DocumentNode.SelectNodes($"//*[@id='mw-content-text']/div[1]/table[{5 + getXpathIndex2021}]/tbody/tr[{i}]/td")[1].InnerHtml);
                WinGames = Convert.ToInt32(doc.DocumentNode.SelectNodes($"//*[@id='mw-content-text']/div[1]/table[{5 + getXpathIndex2021}]/tbody/tr[{i}]/td")[2].InnerHtml);
                TieGames = Convert.ToInt32(doc.DocumentNode.SelectNodes($"//*[@id='mw-content-text']/div[1]/table[{5 + getXpathIndex2021}]/tbody/tr[{i}]/td")[3].InnerHtml);
                LoseGames = Convert.ToInt32(doc.DocumentNode.SelectNodes($"//*[@id='mw-content-text']/div[1]/table[{5 + getXpathIndex2021}]/tbody/tr[{i}]/td")[4].InnerHtml);
                WinBall = Convert.ToInt32(doc.DocumentNode.SelectNodes($"//*[@id='mw-content-text']/div[1]/table[{5 + getXpathIndex2021}]/tbody/tr[{i}]/td")[5].InnerHtml);
                LoseBall = Convert.ToInt32(doc.DocumentNode.SelectNodes($"//*[@id='mw-content-text']/div[1]/table[{5 + getXpathIndex2021}]/tbody/tr[{i}]/td")[6].InnerHtml);
                SubtractBall = (doc.DocumentNode.SelectNodes($"//*[@id='mw-content-text']/div[1]/table[{5 + getXpathIndex2021}]/tbody/tr[{i}]/td")[7].InnerHtml).Replace("&#8722;","-").Trim();
                ListFootBallTeams.Add(new FootBallTeams { Years=Years, Level = Level, TeamName = TeamName, TotalGames = TotalGames, WinGames = WinGames, TieGames = TieGames, LoseGames = LoseGames, WinBall = WinBall, LoseBall = LoseBall, SubtractBall = SubtractBall });
            }
        }

    }
}
