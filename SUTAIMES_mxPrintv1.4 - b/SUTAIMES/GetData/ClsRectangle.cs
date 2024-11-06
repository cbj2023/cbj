using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SUTAIMES.GetData
{
    class ClsRectangle
    {
        private int m_TemplateWidth;//矩形宽度
        private int m_TemplateHeight;//矩形高度
        private int m_BianSize = 5;
        private int m_ZuoSize = 10;
        private int m_LineSize = 10;

        private int mLine = 0;
        public ClsLine mclsLine = new ClsLine { line_id = 0, MaxW = 0, MinW = 0 };
        public ClsRectangle(int _TemplateWidth, int _TemplateHeight)
        {
            _TemplateWidth = _TemplateWidth - m_BianSize * 2;
            mclsLine = new ClsLine { line_id = 0, MaxW = 0, MinW = 0 };
            mclsLine.SurH = _TemplateHeight; m_TemplateHeight = _TemplateHeight;
            mclsLine.SurW = _TemplateWidth; m_TemplateWidth = _TemplateWidth;

            mLine = 0;
            mDicList = new Dictionary<int, List<NewNode>>();
            mlist = new List<NewNode>();
            mID = 0;
        }
        public class NewNode
        {
            public int id { get; set; }
            public int X { get; set; }
            public int Y { get; set; }
            public int W { get; set; }
            public int H { get; set; }
            public int Line { get; set; }
        }


        public class ClsLine
        {
            public int line_id { get; set; }
            public int MaxW { get; set; }
            public int MinW { get; set; }
            public int SurW { get; set; }
            public int SurH { get; set; }
            public int MinY { get; set; }

        }

        public NewNode NewPaking(int _H, int _W, ref int _AddH)
        {
            NewNode tnewNode = new NewNode(); int AddH = 0;

            tnewNode = NewSplitPlate(mLine, _H, _W, ref AddH);
            _AddH = AddH;
            return tnewNode;
        }
        Dictionary<int, List<NewNode>> mDicList = new Dictionary<int, List<NewNode>>();
        List<NewNode> mlist = new List<NewNode>();
        int mID = 0;
        private NewNode NewSplitPlate(int _line, int _H, int _W, ref int _AddH)
        {
            NewNode tnewNode = new NewNode(); int tZsize = 0;
            if (mclsLine.SurH < m_TemplateHeight) { tZsize = m_ZuoSize; }
            List<NewNode> tNodesList = new List<NewNode>(); tNodesList = GetList(_line - 1);
            if (tNodesList == null)//第一列
            {
                if (mclsLine.SurH >= _H && mclsLine.SurW >= _W)
                {
                    tnewNode.id = mID + 1; mID = mID + 1;
                    tnewNode.Line = _line;
                    tnewNode.H = _H; tnewNode.W = _W;
                    tnewNode.X = mclsLine.SurW - _W;
                    tnewNode.Y = mclsLine.SurH - _H - m_BianSize - tZsize;
                    mclsLine.SurH = mclsLine.SurH - _H - tZsize;
                    if (mclsLine.MaxW < _W)
                    {
                        mclsLine.MaxW = _W;
                    }
                    mlist.Add(tnewNode);

                }
                else if (mclsLine.SurW >= _W)
                {
                    if (mlist.Count > 0)
                    { int tSurH = mclsLine.SurH / (mlist.Count + 1); foreach (var item in mlist) { item.Y = item.Y + tSurH; } _AddH = tSurH; }
                    mDicList.Add(_line, mlist); mlist = new List<NewNode>(); mclsLine.SurH = m_TemplateHeight;
                    mclsLine.SurW = mclsLine.SurW - mclsLine.MaxW - m_LineSize; mclsLine.MaxW = 0;
                    tnewNode = NewSplitPlate(_line + 1, _H, _W, ref _AddH); mLine = mLine + 1;
                }
                else
                {
                    tnewNode = null;
                }
            }
            else
            {
                int tTX = 0, tTY = 0;
                //if (mclsLine.SurH >= _H)//高度可以放下
                //{

                //    tTY= mclsLine.SurH - _H;
                //    foreach (var item in tNodesList)
                //    {
                //        if (mclsLine.SurW < item.X & item.Y <=tTY)
                //        {
                //            tTX = item.X;
                //            break;
                //        }

                //    }
                //}

                if (mclsLine.SurH >= _H && mclsLine.SurW >= _W)//可以排版
                {
                    tnewNode.id = mID + 1; mID = mID + 1;
                    tnewNode.Line = _line;

                    tnewNode.H = _H; tnewNode.W = _W;
                    if (tTX > 0)
                    {
                        tnewNode.X = tTX - _W;
                    }
                    else
                    {
                        tnewNode.X = mclsLine.SurW - _W;
                    }
                    tnewNode.Y = mclsLine.SurH - _H - m_BianSize - tZsize;
                    mclsLine.SurH = mclsLine.SurH - _H - tZsize;
                    if (mclsLine.MaxW < _W)
                    {
                        mclsLine.MaxW = _W;
                    }
                    mlist.Add(tnewNode);

                }
                else if (mclsLine.SurW >= _W)//另一列
                {
                    if (mlist.Count > 0)
                    { int tSurH = mclsLine.SurH / (mlist.Count + 1); foreach (var item in mlist) { item.Y = item.Y + tSurH; } _AddH = tSurH; }
                    mDicList.Add(_line, mlist); mLine = mLine + 1; mlist = new List<NewNode>(); mclsLine.SurH = m_TemplateHeight;
                    mclsLine.SurW = mclsLine.SurW - mclsLine.MaxW - m_LineSize; mclsLine.MaxW = 0;
                    tnewNode = NewSplitPlate(_line + 1, _H, _W, ref _AddH); mLine = mLine + 1;
                }
                else
                {
                    tnewNode = null;
                }
            }

            return tnewNode;
        }
        private List<NewNode> GetList(int _line)
        {
            try
            {
                List<NewNode> tNodes = new List<NewNode>();
                tNodes = mDicList[_line];

                return tNodes;
            }
            catch { return null; }
        }

    }
}
