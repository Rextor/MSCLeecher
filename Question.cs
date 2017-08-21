using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MSC_Leecher
{
    public class Question
    {
        public static string SetQuestion(string body, string title = "")
        {
            QuestionBox qb = new QuestionBox();
            qb.box = new IQuestionBox { Body = body, Title = title };
            qb.ShowDialog();
            return qb.box.Answer;
        }

    }
    public class IQuestionBox
    {
        public string Body { set; get; }
        public string Title { set; get; }
        public string Answer { set; get; }
    }
}
