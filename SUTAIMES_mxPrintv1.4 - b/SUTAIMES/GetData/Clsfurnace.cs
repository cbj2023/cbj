using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SUTAIMES.GetData
{
    class Clsfurnace
    {
        Program tSystem;
        public Clsfurnace(Program tSys)
        {
            tSystem = tSys;
            
        }

        //    //节点
        public class Node
        {
            public int X { get; set; }
            public int Y { get; set; }
            public int W { get; set; }
            public int H { get; set; }
            public bool IsUsed { get; set; }
            public Node Right { get; set; }
            public Node Down { get; set; }
            public DataModel Data { get; set; }
        }
        //数据源
        public class DataModel
        {
            public int W { get; set; }
            public int H { get; set; }
            public int Area { get; set; }
            public bool IsUsed { get; set; }
            public bool IsRotate { get; set; }
        }
        public class Core
        {
            private Node m_Root;
            private int m_TemplateWidth;
            private int m_TemplateHeight;
            public List<Clsfurnace.DataModel> Data;
            public Core(List<DataModel> Datas,int TemplateWidth,int TemplateHeight)
            {
                this.Data = Datas;
                this.m_TemplateWidth = TemplateWidth;
                this.m_TemplateHeight = TemplateHeight;
            }
            public Node Paking()
            {
                this.m_Root = new Node() { W = this.m_TemplateWidth, H = this.m_TemplateHeight };
                foreach (var item in this.Data)
                {
                    if (!item.IsUsed)//该数据已经包装
                    {
                        var CurrentNode = this.FindNode(this.m_Root, item);
 
                        if (CurrentNode != null)
                        {
                            this.SplitPlate(CurrentNode, item);
                            item.IsUsed = true;
                        }
                    }
                }
 
                return this.m_Root;
            }
 
            public Node FindNode(Node root, Clsfurnace.DataModel model)
            {
                if (root.IsUsed)
                {
                    //往右摆放是否有空间
                    var node = FindNode(root.Right, model);
                    if (node == null)
                    {
                        //往下摆放是否有空间
                        node = FindNode(root.Down, model);
                    }
                    return node;
                }
                //无需旋转
                else if (model.W <= root.W && model.H <= root.H)
                {
                    return root;
                }
                ////宽高交换
                //else if (model.W <= root.H && model.H <= root.W)
                //{
                //    int Temp = model.W;
                //    model.W = model.H;
                //    model.H = Temp;
                //    model.IsRotate = true;
                //    return root;
                //}
                else
                {
                    return null;
                }
            }
 
            public Node SplitPlate(Node node, Clsfurnace.DataModel model)
            {
                node.IsUsed = true;
                node.Data = model;
                node.Down = new Node { W = node.W, H = node.H - model.H, X = node.X, Y = node.Y + model.H };
                node.Right = new Node { W = node.W - model.W, H = model.H, X = node.X + model.W, Y = node.Y };
 
                return node;
            }
        }
 



    }
}
